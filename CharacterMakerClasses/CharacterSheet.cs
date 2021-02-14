using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Drawing;

namespace SynovianEmpireDiscordBot.CharacterMakerClasses
{
    public class CharacterSheet
    {
        public string Name { get; }
        public List<string> Abilities { get; }
        public CharacterRank rank { get; }
        public CharacterAlignment alignment { get; }
        [JsonIgnore]
        public int max_skill_points { get; }
        [JsonIgnore]
        public int current_skill_points { get; }
        [JsonIgnore]
        public int max_feat_points { get; }
        [JsonIgnore]
        public int current_feat_points { get; }

        public string lastEdit = "";
        [JsonIgnore]
        public bool wasChanged { get; set; }

        public string characterDescription { get; set; }
        public string Author { get; }
        [JsonIgnore]
        public Image characterImage { get; }
        [JsonIgnore]
        public string imageName { set; get; }

        public CharacterSheet()
        {
            Name = "";
            Abilities = new List<string>();
            rank = CharacterRank.NoRank;
            alignment = CharacterAlignment.None;
            max_skill_points = 0;
            current_skill_points = 0;
            max_feat_points = 0;
            current_feat_points = 0;
            lastEdit = "UNKNOWN";
            wasChanged = false;
            characterDescription = "";
        }

        public CharacterSheet(string newName, CharacterSheet sheet)
        {
            Name = newName;
            Abilities = sheet.Abilities;
            rank = sheet.rank;
            alignment = sheet.alignment;
            max_skill_points = DetermineMaxSkillByRank(rank);
            //current_skill_points = 0;
            max_feat_points = DetermineMaxFeetByRank(rank);
            //current_feat_points = 0;
            lastEdit = sheet.lastEdit;
            wasChanged = sheet.wasChanged;
            Author = sheet.Author;
            characterDescription = "";
        }

        public CharacterSheet(string _name, List<string> _abilities, CharacterRank _rank, CharacterAlignment _alignment)
        {
            Name = _name;
            Abilities = new List<string>(_abilities);
            rank = _rank;
            alignment = _alignment;
            //max_skill_points = DetermineMaxSkillByRank(_rank);
            //current_skill_points = CalculateCurrentSkillPoints();
            //max_feat_points = DetermineMaxFeetByRank(_rank);
            //current_feat_points = CalculateCurrentFeatPoints();
            lastEdit = "UNKNOWN";
            wasChanged = false;
            characterDescription = "";
        }

        public CharacterSheet(string _name, CharacterRank _rank, CharacterAlignment _alignment)
        {
            Name = _name;
            Abilities = new List<string>();
            rank = _rank;
            alignment = _alignment;
            //max_skill_points = DetermineMaxSkillByRank(_rank);
            //current_skill_points = CalculateCurrentSkillPoints();
            //max_feat_points = DetermineMaxFeetByRank(_rank);
            //current_feat_points = CalculateCurrentFeatPoints();
            lastEdit = "UNKNOWN";
            wasChanged = false;
            characterDescription = "";
        }

        public CharacterSheet(string _name, List<string> _abilities, CharacterRank _rank, CharacterAlignment _alignment, string _lastEdit)
        {
            Name = _name;
            Abilities = new List<string>(_abilities);
            rank = _rank;
            alignment = _alignment;
            //max_skill_points = DetermineMaxSkillByRank(_rank);
            //current_skill_points = CalculateCurrentSkillPoints();
            //max_feat_points = DetermineMaxFeetByRank(_rank);
            //current_feat_points = CalculateCurrentFeatPoints();
            //lastEdit = _lastEdit;
            wasChanged = false;
            characterDescription = "";
        }

        public CharacterSheet(string _name, CharacterRank _rank, CharacterAlignment _alignment, string _lastEdit, string author)
        {
            Name = _name;
            Abilities = new List<string>();
            rank = _rank;
            alignment = _alignment;
            //max_skill_points = DetermineMaxSkillByRank(_rank);
            //current_skill_points = CalculateCurrentSkillPoints();
            //max_feat_points = DetermineMaxFeetByRank(_rank);
            //current_feat_points = CalculateCurrentFeatPoints();
            lastEdit = _lastEdit;
            wasChanged = false;
            Author = author;
            characterDescription = "";
        }
        public CharacterSheet(string _name, CharacterRank _rank, CharacterAlignment _alignment, List<string> abilities,string _lastEdit, string author)
        {
            Name = _name;
            Abilities = abilities;
            rank = _rank;
            alignment = _alignment;
            //max_skill_points = DetermineMaxSkillByRank(_rank);
            //current_skill_points = CalculateCurrentSkillPoints();
            //max_feat_points = DetermineMaxFeetByRank(_rank);
            //current_feat_points = CalculateCurrentFeatPoints();
            lastEdit = _lastEdit;
            wasChanged = false;
            Author = author;
            characterDescription = "";
        }

        public CharacterSheet(string _name, CharacterRank _rank, CharacterAlignment _alignment, List<string> abilities,string _author)
        {
            Name = _name;
            rank = _rank;
            alignment = _alignment;
            Abilities = abilities;
            Author = _author;
            characterDescription = "";
        }

        public CharacterSheet(string jsonString)
        {
            using(JsonDocument document = JsonDocument.Parse(jsonString))
            {
                JsonElement root = document.RootElement;

                if (root.TryGetProperty("Name", out JsonElement nameElement))
                {
                    Name = nameElement.GetString();
                }
                if (root.TryGetProperty("rank", out JsonElement rankElement))
                {
                    rank = (CharacterRank)rankElement.GetInt32();
                }
                if (root.TryGetProperty("alignment", out JsonElement alignElement))
                {
                    alignment = (CharacterAlignment)alignElement.GetInt32();
                }
                if (root.TryGetProperty("characterDescription", out JsonElement descElement))
                {
                    characterDescription = descElement.GetString();
                }
                if (root.TryGetProperty("Abilities", out JsonElement prereqsElement))
                {
                    foreach (var abilityElem in prereqsElement.EnumerateArray())
                    {
                        Abilities.Add(abilityElem.GetString());
                    }
                }
                if (root.TryGetProperty("Author", out JsonElement authorElement))
                {
                    Author = authorElement.GetString();
                }
            }
        }

        public CharacterSheet(string jsonString, Image image)
        {
            using (JsonDocument document = JsonDocument.Parse(jsonString))
            {
                JsonElement root = document.RootElement;

                if (root.TryGetProperty("Name", out JsonElement nameElement))
                {
                    Name = nameElement.GetString();
                }
                if (root.TryGetProperty("rank", out JsonElement rankElement))
                {
                    rank = (CharacterRank)rankElement.GetInt32();
                }
                if (root.TryGetProperty("alignment", out JsonElement alignElement))
                {
                    alignment = (CharacterAlignment)alignElement.GetInt32();
                }
                if (root.TryGetProperty("characterDescription", out JsonElement descElement))
                {
                    characterDescription = descElement.GetString();
                }
                if (root.TryGetProperty("Abilities", out JsonElement prereqsElement))
                {
                    Abilities = new List<string>();
                    foreach (var abilityElem in prereqsElement.EnumerateArray())
                    {
                        Abilities.Add(abilityElem.GetString());
                    }
                }
                if (root.TryGetProperty("Author", out JsonElement authorElement))
                {
                    Author = authorElement.GetString();
                }
            }

            characterImage = image;
        }

        //public int CalculateCurrentSkillPoints()
        //{
        //    int points = 0;
        //    foreach (string str in Abilities)
        //    {
        //        if (Program.abilityDictionary.TryGetAbility(str, out Ability ab))
        //        {
        //            points += ab.skillCost;
        //        }
        //    }
        //
        //    return points;
        //}

        //public int CalculateCurrentFeatPoints()
        //{
        //    int points = 0;
        //    foreach (string str in Abilities)
        //    {
        //        if (Program.abilityDictionary.TryGetAbility(str, out Ability ab))
        //        {
        //            if (ab.isFeat) points += ab.skillCost;
        //        }
        //    }
        //    return points;
        //}

        static public int DetermineMaxSkillByRank(CharacterRank rank)
        {
            switch (rank)
            {
                case CharacterRank.Acolyte:
                    return 5;
                case CharacterRank.Apprentice:
                    return 10;
                case CharacterRank.Knight:
                    return 30;
                case CharacterRank.Lord:
                    return 60;
                case CharacterRank.Archon:
                    return 80;
                case CharacterRank.Elder:
                    return 100;
                case CharacterRank.Emperor:
                    return 140;
                default:
                    return 0;
            }
        }

        static public int DetermineMaxFeetByRank(CharacterRank rank)
        {
            switch (rank)
            {
                case CharacterRank.Acolyte:
                    return 10;
                case CharacterRank.Apprentice:
                    return 15;
                case CharacterRank.Knight:
                    return 20;
                case CharacterRank.Lord:
                    return 30;
                case CharacterRank.Archon:
                    return 40;
                case CharacterRank.Elder:
                    return 50;
                case CharacterRank.Emperor:
                    return 60;
                default:
                    return 0;
            }
        }

        //public void AddAbility(Ability ab)
        //{
        //    AddAbility(ab.Name);
        //}

        public void AddAbility(string ab)
        {
            if (Abilities.Contains(ab)) return;

            Abilities.Add(ab);

            if (!wasChanged) wasChanged = true;
        }

        // Will return true if successful, false if failed
        public bool RemoveAbility(string ab)
        {
            if (!Abilities.Contains(ab)) return false;

            Abilities.Remove(ab);

            if (!wasChanged) wasChanged = true;

            return true;
        }

        //public bool RemoveAbility(Ability ab)
        //{
        //    return RemoveAbility(ab.Name);
        //}

        public bool HasAbility(string str)
        {
            return Abilities.Contains(str);
        }

        //public bool HasAbility(Ability ab)
        //{
        //    foreach (string str in Abilities)
        //    {
        //        if (ab.Name == str)
        //            return true;
        //    }
        //
        //    return false;
        //}

        public bool IsValid()
        {
            return Name != "" && rank != CharacterRank.NoRank && alignment != CharacterAlignment.None;
        }

        public string GetRankAsString()
        {
            switch (rank)
            {
                case CharacterRank.Acolyte:
                    return "Acolyte";
                case CharacterRank.Apprentice:
                    return "Apprentice";
                case CharacterRank.Knight:
                    return "Knight";
                case CharacterRank.Lord:
                    return "Lord";
                case CharacterRank.Archon:
                    return "Archon";
                case CharacterRank.Elder:
                    return "Elder";
                case CharacterRank.Emperor:
                    return "Emperor";
                default:
                    return "NULL";
            }
        }

        public string GetAlignmentAsString()
        {
            switch (alignment)
            {
                case CharacterAlignment.NoForce:
                    return "non-force";
                case CharacterAlignment.Neutral:
                    return "Neutral";
                case CharacterAlignment.Lightside:
                    return "Lightside";
                case CharacterAlignment.Darkside:
                    return "Darkside";
                case CharacterAlignment.AlignAlchemy:
                    return "Alchemy";
                default:
                    return "NULL";
            }
        }
    }
}

