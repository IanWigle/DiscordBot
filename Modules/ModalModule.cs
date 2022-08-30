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
    public class ModalModule : ModuleBase<SocketCommandContext>
    {


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