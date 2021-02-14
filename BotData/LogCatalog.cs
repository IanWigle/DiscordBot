using System.Collections;

namespace SynovianEmpireDiscordBot.BotData
{
    static public class LogCatalog
    {
        private static ArrayList LogHistory = new ArrayList();

        static public void AddLog(string log)
        {
            LogHistory.Add(log);
        }

        static public ArrayList GetLogHistory() { return LogHistory; }
    }
}
