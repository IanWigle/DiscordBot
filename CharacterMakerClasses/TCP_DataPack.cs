using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynovianEmpireDiscordBot.CharacterMakerClasses
{
    class TCP_DataPack
    {
        public CharacterSheet characterSheet { get; set; }

        public SubmissionDetails submissionDetails { get; set; }

        public TCP_DataPack(CharacterSheet sheet, SubmissionDetails submission)
        {
            characterSheet = sheet;
            submissionDetails = submission;
        }
    }
}
