using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace TextAnalysis
{
    public class TextAnalyzer
    {
        public string[] GetBannedKeyWords()
        {
            var words = File.ReadLines("./Input/Banned.txt");
            return words.ToArray();
        }

        public string[] GetBannedChars()
        {
            return new string[] { ",", ";", ":", "-" };
        }

        public KeyWord[] FindKeyWords(string[] sentences)
        {
            string text = string.Join(String.Empty, sentences);
            return FindKeyWords(text);
        }


        public KeyWord[] FindKeyWords(string text)
        {
            var allKeywords = new Dictionary<string, int>();
            var bannedWords = new HashSet<string>(GetBannedKeyWords());

            var sanitized = text.ToLower();
            foreach (var bannedChar in GetBannedChars())
            {
                sanitized = sanitized.Replace( bannedChar, String.Empty );
            }

            var words = sanitized.Split(' ');
            foreach (var word in words.Where(x => !bannedWords.Contains(x)))
            {
                if (word == "")
                {
                    continue;
                }

                if (!allKeywords.ContainsKey(word))
                {
                    allKeywords[word] = 0;
                }

                allKeywords[word]++;
            }

            var ordered = allKeywords.OrderByDescending(x => x.Value).Take(10);
            return ordered.Select(x => new KeyWord() { Name = x.Key, Value = x.Value }).ToArray();
        }
    }
}
