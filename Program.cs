using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using SynovianEmpireDiscordBot.Modules;
using SynovianEmpireDiscordBot.BotData;
using SynovianEmpireDiscordBot.Libraries;
using SynovianEmpireDiscordBot.Windows;
using SynovianEmpireDiscordBot.BotClasses;
using SynovianEmpireDiscordBot.Firebase;
using SynovianEmpireDiscordBot.TCP;

namespace SynovianEmpireDiscordBot
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        // Retrieve client and CommandService instance via ctor
        public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider service)
        {
            _commands = commands;
            _client = client;
            _services = service;
        }

        public async Task InstallCommandsAsync()
        {
            // Hook the MessageReceived event into our command handler
            _client.MessageReceived += HandleCommandAsync;
            

            // Here we discover all of the command modules in the entry 
            // assembly and load them. Starting from Discord.NET 2.0, a
            // service provider is required to be passed into the
            // module registration method to inject the 
            // required dependencies.
            await _commands.AddModuleAsync<InfoModule>(_services);
            await _commands.AddModuleAsync<CombatModule>(_services);
            await _commands.AddModuleAsync<ServerModule>(_services);
            await _commands.AddModuleAsync<AbilityModule>(_services);
            await _commands.AddModuleAsync<PermissionsModule>(_services);
            await _commands.AddModuleAsync<RandomModule>(_services);
            await _commands.AddModuleAsync<CharacterModule>(_services);

            _commands.Log += Program.LogHandlerAsync;
           //await _commands.AddModuleAsync<AudioModule>(_services);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;

            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (!(message.HasCharPrefix('!', ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

           

            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(_client, message);

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.
            await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _services);
        }        
    }

    public partial class Program
    {
        // Character stuff
        static public readonly AbilityLibrary abilityLibrary = new AbilityLibrary();
        static public readonly CharacterLibrary characterLibrary = new CharacterLibrary();

        // Discord.Net
        static private DiscordSocketClient _client;
        static private CommandHandler _commandHandler;
        static private CommandService _commands;
        static public bool IsRunning = true;

        // Firebase
        static private bool fireBaseEnabled = false;
        static public FirebaseManager firebaseManager;

        // Forms
        static public MainWindow hostWindow;

        // Trello
        static public string TrelloLink = "";

        // TCP
        static TCPServer tcpServer;
        static bool tcpEnabled = false;

        // Services
        public IServiceProvider BuildServiceProvider() => new ServiceCollection()
        .AddSingleton(_client)
        .AddSingleton(_commands)
        .AddSingleton(new AudioService())
        .AddSingleton(characterLibrary)
        .AddSingleton<CommandHandler>()
        .BuildServiceProvider();

        [STAThread]
        public static void Main(string[] args)
        {
            new Program().MainAsync(args).Wait();
            hostWindow = new MainWindow();
            Application.Run(hostWindow);
            if (tcpEnabled) tcpServer.ShutdownTCP();
        }

        public async Task MainAsync(string[] args)
        {
#if DEBUG
            //fireBaseEnabled = true;
            tcpEnabled = true;
            goto SkipArgs;
#endif
            foreach (string str in args)
            {
                if (str == "firebase")
                {
                    fireBaseEnabled = true;
                }
                if(str == "tcp")
                {
                    tcpEnabled = true;
                }
            }

            SkipArgs:

            _client = new DiscordSocketClient();
            _client.Log += Log;
            _client.Ready += OnReady;
            _client.LoggedIn += OnLoggin;
            _client.Connected += OnConnected;
            _client.Disconnected += OnDisconnected;

            _commands = new CommandService();

            _commandHandler = new CommandHandler(_client, _commands, BuildServiceProvider());

            InfoImportClass.LoadBlackList();
            InfoImportClass.LoadWhiteList();
            InfoImportClass.LoadAbilityLibrary();
            TrelloLink = InfoImportClass.GetTrelloLink();


            if(fireBaseEnabled)
            {
                firebaseManager = new FirebaseManager();
            }
            if(tcpEnabled)
            {
                tcpServer = new TCPServer();
            }

            if (InfoImportClass.LoadDiscordKey(out string discordKey) == false)
            {
                await Log(new LogMessage(LogSeverity.Error,"", "Failed to load discord key, bot thus cannot run."));
                return;
            }
            else
            {
                await _client.LoginAsync(TokenType.Bot,
                    discordKey);                

                await _client.StartAsync();

                await _commandHandler.InstallCommandsAsync();
            }
        }
                
        #region Helpers
        public static ulong GetBotID() { return _client.CurrentUser.Id; }

        public static IUser GetBotUser() { return _client.CurrentUser; }

        public static DiscordSocketClient GetClient() { return _client; }

        public static bool IsTCPEnabled() { return tcpEnabled; }

        static public List<CommandInfo> GetBotCommands()
        {
            return _commands.Commands.ToList();
        }        
        
        static public IReadOnlyCollection<SocketGuild> GetAllConnectedGuilds()
        {
            return  _client.Guilds;
        }

        static public IReadOnlyCollection<SocketTextChannel> GetAllGuildChannels(SocketGuild guild)
        {
            return _client.GetGuild(guild.Id).TextChannels;
        }

        static public IReadOnlyCollection<SocketTextChannel> GetAllGuildChannels(string guildName)
        {
            foreach(SocketGuild guild in _client.Guilds)
            {
                if (guild.ToString() == guildName) return GetAllGuildChannels(guild);
            }

            return null;
        }

        static public SocketTextChannel GetTextChannel(SocketGuild guild, string textChannel)
        {
            foreach(SocketTextChannel channel in guild.TextChannels)
            {
                if (channel.ToString() == textChannel) return channel;
            }

            return null;
        }

        static public SocketGuild GetGuild(string guildname)
        {
            foreach(SocketGuild guild in _client.Guilds)
            {
                if (guild.ToString() == guildname) return guild;
            }

            return null;
        }

        static public IReadOnlyCollection<SocketGuildUser> GetAllGuildUsers(SocketGuild guild)
        {
            return _client.GetGuild(guild.Id).Users;
        }

        static public IReadOnlyCollection<SocketGuildUser> GetAllGuildUsers(string guildName)
        {
            foreach(SocketGuild guild in _client.Guilds)
            {
                if (guild.ToString() == guildName) return GetAllGuildUsers(guild);
            }

            return null;
        }

        static async public void SendMessageToChannel(string channelName, string guildName, string message)
        {
            foreach(SocketGuild guild in _client.Guilds)
            {
                if (guild.ToString() == guildName)
                {
                    foreach(SocketTextChannel channel in guild.TextChannels)
                    {
                        if (channel.ToString() == channelName)
                        {
                            try
                            {
                                await channel.SendMessageAsync(message);
                                return;
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }
                    }
                }
            }
        }

        static async public void SendMessageToChannel(SocketTextChannel channel, string message)
        {
            try
            {
                await channel.SendMessageAsync(message);
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static async public void UploadFileToChannel(string channelName, string guildName, string file)
        {
            foreach(SocketGuild guild in _client.Guilds)
            {
                if (guild.ToString() == guildName)
                {
                    foreach(SocketTextChannel channel in guild.TextChannels)
                    {
                        if (channel.ToString() == channelName)
                        {
                            try
                            {
                                await channel.SendFileAsync(file, "");
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }
                    }
                }
            }
        }

        static async public void UploadFileToChannel(SocketTextChannel channel, string file)
        {
            try
            {
                await channel.SendFileAsync(file, "");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static public bool IsFirebaseEnabled() { return fireBaseEnabled; }

        #endregion

        #region Events
        static private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        public static Task LogHandlerAsync(LogMessage msg)
        {
            if (msg.Exception is CommandException cmdEx)
            {
                Console.WriteLine($"{cmdEx.GetBaseException().GetType()} was thrown while executing {cmdEx.Command.Aliases.First()} in {cmdEx.Context.Channel} by {cmdEx.Context.User}.");
            }
            return Task.CompletedTask;
        }

        static private Task OnReady()
        {
            Log(new LogMessage(LogSeverity.Info, "", "Bot is ready."));
            return Task.CompletedTask;
        }

        static private Task OnLoggin()
        {
            Log(new LogMessage(LogSeverity.Info, "", "Bot logged in."));
            return Task.CompletedTask;
        }

        static private Task OnConnected()
        {
            Log(new LogMessage(LogSeverity.Info, "", "Bot has connected to gateway."));
            return Task.CompletedTask;
        }

        static private Task OnDisconnected(Exception exception)
        {
            Log(new LogMessage(LogSeverity.Critical, "", "Bot has deconnected to gateway."));
            return Task.CompletedTask;
        }
        #endregion
    }
}

    
