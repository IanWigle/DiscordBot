using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;


using SynovianEmpireDiscordBot.BotData;

namespace SynovianEmpireDiscordBot.Modules
{
    class ServerModule : ModuleBase<SocketCommandContext>
    {
        [Command("userinfo")]
        [Summary
        ("Returns info about the current user, or the user parameter, if one passed.")]
        [Alias("user", "whois")]
        public async Task UserInfoAsync(
            [Summary("The (optional) user to get info from")]
        SocketUser user = null)
        {
            var userInfo = user ?? Context.Client.CurrentUser;
            await ReplyAsync($"{userInfo.Username}#{userInfo.Discriminator}");
        }

        [Command("CreateRank")]
        [Summary
            ("Attempts to create a rank.")]
        [Alias("cr")]
        public async Task CreateRankAsync(
            params string[] name)
        {
            // Check to make sure user is not in blacklist, and if we are using the blacklist
            if (BotInfo.Permissions.IsUserInBlackList(Context.User.ToString()) && BotInfo.UseblackListCaughtUsers)
            {
                await Log("Blacklist user attempted to use a powerful command", LogSeverity.Critical);
                return;
            }

            // Check to make sure a bot is not calling this command
            if (Context.User.IsBot)
            {
                await Log("A BOT ATTEMPTED TO MESSAGE PEOPLE",LogSeverity.Critical);
                if (BotInfo.UseblackListCaughtUsers)
                {
                    BotInfo.Permissions.AddUserToBlacklist(Context.User.ToString());
                }

                return;
            }

            if (BotInfo.UsewhiteList && !BotInfo.Permissions.IsUserInWhiteList(Context.User.ToString()))
            {
                if (BotInfo.UseblackListCaughtUsers && !BotInfo.Permissions.IsUserInBlackList(Context.User.ToString()))
                {
                    await Log("The user " + Context.User.ToString() + " attempted to use the command AnnoyDeath", LogSeverity.Critical);
                    BotInfo.Permissions.AddUserToBlacklist(Context.User.ToString());
                    return;
                }
            }

            string _name = BotHelpers.CondenseStringArray(name);

            foreach (SocketRole role in Context.Guild.Roles)
            {
                if (role.Name == _name)
                {
                    await Context.Channel.SendMessageAsync("Rank already exists!");
                    await Log(Context.User.ToString() + "tried making the rank " + _name + " but it already exists!", LogSeverity.Error);
                    return;
                }
            }

            await Context.Guild.CreateRoleAsync(_name, null, null, false, false, null).ContinueWith(x => Context.Channel.SendMessageAsync("Rank made!"));
            await Log(Context.User.ToString() + " created the rank " + _name, LogSeverity.Info);
        }

        [Command("CreateTextChannel")]
        [Summary
            ("Attempts to create a text channel.")]
        [Alias("ctc")]
        public async Task CreateTextChannel(
            string channel_name, string category_name = "")
        {
            // Check to make sure user is not in blacklist, and if we are using the blacklist
            if (BotInfo.Permissions.IsUserInBlackList(Context.User.ToString()) && BotInfo.UseblackListCaughtUsers)
            {
                await Log("Blacklist user attempted to use a powerful command", LogSeverity.Critical);
                return;
            }

            // Check to make sure a bot is not calling this command
            if (Context.User.IsBot)
            {
                await Log("A BOT ATTEMPTED TO MESSAGE PEOPLE", LogSeverity.Critical);
                if (BotInfo.UseblackListCaughtUsers)
                {
                    BotInfo.Permissions.AddUserToBlacklist(Context.User.ToString());
                }

                return;
            }

            if (BotInfo.UsewhiteList && !BotInfo.Permissions.IsUserInWhiteList(Context.User.ToString()))
            {
                if (BotInfo.UseblackListCaughtUsers && !BotInfo.Permissions.IsUserInBlackList(Context.User.ToString()))
                {
                    await Log("The user " + Context.User.ToString() + " attempted to use the command AnnoyDeath", LogSeverity.Critical);
                    BotInfo.Permissions.AddUserToBlacklist(Context.User.ToString());
                    return;
                }
            }

            foreach (SocketTextChannel channel in Context.Guild.TextChannels)
            {
                if (channel.Name == channel_name)
                {
                    await Context.Channel.SendMessageAsync("The channel " + channel_name + " already exists!");
                    await Log(Context.User.ToString() + " attempted to make the channel" + channel_name + " but it already exists!", LogSeverity.Error);
                    return;
                }
            }

            if (category_name != "" && Context.Guild.CategoryChannels.FirstOrDefault(x => x.Name == "Text Channels") == null)
            {
               if (Context.Guild.CategoryChannels.FirstOrDefault(x => x.Name == category_name) == null)
                {
                    await Context.Guild.CreateCategoryChannelAsync(category_name);
                    await Log(Context.User.ToString() + " has created the category " + category_name);

                    //Find the ID for the desired category 
                    var categoryId = Context.Guild.CategoryChannels.FirstOrDefault(category => category.Name.Equals(category_name))?.Id;

                    //Set channel category during channel creation
                    await Context.Guild.CreateTextChannelAsync(channel_name, prop => prop.CategoryId = categoryId);
                    await Log(Context.User.ToString() + " has created the text channel " + channel_name + " under the category " + category_name);
                }
            }
            else if (category_name == "" && Context.Guild.CategoryChannels.FirstOrDefault(x => x.Name == "Text Channels") != null)
            {
                //Find the ID for the desired category 
                var categoryId = Context.Guild.CategoryChannels.FirstOrDefault(category => category.Name.Equals("Text Channels"))?.Id;

                //Set channel category during channel creation
                await Context.Guild.CreateTextChannelAsync(channel_name, prop => prop.CategoryId = categoryId);
                await Log(Context.User.ToString() + " has created the text channel" + channel_name);
            }
            else
            {
                await Context.Guild.CreateTextChannelAsync(channel_name, null, null);
                await Log(Context.User.ToString() + " has created the text channel " + channel_name);
            }
            await Context.Channel.SendMessageAsync("Text Channel made!");
        }

        [Command("CreateVoiceChannel")]
        [Summary
            ("Attempts to create a voice channel.")]
        [Alias("cvc")]
        public async Task CreateVoiceChannel(
            string name)
        {
            // Check to make sure user is not in blacklist, and if we are using the blacklist
            if (BotInfo.Permissions.IsUserInBlackList(Context.User.ToString()) && BotInfo.UseblackListCaughtUsers)
            {
                await Log("Blacklist user attempted to use a powerful command",LogSeverity.Critical);
                return;
            }

            // Check to make sure a bot is not calling this command
            if (Context.User.IsBot)
            {
                await Log("A BOT ATTEMPTED TO CREATE A CHANNEL", LogSeverity.Critical);
                if (BotInfo.UseblackListCaughtUsers)
                {
                    BotInfo.Permissions.AddUserToBlacklist(Context.User.ToString());
                }

                return;
            }

            if (BotInfo.UsewhiteList && !BotInfo.Permissions.IsUserInWhiteList(Context.User.ToString()))
            {
                if (BotInfo.UseblackListCaughtUsers && !BotInfo.Permissions.IsUserInBlackList(Context.User.ToString()))
                {
                    await Log("The user " + Context.User.ToString() + " attempted to use the command AnnoyDeath", LogSeverity.Critical);
                    BotInfo.Permissions.AddUserToBlacklist(Context.User.ToString());
                    return;
                }
            }

            foreach (SocketVoiceChannel channel in Context.Guild.VoiceChannels)
            {
                if (channel.Name == name)
                {
                    await Context.Channel.SendMessageAsync("Channel already exists!");
                    await Log(Context.User.ToString() + " attempted to make a voice channel called " + name + " but it already exists!", LogSeverity.Error);
                    return;
                }
            }

            await Context.Guild.CreateVoiceChannelAsync(name, null, null);
            await Context.Channel.SendMessageAsync("Voice Channel made!");
            await Log(Context.User.ToString() + " made the voice channel " + name);
        }

        [Command("SetRank")]
        [Summary
            ("Attempts to set a given rank")]
        [Alias("sr")]
        public async Task SetRank(params string[] rank)
        {
            // Check to make sure user is not in blacklist, and if we are using the blacklist
            if (BotInfo.Permissions.IsUserInBlackList(Context.User.ToString()) && BotInfo.UseblackListCaughtUsers)
            {
                await Log("Blacklist user attempted to use a powerful command");
                return;
            }

            // Check to make sure a bot is not calling this command
            if (Context.User.IsBot)
            {
                await Log("A BOT ATTEMPTED TO CREATE A RANK", LogSeverity.Critical);
                if (BotInfo.UseblackListCaughtUsers)
                {
                    BotInfo.Permissions.AddUserToBlacklist(Context.User.ToString());
                }

                return;
            }

            if (BotInfo.UsewhiteList && !BotInfo.Permissions.IsUserInWhiteList(Context.User.ToString()))
            {
                if (BotInfo.UseblackListCaughtUsers && !BotInfo.Permissions.IsUserInBlackList(Context.User.ToString()))
                {
                    await Log("The user " + Context.User.ToString() + " attempted to use the command AnnoyDeath", LogSeverity.Critical);
                    BotInfo.Permissions.AddUserToBlacklist(Context.User.ToString());
                    return;
                }
            }

            string _rank = BotHelpers.CondenseStringArray(rank);

            bool doubleCheck = false;
            foreach (SocketRole role in Context.Guild.Roles)
            {
                if (role.Name == _rank)
                {
                    doubleCheck = true;
                    break;
                }
            }
            if (doubleCheck == true)
            {
                if (!Context.User.IsBot)
                {
                    var user = Context.User;
                    var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == _rank);
                    await (user as IGuildUser).AddRoleAsync(role);
                    await (ReplyAsync("Rank added!"));
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync("Rank doesn't exist!");
                await Log(Context.User.ToString() + " attempted to remove the rank " + _rank + " but it doesn't exist", LogSeverity.Error);
            }
        }

        [Command("RemoveRank")]
        [Summary
            ("Attempts to remove a given rank")]
        [Alias("rr")]
        public async Task RemoveRank(params string[] rank)
        {
            // Check to make sure user is not in blacklist, and if we are using the blacklist
            if (BotInfo.Permissions.IsUserInBlackList(Context.User.ToString()) && BotInfo.UseblackListCaughtUsers)
            {
                await Log("Blacklist user attempted to use a powerful command", LogSeverity.Critical);
                return;
            }

            // Check to make sure a bot is not calling this command
            if (Context.User.IsBot)
            {
                await Log("A BOT ATTEMPTED TO REMOVE A RANK", LogSeverity.Critical);
                if (BotInfo.UseblackListCaughtUsers)
                {
                    BotInfo.Permissions.AddUserToBlacklist(Context.User.ToString());
                }

                return;
            }

            if (BotInfo.UsewhiteList && !BotInfo.Permissions.IsUserInWhiteList(Context.User.ToString()))
            {
                if (BotInfo.UseblackListCaughtUsers && !BotInfo.Permissions.IsUserInBlackList(Context.User.ToString()))
                {
                    await Log("The user " + Context.User.ToString() + " attempted to use the command AnnoyDeath", LogSeverity.Critical);
                    BotInfo.Permissions.AddUserToBlacklist(Context.User.ToString());
                    return;
                }
            }

            SocketUser user = Context.User;

            string _rank = BotHelpers.CondenseStringArray(rank);

            bool doubleCheck = false;
            foreach (SocketRole role in Context.Guild.Roles)
            {
                if (role.Name == _rank)
                {
                    doubleCheck = true;
                    break;
                }
            }
            if (doubleCheck == true)
            {
                var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == _rank);
                await (user as IGuildUser).RemoveRoleAsync(role);
                await (ReplyAsync("Rank removed!"));
            }
            else
            {
                await (ReplyAsync("Couldn't complete request!"));
            }
        }

        [Command("ClearChannelHistory")]
        [Summary
            ("Clears all channel history")]
        [Alias("cch")]
        public async Task CleanChannelHistory(int amount)
        {
            // Check to make sure user is not in blacklist, and if we are using the blacklist
            if (BotInfo.Permissions.IsUserInBlackList(Context.User.ToString()) && BotInfo.UseblackListCaughtUsers)
            {
                await Log("Blacklist user attempted to use a powerful command", LogSeverity.Critical);
                return;
            }

            // Check to make sure a bot is not calling this command
            if (Context.User.IsBot)
            {
                await Log("A BOT ATTEMPTED TO WIPE CHANNEL HISTORY",LogSeverity.Critical);
                if (BotInfo.UseblackListCaughtUsers)
                {
                    BotInfo.Permissions.AddUserToBlacklist(Context.User.ToString());
                }

                return;
            }

            if (BotInfo.UsewhiteList && !BotInfo.Permissions.IsUserInWhiteList(Context.User.ToString()))
            {
                if (BotInfo.UseblackListCaughtUsers && !BotInfo.Permissions.IsUserInBlackList(Context.User.ToString()))
                {
                    await Log("The user " + Context.User.ToString() + " attempted to use the command AnnoyDeath", LogSeverity.Critical);
                    BotInfo.Permissions.AddUserToBlacklist(Context.User.ToString());
                    return;
                }
            }

            IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(amount + 1).FlattenAsync();
            await ((ITextChannel)Context.Channel).DeleteMessagesAsync(messages);
            const int delay = 3000;
            IUserMessage m = await ReplyAsync($"I have deleted {amount} messages for ya.");
            await Log(Context.User.ToString() + $" has deleted {amount} from the channel " + Context.Channel.ToString());
            await Task.Delay(delay);
            await m.DeleteAsync();
        }

        [Command("MessagePlayersOfRank")]
        [Summary
            ("Message all players of rank")]
        [Alias("mpr")]
        public async Task MessagePlayersOfRank(string _rank, params string[] message)
        {
            // Check to make sure user is not in blacklist, and if we are using the blacklist
            if (BotInfo.Permissions.IsUserInBlackList(Context.User.ToString()) && BotInfo.UseblackListCaughtUsers)
            {
                await Log("Blacklist user attempted to use a powerful command");
                return;
            }

            // Check to make sure a bot is not calling this command
            if (Context.User.IsBot)
            {
                await Log("A BOT ATTEMPTED TO MESSAGE PEOPLE");
                if (BotInfo.UseblackListCaughtUsers)
                {
                    BotInfo.Permissions.AddUserToBlacklist(Context.User.ToString());
                }

                return;
            }

            if (BotInfo.UsewhiteList && !BotInfo.Permissions.IsUserInWhiteList(Context.User.ToString()))
            {
                if (BotInfo.UseblackListCaughtUsers && !BotInfo.Permissions.IsUserInBlackList(Context.User.ToString()))
                {
                    await Log("The user " + Context.User.ToString() + " attempted to use the command AnnoyDeath", LogSeverity.Critical);
                    BotInfo.Permissions.AddUserToBlacklist(Context.User.ToString());
                    return;
                }
            }

            var users = Context.Guild.Users;
            int numValidUsers = 0;
            foreach (var user in users)
            {
                bool hasRole = false;
                foreach (var role in user.Roles)
                {
                    if (role.ToString() == _rank)
                    {
                        hasRole = true;
                        break;
                    }
                }

                if (hasRole)
                {
                    string _message = BotHelpers.CondenseStringArray(message);

                    if (_message == "test" || _message == "") _message = "Hello there! This is either a test, or the guy who made me do this didn't give me a message to give you.";

                    await user.SendMessageAsync(_message);
                    await Log("Sent message to " + user.ToString());
                    numValidUsers++;
                }
            }
            await ReplyAsync($"Sent {numValidUsers} messages");
        }

        //[Command("SetBotNickName")]
        //[Summary
        //    ("Set the nickname of this bot in the guild")]
        //[Alias("sbn")]
        //public async Task SetBotNickName(params string[] nickname)
        //{
        //    // Currently doing research so this function does nothing
        //    
        //
        //    string _nickname = BotHelpers.CondenseStringArray(nickname);
        //    
        //    // If the nickname is invalid
        //    if (_nickname == "")
        //    {
        //        await Log("The name provided for the bot was not valid");
        //    }
        //    else
        //    {
        //    
        //    }
        //}

        [Command("ListCommands")]
        [Summary("Shows a list of commands that are available from the bot")]
        [Alias("lc","listcommands")]
        public async Task ListCommands(int page = 0)
        {
            List<CommandInfo> commands = Program.GetBotCommands();
            EmbedBuilder embedBuilder = new EmbedBuilder();

            // This is a limit to the embedBuilder, you can only have 25 fields max.
            const int maxPerPage = 25;
            int numberPages = commands.Count % maxPerPage;
            if ((page - 1) >= 1) page--;
            else if ((page - 1) < 0) page = 0;
            if(page > numberPages)
            {
                await Log("Provided page number is more than the number of pages. Defaulting to 0", LogSeverity.Error);
                await ReplyAsync("Provided page number is more than the number of pages. Defaulting to 0");
            }


            //foreach (CommandInfo command in commands)
            //{
            //    try
            //    {
            //        if (command.Summary != "") embedBuilder.AddField(command.Name, command.Summary);
            //        else embedBuilder.AddField(command.Name, command.Summary);
            //    }
            //    catch(Exception e)
            //    {
            //        await Log("Failed to get command info for " + command.Name, LogSeverity.Error);
            //        await Log(e.Message,LogSeverity.Error);
            //    }            
            //}

            for (int i = 0; i < (page + 1) * 25; i++)
            {
                if ((i + (page * 25)) >= commands.Count) break;
                CommandInfo commandInfo = commands[i + (page * 25)];
                try
                {
                    if (commandInfo.Summary != "") embedBuilder.AddField(commandInfo.Name, commandInfo.Summary);
                    else embedBuilder.AddField(commandInfo.Name, commandInfo.Summary);
                }
                catch (Exception e)
                {
                    await Log("Failed to get command info for " + commandInfo.Name, LogSeverity.Error);
                    await Log(e.Message, LogSeverity.Error);
                }
            }


            await ReplyAsync($"Here's a list of commands and their descriptions from page {page} :ok_hand: : ", false, embedBuilder.Build());
        }

        [Command("Help")]
        [Summary("Describes the bot.")]
        [Alias("h","help")]
        public async Task Help()
        {
            EmbedBuilder embedBuilder = new EmbedBuilder();

            embedBuilder.AddField("Summary", "I am capable of making ranks, setting ranks, and removing ranks. I can do mass messages to certain ranked users as well.\n\n However I am programmed to not take orders from other bots or those who are suspicious, and I do save a blacklist of users that I will not accept orders from.\n\n");
            embedBuilder.AddField("Blacklist", "Only certain people of rank, and the creator : Paldamar/Abhrame#5843, has the ability to do anything related to the blacklist and my options.\n\n If you find yourself on the blacklist somehow, let the maker (mentioned above) know.\n\n");
            

            await ReplyAsync("Here's some info about me: ", false, embedBuilder.Build());
        }

        [Command("BotInvite")]
        [Summary("Link to invite bot to servers")]
        [Alias("bi")]
        public async Task BotInvite()
        {
            var builder = new ComponentBuilder()
                .WithButton("Test", "custom-id");

            await ReplyAsync("https://discord.com/api/oauth2/authorize?client_id=751741124351230063&permissions=8&scope=bot%20applications.commands");
        }

        // ---------------- End of commands

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
            //if (AutoLog) InfoExportClass.DumpLogOutput(message.ToString());
            //BotInfo.LogList.Add(message.ToString());
            LogCatalog.AddLog(message.ToString());
            return Task.CompletedTask;
        }
    }
}
