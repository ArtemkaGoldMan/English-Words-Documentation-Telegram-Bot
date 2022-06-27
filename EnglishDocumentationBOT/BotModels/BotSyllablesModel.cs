using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishDocumentationBOT.BotModels
{
    public class BotSyllablesModel
    {
        public Syllables syllables { get; set; }
    }
    public class Syllables
    {
        public int count { get; set; }
        public List<string> list { get; set; }
    }
}
