using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text.Json.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SynovianEmpireDiscordBot.CharacterMakerClasses
{
    /// <summary>
    /// The alignment of characters and abilities/feats that can determine or aid in filtering, and unlocking options.
    /// </summary>
    public enum Ability_Alignment
    {
        /// <summary>
        /// Reserved for error checking.
        /// </summary>
        Ability_Invalid,
        Ability_Neutral,
        Ability_Light,
        Ability_Dark,
        Ability_NonForce,
        /// <summary>
        /// Reserved for non learning abilities such as school prereqs.
        /// </summary>
        Ability_Other,
        /// <summary>
        /// Reserved for program calculating.
        /// </summary>
        Ability_Max
    }

    /// <summary>
    /// The rank of characters and abilities/feats that can determine or aid in filtering, and unlocking options.
    /// </summary>
    public enum Rank
    {
        /// <summary>
        /// Reserved for error checking.
        /// </summary>
        Invalid,
        Initiate,
        Acolyte,
        Apprentice,
        Knight,
        Lord,
        Archon,
        Elder,
        Emperor,
        /// <summary>
        /// Reserved for program calculating.
        /// </summary>
        Max
    }

    public enum Ability_Schools
    {
        /// <summary>
        /// Reserved for error checking.
        /// </summary>
        Ability_Invalid,
        Ability_Defense,
        Ability_Offense,
        Ability_Mentalism,
        Ability_Survival,
        Ability_Understanding,
        Ability_Forms,
        Ability_Arms,
        Ability_Explosives,
        Ability_Close_Quarters,
        Ability_Mobility,
        Ability_Medical,
        Ability_Engineering,
        Ability_Technology,
        Ability_Droids,
        Ability_Relics,
        Ability_Taming,
        /// <summary>
        /// Reserved for program calculating.
        /// </summary>
        Ability_Max
    }

    public enum Ability_Mastery
    {
        Mastery_NotLearned,
        Mastery_Learned,
        Mastery_Master,
        Mastery_HeadMaster
    }

    public class Ability
    {
        /*
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
        */

        /// <summary>
        /// The unique ID of an ability.
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// Unique ability name.
        /// </summary>
        [JsonIgnore]
        public string Name { get; private set; }

        /// <summary>
        /// Enum value for an abilities name.
        /// </summary>
        public Ability_Alignment alignment { get; private set; }

        /// <summary>
        /// Returns a string of the enum value name. Removes the "Ability_" prefix.
        /// </summary>
        public string s_alignment { get => Enum.GetName(typeof(Ability_Alignment), alignment).Replace("Ability_", ""); }

        /// <summary>
        /// Enum value for ability rank.
        /// </summary>
        public Rank Rank { get; private set; }

        /// <summary>
        /// Returns a string of the enum value name.
        /// </summary>
        public string s_rank { get => Enum.GetName(typeof(Rank), Rank); }

        /// <summary>
        /// Enum value for the school category of the ability.
        /// </summary>
        public Ability_Schools ability_School { get; private set; }

        /// <summary>
        /// Returns a string of the enum value name. Removes the "Ability_" prefix and replaces '_' with spaces.
        /// </summary>
        public string s_ability_School { get => Enum.GetName(typeof(Ability_Schools), ability_School).Replace("Ability_", "").Replace('_', ' '); }

        /// <summary>
        /// The skill cost of an ability. By default the cost of an ability equals 1.
        /// </summary>
        public int skillCostOverride { get; private set; }

        /// <summary>
        /// Returns the skill cost as a string.
        /// </summary>
        public string s_skillCostOverride { get => skillCostOverride.ToString(); }

        /// <summary>
        /// Unique ability description.
        /// </summary>
        public string description { get; private set; }

        /// <summary>
        /// Returns a string list of all the prerequisite abilitys for the ability. Returns the names, not IDs.
        /// </summary>
        //public List<string> s_prereqs
        //{
        //    get
        //    {
        //        List<string> list = new List<string>();
        //        if (Program.abilityLibrary == null)
        //            return list;
        //        else
        //        {
        //            foreach (int i in prereqs)
        //            {
        //                if (Program.abilityLibrary.TryGetAbility(i, out Ability ability))
        //                    list.Add(ability.Name);
        //            }
        //            return list;
        //        }
        //    }
        //}

        /// <summary>
        /// Integer list of the ability IDs required for this ability.
        /// </summary>
        public List<int> prereqs { get; private set; }

        /// <summary>
        /// Boolian flag to determine if the ability is a Feat or not.
        /// </summary>
        public bool isFeat { get; private set; }
    }
}
