using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynovianEmpireDiscordBot.CharacterMakerClasses
{
    public struct SubmissionDetails
    {
        public string Author { get; set; }
        public string Date { get; set; }
        public bool OverrideSubmission { get; set; }
        public SubmissionDetails(string author, string date, bool override_sub)
        {
            Author = author;
            Date = date;
            OverrideSubmission = override_sub;
        }

        public SubmissionDetails(string author, bool override_sub)
        {
            Author = author;
            Date = DateTime.Now.ToString();
            OverrideSubmission = override_sub;
        }
    }
}
