using System.Collections;
using System.Collections.Generic;
using SynovianEmpireDiscordBot.BotData;

using SynovianEmpireDiscordBot.CharacterMakerClasses;

namespace SynovianEmpireDiscordBot.Libraries
{
    public class AbilityLibrary
    {
        private readonly List<Ability> Abilities = new List<Ability>();

        public AbilityLibrary() 
        {
            //AddAbility(new Ability("testname1", "testDescription1", "testRank1", "testAlignment1"));
            //AddAbility(new Ability("testname2", "testDescription2", "testRank2", "testAlignment2"));
            //AddAbility(new Ability("testname3", "testDescription3", "testRank3", "testAlignment3"));
        }

        ~AbilityLibrary()
        {
            //InfoExportClass.ExportAbilityLibrary();
        }

        public bool LibraryHasAbility(string name)
        {
            foreach(Ability ab in Abilities)
            {
                if (ab.Name == name) return true;
            }

            return false;
        }

        public Ability GetAbility(string name)
        {
            if (LibraryHasAbility(name))
            {
                foreach (Ability ab in Abilities)
                    if (ab.Name == name) return ab;
            }

            return new Ability();
        }

        public void AddAbility(Ability ab)
        {
            if (Abilities.Contains(ab))
                return;

            Abilities.Add(ab);
        }

        public ArrayList GetAbilitiesByFilters(string rank = "", string alignment = "")
        {
            ArrayList list = new ArrayList();

            foreach(Ability ab in Abilities)
            {
                if (ab.GetRankAsString() == rank || rank == "")
                {
                    if (ab.GetAlignmentAsString() == alignment || alignment == "")
                    {
                        list.Add(ab);
                    }
                }
            }

            return list;
        }

        public List<Ability> GetAbilityList()
        {
            List<Ability> list = new List<Ability>();
            foreach(var ab in Abilities)
            {
                list.Add(ab as Ability);
            }
            return list;
        }

        public Dictionary<string,Ability> GetAbilitiesAsDictionary()
        {
            Dictionary<string, Ability> abilityDictionary = new Dictionary<string, Ability>();

            foreach(var ab in Abilities)
            {
                abilityDictionary[(ab as Ability).Name] = ab as Ability;
            }

            return abilityDictionary;
        }
    }
}
