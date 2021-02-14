using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynovianEmpireDiscordBot.CharacterMakerClasses;
using SynovianEmpireDiscordBot.BotData;

namespace SynovianEmpireDiscordBot.Libraries
{
    public class CharacterLibrary
    {
        List<CharacterSheet> characterSheets = InfoImportClass.ImportCharacterSheetsFromDisk();
        public CharacterLibrary()
        {
        }

        ~CharacterLibrary()
        {
            InfoExportClass.ExportCharacterList(characterSheets);
        }

        public void AddCharacter(CharacterSheet sheet, bool overrideExisting, bool saveImmediatly = false)
        {
            // Check for duplicate sheets
            if(!characterSheets.Contains(sheet))
            {
                // Check if a sheet with two of the same character names exist. If the override
                // is true, remove the old one. If not set then we abort.
                CharacterSheet oldSheet = null;
                foreach(CharacterSheet characterSheet in characterSheets)
                {
                    if (characterSheet.Name == sheet.Name)
                    {
                        if (overrideExisting == false)
                            return;

                        oldSheet = characterSheet;
                    }
                }

                if(oldSheet != null)
                {
                    characterSheets.Remove(oldSheet);
                }
                characterSheets.Add(sheet);

                if(saveImmediatly)
                {
                    InfoExportClass.ExportCharacterSheet(sheet,overrideExisting);
                }

                Console.WriteLine("Added new character to library");
            }
        }

        public int NumberOfCharacters() { return characterSheets.Count; }

        public List<CharacterSheet> GetList() { return new List<CharacterSheet>(characterSheets); }

        public List<string> GetNameList()
        {
            List<string> names = new List<string>();
            foreach(var sheet in characterSheets)
            {
                names.Add(sheet.Name);
            }
            return names;
        }

        public CharacterSheet GetSheet(string name)
        {
            foreach(var sheet in characterSheets)
            {
                if (sheet.Name == name)
                    return sheet;
            }

            return null;
        }
    }
}
