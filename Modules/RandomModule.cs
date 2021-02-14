using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using SynovianEmpireDiscordBot.BotData;

namespace SynovianEmpireDiscordBot.Modules
{
    class RandomModule : ModuleBase<SocketCommandContext>
    {
        [Command("RandomMeme")]
        [Summary("A random meme from Abe's brain.")]
        [Alias("rm")]
        public async Task RandomMeme()
        {            
            RandomResult result = (RandomResult)rand.Next(0, 2);

            switch (result)
            {
                case RandomResult.YoutubeLink:
                    {
                        string meme = InfoImportClass.GetRandomYoutubeLink();

                        if (meme != "") await ReplyAsync(meme);
                        else
                        {
                            await ReplyAsync("Failed to get a meme, message Abe");
                            await Log("Failed to post youtube link to the channel " + Context.Channel.ToString() + " in the guild " + Context.Guild.ToString(), LogSeverity.Error);
                            break;
                        }
                        break;
                    }
                case RandomResult.Image:
                    {
                        string meme = InfoImportClass.GetRandomImageFromFile();
                        if (meme != "") await Context.Channel.SendFileAsync(meme, "");
                        else
                        {
                            await ReplyAsync("Tried to upload image but failed to get one.");
                            await Log("Failed to upload a image to the channel " + Context.Channel.ToString() + " in the guild " + Context.Guild.ToString(), LogSeverity.Error);
                            break;
                        }
                        break;
                    }
                case RandomResult.Video:
                    {
                        string meme = InfoImportClass.GetRandomVideoFromFile();
                        if (meme != "") await Context.Channel.SendFileAsync(meme, "");
                        else
                        {
                            await ReplyAsync("Tried to upload a video but failed to get one.");
                            await Log("Failed to upload a video to the channel " + Context.Channel.ToString() + " in the guild " + Context.Guild.ToString(), LogSeverity.Error);
                            break;
                        }
                        break;
                    }
                default:
                    break;
            }
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

        enum RandomResult
        {
            YoutubeLink = 0,
            Image = 1,
            Video = 2
        };

        Random rand = new Random();
    }
}
