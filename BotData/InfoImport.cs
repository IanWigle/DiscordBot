using System;
using System.Collections;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using SynovianEmpireDiscordBot.Libraries;
//using Synovian_Character_Maker.DataClasses.Instanced;

namespace SynovianEmpireDiscordBot.BotData
{
    /***********************************************************************************************
     * InfoImportClass -- A static class manager to handle the import of various amounts of data   *
     *                    from files within the program.                                           *
     *                                                                                             *
     *                                                                                             *
     *                                                                                             *
     * INPUT:    Nothing                                                                           *
     *                                                                                             *
     * OUTPUT:   Various                                                                           *
     *                                                                                             *
     * WARNINGS: None                                                                              *
     *                                                                                             *
     * HISTORY:                                                                                    *
     *    2020-09-25 11:03AM EST : Created                                                         *
     *=============================================================================================*/
    static public class InfoImportClass
    {
        /***********************************************************************************************
         * LoadBlackList -- A method in which it searchs for the file Blacklist.txt, then store the    *
         *                  string lines from the System.IO into the ArrayList Blacklist for the       *
         *                  bot to use                                                                 *
         *                                                                                             *
         *                                                                                             *
         *                                                                                             *
         * INPUT:    Nothing                                                                           *
         *                                                                                             *
         * OUTPUT:   Nothing                                                                           *
         *                                                                                             *
         * WARNINGS: None                                                                              *
         *                                                                                             *
         * HISTORY:                                                                                    *
         *    2020-09-25 11:12AM EST : Created                                                         *
         *=============================================================================================*/
        static public void LoadBlackList()
        {
            string curDir = Directory.GetCurrentDirectory() + "\\Lists\\";

            if (!Directory.Exists(curDir))
            {
                Directory.CreateDirectory(curDir);
                Console.WriteLine("Error! List Folder could not be found! Made directory at " + curDir);
                return;
            }

            string[] lines = System.IO.File.ReadAllLines(curDir + "Blacklist.txt");

            foreach (string str in lines)
            {
                BotInfo.Permissions.AddUserToBlacklist(str);
            }

            Console.WriteLine("Loaded Blacklist");
        }

        /***********************************************************************************************
         * LoadWhiteList -- A method in which it searchs for the file Whitelist.txt, then store the    *
         *                  string lines from the System.IO into the ArrayList Whitelist for the       *
         *                  bot to use                                                                 *
         *                                                                                             *
         *                                                                                             *
         *                                                                                             *
         * INPUT:    Nothing                                                                           *
         *                                                                                             *
         * OUTPUT:   Nothing                                                                           *
         *                                                                                             *
         * WARNINGS: None                                                                              *
         *                                                                                             *
         * HISTORY:                                                                                    *
         *    2020-09-25 11:12AM EST : Created                                                         *
         *=============================================================================================*/
        static public void LoadWhiteList()
        {
            string curDir = Directory.GetCurrentDirectory() + "\\Lists\\";

            if (!Directory.Exists(curDir))
            {
                Directory.CreateDirectory(curDir);
                Console.WriteLine("Error! List Folder could not be found! Made directory at " + curDir);
                return;
            }

            if (File.Exists(curDir + "\\Whitelist.txt"))
            {
                string[] lines = System.IO.File.ReadAllLines(curDir + "\\Whitelist.txt");

                foreach (string str in lines)
                {
                    BotInfo.Permissions.AddUserToWhitelist(str);
                }

                Console.WriteLine("Loaded Whitelist");
            }
            else
                return;
        }

        /***********************************************************************************************
         * LoadAbilityLibrary -- Will load library json from AbilityLibrary.txt and cast the json      *
         *                       format to the needed Ability class template.                          *
         *                                                                                             *
         *                                                                                             *
         *                                                                                             *
         * INPUT:    Nothing                                                                           *
         *                                                                                             *
         * OUTPUT:   Nothing                                                                           *
         *                                                                                             *
         * WARNINGS: Should any changes be made in the ability class (such as the addition of vars),   *
         *           then the validation check must be updated to read and configure any updated       *
         *           class variables. Ability class constructor will set any vars to 'invalid' by      *
         *           default.                                                                          *
         *                                                                                             *
         * HISTORY:                                                                                    *
         *    2020-09-25 11:12AM EST : Created                                                         *
         *    2020-10-01 03:38PM EST : Added input param for if we are refreshing the library.         *
         *=============================================================================================*/
        static public void LoadAbilityLibrary(bool isRefreshing = true)
        {
            //string curDir = Directory.GetCurrentDirectory() + "\\Lists\\";
            //
            //if (!Directory.Exists(curDir))
            //{
            //    Directory.CreateDirectory(curDir);
            //    Console.WriteLine("Error! List Folder could not be found! Made directory at " + curDir);
            //    return;
            //}
            //
            //string JsonString = System.IO.File.ReadAllText(curDir + "AbilityLibrary.txt");
            //
            //using (JsonDocument document = JsonDocument.Parse(JsonString))
            //{
            //    List<Ability> abilityList = new List<Ability>();
            //
            //    JsonElement root = document.RootElement;
            //    foreach (JsonElement ability in root.EnumerateArray())
            //    {
            //        string name = "";
            //        string desc = "";
            //        CharacterRank rank = CharacterRank.NoRank;
            //        CharacterAlignment align = CharacterAlignment.None;
            //        AbilityCategory cat = AbilityCategory.NoCategory;
            //        int skill = 0;
            //        List<string> prereqs = new List<string>();
            //
            //        if (ability.TryGetProperty("Name", out JsonElement nameElement))
            //        {
            //            name = nameElement.GetString();
            //        }
            //        if (ability.TryGetProperty("rank", out JsonElement rankElement))
            //        {
            //            rank = (CharacterRank)rankElement.GetInt32();
            //        }
            //        if (ability.TryGetProperty("Description", out JsonElement descElement))
            //        {
            //            desc = descElement.GetString();
            //        }
            //        if (ability.TryGetProperty("alignment", out JsonElement alignElement))
            //        {
            //            align = (CharacterAlignment)alignElement.GetInt32();
            //        }
            //        if (ability.TryGetProperty("skillCost", out JsonElement skillElement))
            //        {
            //            skill = skillElement.GetInt32();
            //        }
            //        if (ability.TryGetProperty("category", out JsonElement categElement))
            //        {
            //            cat = (AbilityCategory)categElement.GetInt32();
            //        }
            //        if (ability.TryGetProperty("Prereqs", out JsonElement prereqsElement))
            //        {
            //            foreach (var abilityElem in prereqsElement.EnumerateArray())
            //            {
            //                if (abilityElem.TryGetProperty("Name", out JsonElement prereqName))
            //                    prereqs.Add(prereqName.GetString());
            //            }
            //        }
            //
            //        Ability ab = new Ability(name, desc, "", skill, prereqs, rank, cat, align);
            //
            //        if (ab.IsValid())
            //        {
            //            abilityList.Add(ab);
            //        }
            //    }
            //
            //    foreach (Ability ab in abilityList)
            //    {
            //        Program.abilityLibrary.AddAbility(ab);
            //    }
            //}
            //
            //System.Console.WriteLine("Loaded Ability List");
        }

        //static public List<CharacterSheet> ImportCharacterSheetsFromDisk()
        //{
        //    List<CharacterSheet> importedSheets = new List<CharacterSheet>();
        //
        //    string curDir = Directory.GetCurrentDirectory();
        //
        //    if (!Directory.Exists($"{curDir}\\CharacterSheets"))
        //    {
        //        Directory.CreateDirectory($"{curDir}\\CharacterSheets");
        //        return importedSheets;
        //    }
        //
        //    // start with all txts
        //    DirectoryInfo directoryInfo = new DirectoryInfo(curDir + "\\CharacterSheets\\");
        //    FileInfo[] files = directoryInfo.GetFiles("*.txt");
        //
        //    foreach (FileInfo file in files)
        //    {
        //        string jsonString = File.ReadAllText(file.FullName);
        //
        //        using (JsonDocument document = JsonDocument.Parse(jsonString))
        //        {
        //            JsonElement root = document.RootElement;
        //
        //            string charName = "";
        //            string charDesc = "";
        //            string charAuth = "";
        //            Rank charRank = Rank.Invalid;
        //            Ability_Alignment charAlignment = Ability_Alignment.Ability_Invalid;
        //            List<string> abilities = new List<string>();
        //
        //            if (root.TryGetProperty("Name", out JsonElement nameElement))
        //            {
        //                charName = nameElement.GetString();
        //            }
        //            if (root.TryGetProperty("rank", out JsonElement rankElement))
        //            {
        //                charRank = (Rank)rankElement.GetInt32();
        //            }
        //            if (root.TryGetProperty("alignment", out JsonElement alignElement))
        //            {
        //                charAlignment = (Ability_Alignment)alignElement.GetInt32();
        //            }
        //            if (root.TryGetProperty("characterDescription", out JsonElement descElement))
        //            {
        //                charDesc = descElement.GetString();
        //            }
        //            if (root.TryGetProperty("Abilities", out JsonElement prereqsElement))
        //            {
        //                foreach (var abilityElem in prereqsElement.EnumerateArray())
        //                {
        //                    abilities.Add(abilityElem.GetString());
        //                }
        //            }
        //            if (root.TryGetProperty("Author", out JsonElement authorElement))
        //            {
        //                charAuth = authorElement.GetString();
        //            }
        //
        //            CharacterSheet characterSheet = new CharacterSheet(charName, charRank, charAlignment, abilities, charAuth);
        //            characterSheet.characterDescription = charDesc;
        //
        //            if (characterSheet.IsValid())
        //            {
        //                importedSheets.Add(characterSheet);
        //            }
        //        }
        //    }
        //
        //    return importedSheets;
        //}


        /***********************************************************************************************
         * LoadDiscordKey -- Will parse the txt file containing the discord key to log in and boot up  *
         *                   the discord bot.                                                          *
         *                                                                                             *
         *                                                                                             *
         *                                                                                             *
         * INPUT:    Nothing                                                                           *
         *                                                                                             *
         * OUTPUT:   bool - return true if successfuly loaded discord key, false otherwise.            *
         *           out string key - key string of the found discord 
         *                                                                                             *
         * WARNINGS: Should any changes be made in the ability class (such as the addition of vars),   *
         *           then the validation check must be updated to read and configure any updated       *
         *           class variables. Ability class constructor will set any vars to 'invalid' by      *
         *           default.                                                                          *
         *                                                                                             *
         * HISTORY:                                                                                    *
         *    2020-10-01 11:52PM EST : Created                                                         *
         *=============================================================================================*/
        static public bool LoadDiscordKey(out string key)
        {
            string curDir = Directory.GetCurrentDirectory();

            if (!Directory.Exists(curDir) && !File.Exists(curDir + "\\BotKey.txt"))
            {
                Console.WriteLine("Error! BotKey.txt could not be found!");
                Console.WriteLine("This Error is important as we could not find the discord bot key.");
                key = "";
                return false;
            }

            key = System.IO.File.ReadAllText(curDir + "\\BotKey.txt");
            return true;
        }

        /***********************************************************************************************
         * LoadAbilityLibrary -- Will load library json from AbilityLibrary.txt and cast the json      *
         *                       format to the needed Ability class template.                          *
         *                                                                                             *
         *                                                                                             *
         *                                                                                             *
         * INPUT:    Nothing                                                                           *
         *                                                                                             *
         * OUTPUT:   bool - return true if successfuly loaded discord key, false otherwise.            *
         *           out string key - key string of the found discord 
         *                                                                                             *
         * WARNINGS: Should any changes be made in the ability class (such as the addition of vars),   *
         *           then the validation check must be updated to read and configure any updated       *
         *           class variables. Ability class constructor will set any vars to 'invalid' by      *
         *           default.                                                                          *
         *                                                                                             *
         * HISTORY:                                                                                    *
         *    2020-10-01 11:52PM EST : Created                                                         *
         *=============================================================================================*/
        static public string GetTrelloLink()
        {
            string curDir = Directory.GetCurrentDirectory();

            if (!Directory.Exists(curDir) && !File.Exists(curDir + "\\Trello.txt"))
            {
                Console.WriteLine("Error! Trello.txt could not be found!");
                return "";
            }

            return System.IO.File.ReadAllText(curDir + "\\Trello.txt");
        }

        static public string GetRandomYoutubeLink()
        {
            string[] lines = System.IO.File.ReadAllLines(Directory.GetCurrentDirectory() + "\\Random\\RandomLinks.txt");

            int max = lines.Length;

            var rand = new Random();

            if (lines.Length > 0) return lines[rand.Next(0, max - 1)];

            return "";
        }

        static public string GetRandomImageFromFile()
        {
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\Random\\Images")) return "";

            var ext = new List<string> { "jpg", "gif", "png", "PNG" };
            var myFiles = Directory
                .EnumerateFiles(Directory.GetCurrentDirectory() + "\\Random\\Images", "*.*", SearchOption.AllDirectories)
                .Where(s => ext.Contains(Path.GetExtension(s).TrimStart('.').ToLowerInvariant()));

            var list = new List<string>();

            foreach (string str in myFiles)
            {
                list.Add(str);
            }

            if (list.Count == 0) return "";

            var rand = new Random();

            return list[rand.Next(0, list.Count)];
        }

        static public string GetRandomVideoFromFile()
        {
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\Random\\Videos")) return "";

            var ext = new List<string> { "mp4", "mov" };
            var myFiles = Directory
                .EnumerateFiles(Directory.GetCurrentDirectory() + "\\Random\\Videos", "*.*", SearchOption.AllDirectories)
                .Where(s => ext.Contains(Path.GetExtension(s).TrimStart('.').ToLowerInvariant()));

            var list = new List<string>();

            foreach (string str in myFiles)
            {
                list.Add(str);
            }

            if (list.Count == 0) return "";

            var rand = new Random();

            return list[rand.Next(0, list.Count)];
        }

        static public bool GetFirebaseJson(out string jsonString)
        {
            if (!File.Exists(Directory.GetCurrentDirectory() + "\\google-services.json"))
            {
                jsonString = "";
                return false;
            }

            jsonString = System.IO.File.ReadAllText(Directory.GetCurrentDirectory() + "\\google-services.json");
            return true;
        }
    }
}
