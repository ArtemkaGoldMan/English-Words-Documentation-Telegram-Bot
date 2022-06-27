using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishDocumentationBOT.BotModels
{
    public class BotDictionaryModel
    {
        public string word { get; set; }
        public List<Result> results { get; set; }
        public Pronunciation pronunciation { get; set; }
    }
    public class Pronunciation
    {
        public string all { get; set; }
    }

    public class Result
    {
        public string definition { get; set; }
        public string partOfSpeech { get; set; }
    }
}
