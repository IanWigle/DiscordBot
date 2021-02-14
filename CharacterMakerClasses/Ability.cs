using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text.Json.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SynovianEmpireDiscordBot.CharacterMakerClasses
{
    public class Ability
    {
        public string Name { get; }
        public string Description { get; }
        public string detailedDescription { get; }
        public int skillCost { get; }
        public ArrayList prereqAbilities { get; }
        public CharacterRank rank { get; }
        public AbilityCategory category { get; }
        public CharacterAlignment alignment { get; }
        [JsonIgnore]
        public bool isFeat { get; }

        public Ability()
        {
            Name = "";
            Description = "";
            detailedDescription = "";
            skillCost = 0;
            prereqAbilities = new ArrayList();
            rank = CharacterRank.NoRank;
            category = AbilityCategory.NoCategory;
            isFeat = (category == AbilityCategory.Feat) ? true : false;
            alignment = CharacterAlignment.None;
            isFeat = false;
        }

        public Ability(string _name, string _desc, string _detailDesc, int _skill, string _prereq, CharacterRank _rank, AbilityCategory _category, CharacterAlignment _alignment)
        {
            Name = _name;
            Description = _desc;
            detailedDescription = _detailDesc;
            skillCost = _skill;
            prereqAbilities = new ArrayList();
            prereqAbilities.Add(_prereq);
            rank = _rank;
            category = _category;
            isFeat = (category == AbilityCategory.Feat) ? true : false;
            alignment = _alignment;
        }

        public Ability(string _name, string _desc, string _detailDesc, int _skill, ArrayList _prereqs, CharacterRank _rank, AbilityCategory _category, CharacterAlignment _alignment)
        {
            Name = _name;
            Description = _desc;
            detailedDescription = _detailDesc;
            skillCost = _skill;

            prereqAbilities = new ArrayList();
            foreach (var str in _prereqs)
            {
                prereqAbilities.Add(str);
            }

            rank = _rank;
            category = _category;
            isFeat = (category == AbilityCategory.Feat) ? true : false;
            alignment = _alignment;
        }

        public Ability(string _name, string _desc, string _detailDesc, int _skill, List<string> _prereqs, CharacterRank _rank, AbilityCategory _category, CharacterAlignment _alignment)
        {
            Name = _name;
            Description = _desc;
            detailedDescription = _detailDesc;
            skillCost = _skill;

            prereqAbilities = new ArrayList();
            foreach (var str in _prereqs)
            {
                prereqAbilities.Add(str);
            }

            rank = _rank;
            category = _category;
            isFeat = (category == AbilityCategory.Feat) ? true : false;
            alignment = _alignment;
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

        public string GetCategoryAsString()
        {
            switch (category)
            {
                case AbilityCategory.NoCategory:
                    return "None";
                case AbilityCategory.Alter:
                    return "Alter";
                case AbilityCategory.Control:
                    return "Control";
                case AbilityCategory.Sense:
                    return "Sense";
                case AbilityCategory.Meditation:
                    return "Meditation";
                case AbilityCategory.AlchemyCategory:
                    return "Alchemy";
                case AbilityCategory.ForceForm:
                    return "Force Form";
                case AbilityCategory.SaberForm:
                    return "Saber Form";
                case AbilityCategory.Feat:
                    return "Feat";
                case AbilityCategory.Explosives:
                    return "Explosives";
                case AbilityCategory.Mobility:
                    return "Mobility";
                case AbilityCategory.Blades:
                    return "Blades";
                case AbilityCategory.ArmsAbilities:
                    return "Arms Abilities";
                case AbilityCategory.Technology:
                    return "Technology";
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

        public bool HasPrereq(Ability _ability)
        {
            return HasPrereq(_ability.Name);
        }

        public bool HasPrereq(string _name)
        {
            foreach (var str in prereqAbilities)
            {
                if (_name == str as string)
                    return true;
            }

            return false;
        }

        public bool HasAllPrereqsInList(string[] _names)
        {
            foreach (var str in _names)
            {
                if (!prereqAbilities.Contains(str))
                    return false;
            }

            return true;
        }

        public bool HasAllPrereqsInList(ArrayList _names)
        {
            return HasAllPrereqsInList(_names.ToArray() as string[]);
        }

        public bool HasAllPrereqsInList(List<Ability> _names)
        {
            List<string> names = new List<string>();
            foreach (Ability ab in _names)
            {
                names.Add(ab.Name);
            }
            return HasAllPrereqsInList(names.ToArray());
        }

        public bool HasAllPrereqsInStringList(List<string> _names)
        {
            return HasAllPrereqsInList(_names.ToArray());
        }

        public bool IsValid()
        {
            return Name != "" && rank != CharacterRank.NoRank && category != AbilityCategory.NoCategory && alignment != CharacterAlignment.None;
        }
    }
}
