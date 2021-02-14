namespace SynovianEmpireDiscordBot.BotData
{
    static class BotHelpers
    {
        public static string CondenseStringArray(string[] stringArray)
        {
            string _string = "";
            foreach (string str in stringArray)
            {
                _string += str;
            }
            return _string;
        }
    }
}
