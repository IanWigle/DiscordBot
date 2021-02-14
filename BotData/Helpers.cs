using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynovianEmpireDiscordBot.BotData.Helpers
{
    public class StringHelpers
    {
        public static string GetFileNameWithoutExtension(string filename)
        {
            return filename.Split('.')[0];
        }
        public static string GetFileNameExtension(string filename)
        {
            return filename.Split('.')[1];
        }
    }
}
