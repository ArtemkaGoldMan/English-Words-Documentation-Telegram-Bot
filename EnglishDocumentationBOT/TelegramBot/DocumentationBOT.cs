using EnglishDocumentationBOT.BotConstants;
using EnglishDocumentationBOT.DocumentationClient;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Exceptions;
using System.Text.RegularExpressions;

namespace EnglishDocumentationBOT
{
    public class DocumentationBOT
    {
        public static string? LastMessage;
        public static int? Count;
        public static string? Word;

        WordsClient wordsClient = new WordsClient();

        TelegramBotClient botClient = new TelegramBotClient(Constants.telegramBotToken);
        CancellationToken cancellationToken = new CancellationToken();
        ReceiverOptions receiverOptions = new ReceiverOptions { AllowedUpdates = { } };
        public async Task Start()
        {
            botClient.StartReceiving(HandlerUpdateSync, HandlerErrorAsync, receiverOptions, cancellationToken);
            var Botme = await botClient.GetMeAsync();
            Console.WriteLine($"The Bot {Botme.Username} starts working!");
            Console.ReadKey();
        }

        private Task HandlerErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Error in the Telegram Bot API:\n {apiRequestException.ErrorCode}" +
                $"\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        private async Task HandlerUpdateSync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update?.Message?.Text != null)
            {
                await HandlerMessageAsync(botClient, update.Message);
            }
        }

        private async Task HandlerMessageAsync(ITelegramBotClient botClient, Message message)
        {
            if (message.Text == "/start")
            {
                
                await botClient.SendTextMessageAsync(message.Chat.Id, $"Write '/help' to see, what this bot can do.\nWrtire '/keyboard' to use this bot's abilities");
                return;
            }
            else if (message.Text == "/help")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "English: This bot can help you to find some information of English words like:" +
                                                                      "synonyms, antonyms, defenition, syllabels, pronunciation, with what use this words," +
                                                                      "examples of using, the similar words to your(not synonym), categories of your word.\n" +
                                                                      "also you can add this word to a Dictionary\n" +
                                                                      "Rules of the use :\nInscribing is needed only the English words, without errors." +
                                                                      "If in text showed out an error, then it is needed to begin to produce operating " +
                                                                      "under beginning->from the choice of the desirable button." +
                                                                      "\n *************************\n" +
                                                                      "Українською: Цей телеграм бот може допомогти вам знайти різну інформацію про англійські слова " +
                                                                      "таку як: синоніми, антоніми, значення, розклад на склади, приклад використання, слова схожі за значенням" +
                                                                      "слова, з якими використовується ваше слово та кактегорія слова.\n" +
                                                                      "Також ви можете добавити свої слова до словнка\n" +
                                                                      "Правила використання:\nВписувати потрібно тільки англійські слова, без помилок. Якщо в тексті вивело помилку" +
                                                                      ", то потрібно почати виконувати дію з початку->з вибору бажаної кнопки.");
                return;
            }
            else if (message.Text == "/keyboard")
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = new
                    (
                        new[]
                        {
                            new KeyboardButton[]{ "Definition", "Synonyms","Antonyms"},
                            new KeyboardButton[]{ "Examples","Pronunciation", "Syllables" },     
                            new KeyboardButton[]{ "Similar words", "Using with words","Categories" },
                            new KeyboardButton[]{ "Add to dictionary", "Show dictionary", "Delete from dictionary"},     
                        }
                    )
                {
                    ResizeKeyboard = true
                };
                await botClient.SendTextMessageAsync(message.Chat.Id, "Choose an one item", replyMarkup: replyKeyboardMarkup);
            }
            else if (message.Text == "Definition")
            {
                LastMessage = "Definition";
                await botClient.SendTextMessageAsync(message.Chat.Id, "Write a word:");
                return;
            }
            else if (message.Text == "Synonyms")
            {
                LastMessage = "Synonyms";
                await botClient.SendTextMessageAsync(message.Chat.Id, "Write a word:");
                return;
            }
            else if (message.Text == "Antonyms")
            {
                LastMessage = "Antonyms";
                await botClient.SendTextMessageAsync(message.Chat.Id, "Write a word:");
                return;
            }
            else if (message.Text == "Examples")
            {
                LastMessage = "Examples";
                await botClient.SendTextMessageAsync(message.Chat.Id, "Write a word:");
                return;
            }
            else if (message.Text == "Pronunciation")
            {
                LastMessage = "Pronunciation";
                await botClient.SendTextMessageAsync(message.Chat.Id, "Write a word:");
                return;
            }
            else if (message.Text == "Syllables")
            {
                LastMessage = "Syllables";
                await botClient.SendTextMessageAsync(message.Chat.Id, "Write a word:");
                return;
            }
            else if (message.Text == "Using with words")
            {
                LastMessage = "Using with words";
                await botClient.SendTextMessageAsync(message.Chat.Id, "Write a word:");
                return;
            }
            else if (message.Text == "Categories")
            {
                LastMessage = "Categories";
                await botClient.SendTextMessageAsync(message.Chat.Id, "Write a word:");
                return;
            }
            else if (message.Text == "Similar words")
            {
                LastMessage = "Similar words";
                await botClient.SendTextMessageAsync(message.Chat.Id, "Write a word:");
                return;
            }
            else if (message.Text == "Add to dictionary")
            {
                LastMessage = "Add to dictionary";
                await botClient.SendTextMessageAsync(message.Chat.Id, "Write a word:");
                return;
            }
            else if (message.Text == "Show dictionary")
            {
                _ = ShowDictionary(botClient, message);
                return;
            }
            else if (message.Text == "Delete from dictionary")
            {
                LastMessage = "Delete from dictionary";
                await botClient.SendTextMessageAsync(message.Chat.Id, "Write a word:");
                return;
            }
            else if (message.Text != null)
            {
                _ = ButtonAbilities(botClient, message);
                return;
            }
        }

        private async Task ButtonAbilities(ITelegramBotClient botClient, Message message)
        {
            if (Regex.IsMatch(message.Text, "^[a-zA-Z0-9]*$"))
            {
                if (LastMessage == "Definition")
                {
                    Word = message.Text;
                    string? def;
                    string? pOfSpeac;

                    Count = wordsClient.GetDefinisionOfWord(Word).Result?.definitions.Count;
                    if (Count == null)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"\t\t!!!Error!!!\nSomething went wrong. You may have entered the word incorrectly");
                        return;
                    }

                    for (int i = 0; i < Count; i++)
                    {
                        def = wordsClient.GetDefinisionOfWord(Word).Result?.definitions[i].definition;
                        pOfSpeac = wordsClient.GetDefinisionOfWord(Word).Result?.definitions[i].partOfSpeech;
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Definition of '{Word}':\n{def}\n*************\nPart of speach : {pOfSpeac}");
                    }
                    return;

                }

                else if (LastMessage == "Synonyms")
                {
                    Word = message.Text;
                    string? syn = null;

                    Count = wordsClient.GetSynonyms(Word).Result?.synonyms.Count;
                    if(Count == null)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"\t\t!!!Error!!!\nSomething went wrong. You may have entered the word incorrectly");
                        return;
                    }

                    
                    for (int i = 0; i < Count; i++)
                    {
                        if (i == Count - 1)
                        {
                            syn += wordsClient.GetSynonyms(Word).Result?.synonyms[i];
                        }
                        else
                        {
                            syn += wordsClient.GetSynonyms(Word).Result?.synonyms[i] + ", ";
                        }

                    }
                    if (syn == null)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Maybe this word is not in WEB API database");
                        return;
                    }
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Synonyms of '{Word}' :\n{syn}");
                    return;

                }

                else if (LastMessage == "Antonyms")
                {
                    Word = message.Text;
                    string? ant = null;

                    Count = wordsClient.GetAntonyms(Word).Result?.antonyms.Count;
                    if (Count == null)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"\t\t!!!Error!!!\nSomething went wrong. You may have entered the word incorrectly");
                        return;
                    }

                    
                    for (int i = 0; i < Count; i++)
                    {
                        if (i == Count - 1)
                        {
                            ant += wordsClient.GetAntonyms(Word).Result?.antonyms[i];
                        }
                        else
                        {
                            ant += wordsClient.GetAntonyms(Word).Result?.antonyms[i] + ", ";
                        }

                    }
                    if (ant == null)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Maybe this word is not in WEB API database");
                        return;
                    }
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Antonyms of '{Word}' :\n{ant}");
                    return;

                }

                else if (LastMessage == "Examples")
                {
                    Word = message.Text;
                    string? exp=null;

                    Count = wordsClient.GetExamples(Word).Result?.examples.Count;
                    if (Count == null)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"\t\t!!!Error!!!\nSomething went wrong. You may have entered the word incorrectly");
                        return;
                    }

                   
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Examples of '{Word}':");
                    for (int i = 0; i < Count; i++)
                    {
                        exp = wordsClient.GetExamples(Word).Result?.examples[i];

                        await botClient.SendTextMessageAsync(message.Chat.Id, $"{exp}");
                    }
                    if (exp == null)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Maybe this word is not in WEB API database");
                        return;
                    }
                    return;

                }

                else if (LastMessage == "Pronunciation")
                {
                    Word = message.Text;

                    string? pro = wordsClient.GetPronunciation(Word).Result?.pronunciation.all;
                    if (pro == null)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"\t\t!!!Error!!!\nSomething went wrong. You may have entered the word incorrectly" +
                            $"or this word is not in WEB API database");
                        return;
                    }

                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Pronunciation of '{Word}' :\n{pro}");
                    return;


                }

                else if (LastMessage == "Syllables")
                {
                    Word = message.Text;
                    string? syl = null;
                    Count = wordsClient.GetSyllables(Word).Result?.syllables.list.Count;
                    if (Count == null)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"\t\t!!!Error!!!\nSomething went wrong. You may have entered the word incorrectly");
                        return;
                    }

                    int? countOfSyllabels = wordsClient.GetSyllables(Word).Result?.syllables.count;
                    
                    for (int i = 0; i < Count; i++)
                    {
                        if (i == Count - 1)
                        {
                            syl += wordsClient.GetSyllables(Word).Result?.syllables.list[i];
                        }
                        else
                        {
                            syl += wordsClient.GetSyllables(Word).Result?.syllables.list[i] + "-";
                        }
                    }
                    if(syl == null)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Maybe this word is not in WEB API database");
                        return;
                    }

                    await botClient.SendTextMessageAsync(message.Chat.Id, $"This word has {countOfSyllabels} syllabels.\n" +
                                                                          $"Syllables of '{Word}' :\n{syl}");
                    return;

                }

                else if (LastMessage == "Using with words")
                {
                    Word = message.Text;
                    string? usw = null;

                    Count = wordsClient.GetUsingWith(Word).Result?.also.Count;
                    if (Count == null)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"\t\t!!!Error!!!\nSomething went wrong. You may have entered the word incorrectly");
                        return;
                    }

                    
                    for (int i = 0; i < Count; i++)
                    {
                        if (i == Count - 1)
                        {
                            usw += wordsClient.GetUsingWith(Word).Result?.also[i];
                        }
                        else
                        {
                            usw += wordsClient.GetUsingWith(Word).Result?.also[i] + ", ";
                        }
                    }
                    if(usw == null)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Maybe this word is not in WEB API database");
                        return;
                    }

                    await botClient.SendTextMessageAsync(message.Chat.Id, $"UsingWith of '{Word}' :\n{usw}");
                    return;

                }

                else if (LastMessage == "Categories")
                {
                    Word = message.Text;
                    string? cat = null;

                    Count = wordsClient.GetCategories(Word).Result?.inCategory.Count;
                    if (Count == null)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"\t\t!!!Error!!!\nSomething went wrong. You may have entered the word incorrectly");
                        return;
                    }
                    

                    for (int i = 0; i < Count; i++)
                    {
                        if (i == Count - 1)
                        {
                            cat += wordsClient.GetUsingWith(Word).Result?.also[i];
                        }
                        else
                        {
                            cat += wordsClient.GetUsingWith(Word).Result?.also[i] + ", ";
                        }
                    }
                    if (cat == null)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Maybe this word is not in WEB API database");
                        return;
                    }

                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Categories of '{Word}' :\n{cat}");
                    return;

                }

                else if (LastMessage == "Similar words")
                {
                    Word = message.Text;
                    string? smt = null;

                    Count = wordsClient.GetSimilarTo(Word).Result?.similarTo.Count;
                    if (Count == null)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"\t\t!!!Error!!!\nSomething went wrong. You may have entered the word incorrectly");
                        return;
                    }
                    

                    for (int i = 0; i < Count; i++)
                    {
                        if (i == Count - 1)
                        {
                            smt += wordsClient.GetSimilarTo(Word).Result?.similarTo[i];
                        }
                        else
                        {
                            smt += wordsClient.GetSimilarTo(Word).Result?.similarTo[i] + ", ";

                        }
                    }
                    if (smt == null)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Maybe this word is not in WEB API database");
                        return;
                    }
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"SimilarTo of '{Word}' :\n{smt}");

                }

                else if (LastMessage == "Add to dictionary")
                {
                    string userID =message.From.Id.ToString();
                    Word = message.Text;

                    string? ask = wordsClient.PushDictionary(Word, userID).Result?.word;
                    if (ask == null)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"\t\t!!!Error!!!\nSomething went wrong. You may have entered " +
                                                                              $"the word incorrectly");
                        return;
                    }

                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Word {ask} was added to dictionary");
                    return;

                }

                else if (LastMessage == "Delete from dictionary")
                {
                    Word = message.Text;
                    string userID = message.From.Id.ToString();

                    var  delete = wordsClient.DeleteDictionary(Word, userID);
                    if (delete == null)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"\t\t!!!Error!!!\nSomething went wrong. You may " +
                                                                              $"have entered the word incorrectly or this word is" +
                                                                              $" not in the dictionary");
                        return;
                    }

                    await botClient.SendTextMessageAsync(message.Chat.Id, $"The word {Word} was deleted");

                    return;

                }
            }
            else
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, $"!!!Error!!!\n" +
                    "Your word was not written in English. Please, try again from the start");
            }
        }
        private async Task ShowDictionary(ITelegramBotClient botClient, Message message)
        {
            string userID = message.From.Id.ToString();
            string[]? allfills = wordsClient.ShowAllWordsInDIC(userID).Result;
            if (allfills == null)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, $"!!!Error!!!\nSomething went wrong. You may have entered the word incorrectly \nor\n" +
                    $"Maybe Your dictionary is empty. For the start add some words to a dictionary");
                return;
            }
            Count = allfills.Length;
            string? allwords = null;
            for (int i = 0; i < Count; i++)
            {
                if (i == Count - 1)
                {
                    allwords += allfills[i];
                }
                else
                {
                    allwords += allfills[i] + ", ";
                }

            }
            await botClient.SendTextMessageAsync(message.Chat.Id, $"All words in your dictionary: {allwords}");
            return;
        }
    }
}