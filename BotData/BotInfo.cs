using System.Collections;

namespace SynovianEmpireDiscordBot.BotData
{
    public class PermissionLists
    {
        private ArrayList blacklistedUsers = new ArrayList();
        private ArrayList whitelistedUsers = new ArrayList();

        public void AddUserToBlacklist(string username)
        {
            if (IsUserInBlackList(username))
                return;

            blacklistedUsers.Add(username);
        }

        public void AddUserToWhitelist(string username)
        {
            if (IsUserInWhiteList(username))
                return;

            whitelistedUsers.Add(username);
        }

        public bool IsUserInBlackList(string username)
        {
            return blacklistedUsers.Contains(username);
        }

        public bool IsUserInWhiteList(string username)
        {
            return whitelistedUsers.Contains(username);
        }

        public ArrayList GetBlackList()
        {
            return blacklistedUsers;
        }

        public ArrayList GetWhiteList()
        {
            return whitelistedUsers;
        }
    }


    static class BotInfo
    {
        // Permission vars
        static public bool UseblackListCaughtUsers = true;
        static public bool UsewhiteList = true;
        static public bool UseSpecificTextChannel = true;
        static public string SpecificTextChannel = "";
        static public PermissionLists Permissions = new PermissionLists();
        
        // Log vars
        static public bool AutoLog = true;
        
    }
}
