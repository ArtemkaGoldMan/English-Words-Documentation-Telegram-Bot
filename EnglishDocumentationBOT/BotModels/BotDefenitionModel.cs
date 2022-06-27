using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishDocumentationBOT.BotModels
{
    public class BotDefenitionModel
    {
        public string word { get; set; }
        public List<Definitions> definitions { get; set; }
    }

    public class Definitions
    {
        public string definition { get; set; }
        public string partOfSpeech { get; set; }
    }
}
