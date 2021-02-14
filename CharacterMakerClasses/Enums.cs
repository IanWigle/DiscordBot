using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace SynovianEmpireDiscordBot.CharacterMakerClasses
{
    public enum CharacterRank : int
    {
        NoRank = 0,
        Acolyte,
        Apprentice,
        Knight,
        Lord,
        Archon,
        Elder,
        Emperor,
        MaxValue
    }

    public enum AbilityCategory : int
    {
        NoCategory = 0,
        Alter,
        Control,
        Sense,
        Meditation,
        AlchemyCategory,
        ForceForm,
        SaberForm,
        Explosives,
        Mobility,
        Blades,
        ArmsAbilities,
        Technology,
        Feat,
        Max
    }

    public enum CharacterAlignment : int
    {
        None = 0,
        NoForce,
        Neutral,
        Lightside,
        Darkside,
        AlignAlchemy,
        MaxVariable
    }
}
