using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using SynovianEmpireDiscordBot.BotData;

namespace SynovianEmpireDiscordBot.Modules
{
    class PermissionsModule : ModuleBase<SocketCommandContext>
    {
        [Command("AddUserToWhitelist")]
        [Summary("Developer thing : Adds user to whitelist")]
        [Alias("atwl")]
        public async Task AddUserToWhitelist(string username)
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

            BotInfo.Permissions.AddUserToWhitelist(username);
            await Log(Context.User.ToString() + " has added the user " + username + " to the whitelist.");
        }

        [Command("AddUserToBlacklist")]
        [Summary("Developer thing : Adds user to blacklist")]
        [Alias("atbl")]
        public async Task AddUserToBlacklist(string username)
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

            BotInfo.Permissions.AddUserToBlacklist(username);
            await Log(Context.User.ToString() + " has added the user " + username + " to the blacklist.");
        }

        [Command("ExportWhitelist")]
        [Summary("Developer thing : Exports the whitelist")]
        [Alias("ew")]
        public async Task ExportWhitelist()
        {
            InfoExportClass.ExportWhiteList();
            await ReplyAsync("Exported whitelist!");
            await Log(Context.User.ToString() + " export the whitelist.");
        }

        [Command("ExportBlacklist")]
        [Summary("Developer thing : Exports the whitelist")]
        [Alias("eb")]
        public async Task ExportBlacklist()
        {
            InfoExportClass.ExportBlacklist();
            await ReplyAsync("Exported blacklist!");
            await Log(Context.User.ToString() + " export the blacklist.");
        }


        // ------- end of commands
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
