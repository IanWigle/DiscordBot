using System;
using System.Collections;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using SynovianEmpireDiscordBot.Libraries;   

namespace SynovianEmpireDiscordBot.BotData
{
    /***********************************************************************************************
     * InfoExportClass -- A static class manager to handle the export of various amounts of data   *
     *                    from files within the program.                                           *
     *                                                                                             *
     *                                                                                             *
     *                                                                                             *
     * INPUT:    Various                                                                           *
     *                                                                                             *
     * OUTPUT:   Nothing                                                                           *
     *                                                                                             *
     * WARNINGS: None                                                                              *
     *                                                                                             *
     * HISTORY:                                                                                    *
     *    2020-09-25 11:34AM EST : Created                                                         *
     *=============================================================================================*/
    static public class InfoExportClass
    {
        static public void DumpLogOutput()
        {
            string curDir = Directory.GetCurrentDirectory() + "\\Logs\\";

            if (!Directory.Exists(curDir))
            {
                Directory.CreateDirectory(curDir);
            }

            string[] arrayList = (string[])LogCatalog.GetLogHistory().ToArray(typeof(string));
            string filename = "LogDump.txt";
            System.IO.File.WriteAllLines(curDir + filename, arrayList);

        }

        /***********************************************************************************************
         * ExportBlacklist -- Exports the blacklist to the Whitelist.txt file                          *
         *                                                                                             *
         *                                                                                             *
         *                                                                                             *
         *                                                                                             *
         * INPUT:    Nothing                                                                           *
         *                                                                                             *
         * OUTPUT:   Blacklist.txt                                                                     *
         *                                                                                             *
         * WARNINGS: None                                                                              *
         *                                                                                             *
         * HISTORY:                                                                                    *
         *    2020-09-25 11:03AM EST : Created                                                         *
         *=============================================================================================*/
        static public void ExportBlacklist()
        {
            string curDir = Directory.GetCurrentDirectory() + "\\Lists\\";

            if (!Directory.Exists(curDir))
            {
                Directory.CreateDirectory(curDir);
            }

            //if (Directory.Exists(curDir+"Whitelist.txt"))
            //{
            //    System.IO.File.Delete(curDir + "Whitelist.txt");
            //}

            ArrayList list = BotInfo.Permissions.GetBlackList();
            string[] arrayList = (string[])list.ToArray(typeof(string));
            System.IO.File.WriteAllLines(curDir + "Blacklist.txt", arrayList);
        }

        /***********************************************************************************************
         * ExportWhiteList -- Exports the whitelist to the Whitelist.txt file                          *
         *                                                                                             *
         *                                                                                             *
         *                                                                                             *
         *                                                                                             *
         * INPUT:    Nothing                                                                           *
         *                                                                                             *
         * OUTPUT:   Whitelist.txt                                                                     *
         *                                                                                             *
         * WARNINGS: None                                                                              *
         *                                                                                             *
         * HISTORY:                                                                                    *
         *    2020-09-25 11:03AM EST : Created                                                         *
         *=============================================================================================*/
        static public void ExportWhiteList()
        {
            string curDir = Directory.GetCurrentDirectory() + "\\Lists\\";

            if (!Directory.Exists(curDir))
            {
                Directory.CreateDirectory(curDir);
            }

            //if (Directory.Exists(curDir+"Whitelist.txt"))
            //{
            //    System.IO.File.Delete(curDir + "Whitelist.txt");
            //}

            ArrayList list = BotInfo.Permissions.GetWhiteList();
            string[] arrayList = (string[])list.ToArray(typeof(string));
            System.IO.File.WriteAllLines(curDir + "Whitelist.txt", arrayList);
        }

        /***********************************************************************************************
         * ExportAbilityLibrary -- Exports the ability library stored in RAM to a json txt file        *
         *                                                                                             *
         *                                                                                             *
         *                                                                                             *
         *                                                                                             *
         * INPUT:    Nothing                                                                           *
         *                                                                                             *
         * OUTPUT:   AbilityLibrary.txt                                                                *
         *                                                                                             *
         * WARNINGS: None                                                                              *
         *                                                                                             *
         * HISTORY:                                                                                    *
         *    2020-09-25 11:03AM EST : Created                                                         *
         *=============================================================================================*/
        //static public void ExportAbilityLibrary()
        //{
        //    string curDir = Directory.GetCurrentDirectory() + "\\Lists\\";
        //
        //    if (!Directory.Exists(curDir))
        //    {
        //        Directory.CreateDirectory(curDir);
        //        Console.WriteLine("Error! List Folder could not be found!");
        //    }
        //
        //    var options = new JsonSerializerOptions
        //    {
        //        WriteIndented = true,
        //    };
        //
        //    string jsonString = JsonSerializer.Serialize(Program.abilityLibrary.GetAbilityList(), options);
        //    System.IO.File.WriteAllText(curDir + "AbilityLibrary.txt", jsonString);
        //    System.Console.WriteLine("Ability Library exported to " + curDir + "AbilityLibrary.txt");
        //}

        //static public void ExportCharacterSheet(CharacterSheet sheet, bool overriteExisting = true)
        //{
        //    const bool removeInvalidCharacters = true;
        //
        //    bool characterNameValid = (
        //            sheet.Name != "" &&
        //                (
        //                    !sheet.Name.Contains('<') &&
        //                    !sheet.Name.Contains('>') &&
        //                    !sheet.Name.Contains(':') &&
        //                    !sheet.Name.Contains('\"') &&
        //                    !sheet.Name.Contains('/') &&
        //                    !sheet.Name.Contains('\\') &&
        //                    !sheet.Name.Contains('|') &&
        //                    !sheet.Name.Contains('?') &&
        //                    !sheet.Name.Contains('*')
        //                )
        //            );
        //
        //    void SaveSheet(CharacterSheet character)
        //    {
        //        string curDir = Directory.GetCurrentDirectory();
        //        string fullFilename = $"{curDir}\\CharacterSheets\\{character.Name}.txt";
        //
        //        if (!Directory.Exists($"{curDir}\\CharacterSheets"))
        //            Directory.CreateDirectory($"{curDir}\\CharacterSheets");
        //
        //        if (File.Exists(fullFilename) && overriteExisting)
        //        {
        //            File.Delete(fullFilename);
        //
        //            var options = new JsonSerializerOptions
        //            {
        //                WriteIndented = true,
        //            };
        //
        //            string jsonString = JsonSerializer.Serialize(character, options);
        //            System.IO.File.WriteAllText(fullFilename, jsonString);
        //        }
        //        else
        //        {
        //            var options = new JsonSerializerOptions
        //            {
        //                WriteIndented = true,
        //            };
        //
        //            string jsonString = JsonSerializer.Serialize(character, options);
        //            System.IO.File.WriteAllText($"{curDir}\\CharacterSheets\\{character.Name}.txt", jsonString);
        //        }
        //    }
        //
        //
        //    if (characterNameValid)
        //    {
        //        SaveSheet(sheet);
        //    }
        //    else if (characterNameValid == false && removeInvalidCharacters)
        //    {
        //        string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
        //        string invalidName = sheet.Name;
        //
        //        foreach (char c in invalid)
        //        {
        //            invalidName = invalidName.Replace(c.ToString(), "");
        //        }
        //
        //        if (invalidName == "")
        //        {
        //            DirectoryInfo directoryInfo = new DirectoryInfo($"{Directory.GetCurrentDirectory()}\\CharacterSheets\\");
        //            FileInfo[] files = directoryInfo.GetFiles("*.txt");
        //            invalidName = $"CharacterSheet{files.Length + 1}";
        //        }
        //
        //        CharacterSheet characterSheet = new CharacterSheet(invalidName, sheet);
        //        SaveSheet(characterSheet);
        //    }
        //}
        //
        //static public void ExportCharacterList(List<CharacterSheet> sheets, bool overriteExisting = false)
        //{
        //    foreach (CharacterSheet sheet in sheets)
        //    {
        //        ExportCharacterSheet(sheet, overriteExisting);
        //    }
        //}
    }
}
