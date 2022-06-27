using EnglishDocumentationBOT.BotConstants;
using EnglishDocumentationBOT.BotModels;
using Newtonsoft.Json;

namespace EnglishDocumentationBOT.DocumentationClient
{
    public class WordsClient
    {
        private HttpClient _client;
        private static string _address;
        public WordsClient()
        {
            _address = Constants.adress;

            _client = new HttpClient();
            _client.BaseAddress = new Uri(_address);
            _client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "65b4f0102fmshb6c930de7370f8cp1c15f2jsn687ead6d4e72");
            _client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "wordsapiv1.p.rapidapi.com");

        }
        //отримати значення 
        public async Task<BotDefenitionModel?> GetDefinisionOfWord(string Word)
        {
            var response = await _client.GetAsync($"/Defenition?Word={Word}");
            string err = response.StatusCode.ToString();

            Console.WriteLine("Status code" + err);
            if (err == "NotFound")
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<BotDefenitionModel>(content);
           
            return result;
        }

        //отримати синоніми 
        public async Task<BotSynonymsModel?> GetSynonyms(string Word)
        {

            var response = await _client.GetAsync($"/Synonims?Word={Word}");
            string err = response.StatusCode.ToString();

            Console.WriteLine("Status code" + err);
            if (err == "NotFound")
            {
                return null;
            }
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<BotSynonymsModel>(content);

            return result;

        }

        //отримати антоніми 
        public async Task<BotAntonymsModel?> GetAntonyms(string Word)
        {
            var response = await _client.GetAsync($"/Antonyms?Word={Word}");
            string err = response.StatusCode.ToString();

            Console.WriteLine("Status code" + err);
            if (err == "NotFound")
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<BotAntonymsModel>(content);
            
            return result;
        }

        //отримати приклад використання 
        public async Task<BotExamplesModel?> GetExamples(string Word)
        {
            var response = await _client.GetAsync($"/Examples?Word={Word}");
            string err = response.StatusCode.ToString();

            Console.WriteLine("Status code" + err);
            if (err == "NotFound")
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<BotExamplesModel>(content);
            
            return result;
        }

        //отримати вимову слова 
        public async Task<BotPronunciationModel?> GetPronunciation(string Word)
        {
            var response = await _client.GetAsync($"/Pronunciation?Word={Word}");
            string err = response.StatusCode.ToString();

            Console.WriteLine("Status code" + err);
            if (err == "NotFound")
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<BotPronunciationModel>(content);


            return result;
        }

        //отримати розклад на склади 
        public async Task<BotSyllablesModel?> GetSyllables(string Word)
        {
            var response = await _client.GetAsync($"/Syllables?Word={Word}");
            string err = response.StatusCode.ToString();

            Console.WriteLine("Status code" + err);
            if (err == "NotFound")
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<BotSyllablesModel>(content);

            return result;
        }

        //отримати схоже за значенням 
        public async Task<BotSimilarToModel?> GetSimilarTo(string Word)
        {
            var response = await _client.GetAsync($"/SimilarTo?Word={Word}");
            string err = response.StatusCode.ToString();

            Console.WriteLine("Status code" + err);
            if (err == "NotFound")
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();


            var result = JsonConvert.DeserializeObject<BotSimilarToModel>(content);
            
            return result;
        }

        //отримати слова з якими використовується 
        public async Task<BotUsingWithModel?> GetUsingWith(string Word)
        {
            var response = await _client.GetAsync($"/Usingwith?Word={Word}");
            string err = response.StatusCode.ToString();

            Console.WriteLine("Status code" + err);
            if (err == "NotFound")
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<BotUsingWithModel>(content);

            return result;
        }

        //отримати категорію 
        public async Task<BotCategoriesModel?> GetCategories(string Word)
        {
            var response = await _client.GetAsync($"/InCategory?Word={Word}");
            string err = response.StatusCode.ToString();

            Console.WriteLine("Status code" + err);
            if (err == "NotFound")
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<BotCategoriesModel>(content);

            return result;
        }

        //додати до словника
        public async Task<BotDictionaryModel?> PushDictionary(string Word, string userID)
        {
            var response = await _client.GetAsync($"/PushDictionary?Word={Word}&userID={userID}");
            string err = response.StatusCode.ToString();

            Console.WriteLine("Status code" + err);
            if (err == "NotFound")
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<BotDictionaryModel>(content);
           
            /*
            var dictionary = JsonConvert.SerializeObject(result);
            if (!Directory.Exists($"C:\\EnglishWordsDictionary\\{userID}"))
            {
                Directory.CreateDirectory($"C:\\EnglishWordsDictionary\\{userID}");
            }
            if (!File.Exists($"C:\\EnglishWordsDictionary\\{userID}\\{Word}.txt"))
            {
                File.Create($"C:\\EnglishWordsDictionary\\{userID}\\{Word}.txt").Close();

                File.WriteAllText($"C:\\EnglishWordsDictionary\\{userID}\\{Word}.txt", dictionary);
            }
            else if (File.Exists($"C:\\EnglishWordsDictionary\\{userID}\\{Word}.txt"))
            {
                File.WriteAllText($"C:\\EnglishWordsDictionary\\{userID}\\{Word}.txt", dictionary);
            }
            */
            return result;
        }
        //видалити зі словника
        public async Task<string?> DeleteDictionary(string Word, string userID)
        {
            await _client.DeleteAsync(Word);
            if (File.Exists($"C:\\EnglishWordsDictionary\\{userID}\\{Word}.txt"))
            {
                File.Delete($"C:\\EnglishWordsDictionary\\{userID}\\{Word}.txt");
            }
            else if (!File.Exists($"C:\\EnglishWordsDictionary\\{userID}\\{Word}.txt"))
            {
                Console.WriteLine($"Error.{Word}.txt not found. Maybe this word has not added to dictionary");
                return null;
            }
            return Word;
        }
        public async Task<string[]?> ShowAllWordsInDIC(string userID)
        {
            await _client.DeleteAsync(userID);

            var dir = new DirectoryInfo($"C:\\EnglishWordsDictionary\\{userID}");
            if (!dir.Exists)
            {
                return null;
            }
            var files = new List<string>();
            foreach (var file in dir.GetFiles())
            {
                files.Add(file.Name);
            }
            string[] arrWords = files.Select(n => n.TrimEnd().ToString()).ToArray();
            if(arrWords.Length == 0)
            {
                return null;
            }
            for (int i = 0; i < arrWords.Length; i++)
            {
                arrWords[i] = arrWords[i].Remove(arrWords[i].Length - 4);
            }
            return arrWords;
        }
    }
}
