using System;
using System.Collections;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using SynovianEmpireDiscordBot.BotData;

namespace SynovianEmpireDiscordBot
{
    class CombatModule : ModuleBase<SocketCommandContext>
    {

        //command = 2d3 [command(x "d" y)
        //roll 5d3
        // roll 6d3
        // !FightEnemy2 2d8 3d4 help
        [Command("FightEnemy2")]
        [Summary("Start a fight against a enemy.")]
        [Alias("fe2","rollm")]
        public async Task FightEnemy2(params string[] dice)
        {
            ArrayList dicelist = new ArrayList();
            EmbedBuilder embedBuilder = new EmbedBuilder();

            foreach(string die in dice)
            {
                if (die == "help" || die == "h")
                {
                    embedBuilder.AddField("Help 🎲", "To use this command, type as a word your number of dice, a 'd' and your dice rank. \n\n Example : 3d7 \n\n The 3 represents the number of dice to roll, and the 7 represents the max rank number.");
                    await ReplyAsync("", false, embedBuilder.Build());
                    return;
                }
                else
                {
                    dicelist.Add(die);
                }
            }

            var rand = new Random();
            for (int i = 0; i < dicelist.Count; i++)
            {
                string rollCommand = (string)dicelist[i];

                string[] rollNum = rollCommand.Split('d');
                int maxDice = int.Parse(rollNum[0]);
                int rank = int.Parse(rollNum[1]);

                
                string attackerLog = "";
                int highestAttack = 0;
                int lowestAttack = 0; 

                for (int a = 0; a < maxDice; a++)
                {
                    int newNumber = rand.Next(1, rank + 1);
                    if (lowestAttack == 0) lowestAttack = newNumber;

                    attackerLog += ((a + 1) != maxDice) ? $"{newNumber}, " : $"{newNumber}";

                    if (newNumber > highestAttack) highestAttack = newNumber;
                    else if (newNumber < lowestAttack) lowestAttack = newNumber;
                }

                attackerLog += $"\n\n The lowest number was {lowestAttack}. \n The highest number was {highestAttack}.";

                embedBuilder.AddField($"Dice {i + 1}  🎲  ", attackerLog);
            }

            await ReplyAsync("Your rolls 🎲 : \n", false,embedBuilder.Build());            
        }

        // end of commands

        Task Log(string msg, LogSeverity severity = LogSeverity.Info)
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
