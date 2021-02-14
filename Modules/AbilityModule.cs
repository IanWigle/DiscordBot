using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Collections;

using SynovianEmpireDiscordBot.BotData;
using SynovianEmpireDiscordBot.Libraries;
using SynovianEmpireDiscordBot.CharacterMakerClasses;

namespace SynovianEmpireDiscordBot.Modules
{
    class AbilityModule : ModuleBase<SocketCommandContext>
    {
        [Command("DescribeAbility")]
        [Summary("Describes a provided ability by name")]
        [Alias("da")]
        public async Task DescribeAbility(params string[] name)
        {
            string _name = BotHelpers.CondenseStringArray(name);

            if (!Program.abilityLibrary.LibraryHasAbility(_name))
            {
                await Log(Context.User.ToString() + " asked for the invalid ability name " + _name, LogSeverity.Error);
                await ReplyAsync(_name + " is not a valid ability!");
                return;
            }
            else
            {
                Ability ab = Program.abilityLibrary.GetAbility(_name);
                EmbedBuilder embedBuilder = new EmbedBuilder();
                embedBuilder.AddField("Name", ab.Name);
                embedBuilder.AddField("Rank", ab.GetRankAsString());
                embedBuilder.AddField("Description", ab.Description);

                await ReplyAsync("Here is what I found . . . \n", false, embedBuilder.Build()).ContinueWith(x => 
                Log("Descibed the ability " + _name + " in the channel " + Context.Channel.ToString()));
            }
        }

        [Command("ExportAbilityLibrary")]
        [Summary("Exports the AbilityLibrary")]
        [Alias("eal")]
        public async Task ExportAbilityLibrary()
        {
            if(BotInfo.UsewhiteList && !BotInfo.Permissions.IsUserInWhiteList(Context.User.ToString()))
            {
                if (BotInfo.UseblackListCaughtUsers && !BotInfo.Permissions.IsUserInBlackList(Context.User.ToString()))
                {
                    await Log("The user " + Context.User.ToString() + " attempted to export the ability library!", LogSeverity.Critical);
                    BotInfo.Permissions.AddUserToBlacklist(Context.User.ToString());
                    return;
                }
            }

            InfoExportClass.ExportAbilityLibrary();
            await Log("The user " + Context.User.ToString() + " has exported the AbilityLibrary");
            await ReplyAsync("The AbilityLibrary has been exported.");
        }

        [Command("ListAbilities")]
        [Summary("Lists abilities by rank and alignment")]
        [Alias("la")]       
        public async Task ListAbilities(params string[] properties)
        {
            string alignment = "";
            string rank = "";
            bool isAskingForHelp = false;

            ArrayList rankList = new ArrayList();
            rankList.Add("Acolyte");
            rankList.Add("Apprentice");
            rankList.Add("Knight");
            rankList.Add("Lord");
            rankList.Add("Archon");
            rankList.Add("Elder");

            ArrayList alignmentList = new ArrayList();
            alignmentList.Add("Darkside");
            alignmentList.Add("Lightside");
            alignmentList.Add("Neutral");
            alignmentList.Add("Non-Force");
            alignmentList.Add("testAlign");

            foreach(string str in properties)
            {
                if (rankList.Contains(str) && rank == "") rank = str;
                else if (alignmentList.Contains(str) && alignment == "") alignment = str;
                else if (str == "help" || str == "Help" || str == "h") isAskingForHelp = true;
            }

            if (isAskingForHelp)
            {
                EmbedBuilder embedBuilder = new EmbedBuilder();
                embedBuilder.AddField("Help", "To use this command, you can provide a rank and alignment. Or you can only supply one of the two. \n\n In either case, the bot will then provide a list of abilities based on the filters provided.");
                await ReplyAsync("", false, embedBuilder.Build());
                return;
            }

            ArrayList filteredAbilities = Program.abilityLibrary.GetAbilitiesByFilters(rank, alignment);

            EmbedBuilder filtedembedBuilder = new EmbedBuilder();
            foreach(Ability ab in filteredAbilities)
            {
                filtedembedBuilder.AddField(ab.Name, "Rank : " + ab.rank + "\n\n" + ab.Description);
            }

            string resultDesc = $"Found {filteredAbilities.Count} abilities";
            resultDesc += " using " + ((rank != "") ? rank : "");
            resultDesc += (rank != "" && alignment != "") ? " and " : "";
            resultDesc += (alignment != "") ? alignment : "";
            resultDesc += ".";

            await ReplyAsync(resultDesc, false, filtedembedBuilder.Build()).ContinueWith(x => 
                  Log($"Displaying {filteredAbilities.Count} abilities to the channel" + Context.Channel.ToString()));
        }

        // ------------ End of commands


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
