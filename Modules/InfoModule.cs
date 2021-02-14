using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.IO;

using SynovianEmpireDiscordBot.BotData;

namespace SynovianEmpireDiscordBot.Modules
{
    // Keep in mind your module **must** be public and inherit ModuleBase.
    // If it isn't, it will not be discovered by AddModulesAsync!
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        // ~say hello world -> hello world
        [Command("say")]
        [Summary("Echoes a message.")]
        public async Task SayAsync([Remainder][Summary("The text to echo")] string echo)
        {
            IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(1).FlattenAsync();
            await ((ITextChannel)Context.Channel).DeleteMessagesAsync(messages).ContinueWith(x => ReplyAsync(echo));
        }

        // ReplyAsync is a method on ModuleBase 
        // ~sample square 20 -> 400
        [Command("square")]
        [Summary("Squares a number.")]
        public async Task SquareAsync(
            [Summary("The number to square.")]
        int num)
        {
            // We can also access the channel from the Command Context.
            await Context.Channel.SendMessageAsync($"{num}^2 = {Math.Pow(num, 2)}");
        }

        [Command("DumpLogs")]
        [Summary("Developer thing : dumps backend logs to txt file.")]
        [Alias("dl")]
        public async Task DumpLogs()
        {
            if (BotInfo.UsewhiteList && !BotInfo.Permissions.IsUserInWhiteList(Context.User.ToString()))
            {
                if (BotInfo.UseblackListCaughtUsers && !BotInfo.Permissions.IsUserInBlackList(Context.User.ToString()))
                {
                    await Log("The user " + Context.User.ToString() + " attempted to use the command AnnoyDeath", LogSeverity.Critical);
                    BotInfo.Permissions.AddUserToBlacklist(Context.User.ToString());
                    return;
                }
            }

            await Log(Context.User.ToString() + " is dumping logs", LogSeverity.Debug);
            await ReplyAsync("Dumped Logs");
            await Log("Log has been dumped to " + Directory.GetCurrentDirectory() + "Logs\\LogDump.txt");
            InfoExportClass.DumpLogOutput();
        }

        [Command("AnnoyDeath")]
        [Summary("Annoy the ever living fuck out of Death")]
        [Alias("ad")]
        public async Task AnnoyDeath(string username, int numTimes, params string[] message)
        {
            if (BotInfo.UsewhiteList && !BotInfo.Permissions.IsUserInWhiteList(Context.User.ToString()))
            {
                if (BotInfo.UseblackListCaughtUsers && !BotInfo.Permissions.IsUserInBlackList(Context.User.ToString()))
                {
                    await Log("The user " + Context.User.ToString() + " attempted to use the command AnnoyDeath", LogSeverity.Critical);
                    await ReplyAsync("You need to be on the whitelist!");
                    BotInfo.Permissions.AddUserToBlacklist(Context.User.ToString());
                    return;
                }
            }

            if (numTimes > 50) numTimes = 50;

            await Log($"Sending {numTimes} messages to " + username);

            var users = Context.Guild.Users;
            foreach (var user in users)
            {
                if (user.ToString() != username)
                    continue;

                for (int i = 0; i < numTimes; i++)
                {
                    await user.SendMessageAsync(BotHelpers.CondenseStringArray(message));
                    await Task.Delay(500);
                }
            }

            await ReplyAsync("☢️ The nukes have been launched towards " + username + ", god help us all. ☢️");
            return;
        }

        [Command("Trello")]
        [Summary("Provides link to trello board")]
        [Alias("tr")]
        public async Task Trello()
        {
            await ReplyAsync(Program.TrelloLink);
        }

        // ---------------------------------------------- End of commands

        private Task Log(string msg, LogSeverity severity = LogSeverity.Info)
        {
            string source = "";

            switch (severity)
            {
                case LogSeverity.Critical:
                    source = "Critical";
                    break;
                case LogSeverity.Error:
                    source = "Error";
                    break;
                case LogSeverity.Warning:
                    source = "Warning";
                    break;
                case LogSeverity.Info:
                    source = "Info";
                    break;
                case LogSeverity.Verbose:
                    source = "Verbose";
                    break;
                case LogSeverity.Debug:
                    source = "Debug";
                    break;
                default:
                    break;
            }
            LogMessage message = new LogMessage(severity, source, msg);
            Console.WriteLine(message.ToString());
            LogCatalog.AddLog(message.ToString());
            return Task.CompletedTask;
        }
    }
}