using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishDocumentationBOT.BotModels
{
    public class BotPronunciationModel
    {
        public Pronunciations pronunciation { get; set; }
    }

    public class Pronunciations
    {
        public string all { get; set; }
        public string noun { get; set; }
        public string verb { get; set; }
    }
}
