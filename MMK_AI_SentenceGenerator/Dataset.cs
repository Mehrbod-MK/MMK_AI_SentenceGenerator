using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MMK_AI_SentenceGenerator
{
    internal class Dataset
    {
        /// <summary>
        /// Represents structure for a word and its count amount in the corpus.
        /// </summary>
        public struct Struct_Word_Tokens
        {
            public string word = "";
            public ulong count = 0;

            /// <summary>
            /// Default ctor.
            /// </summary>
            public Struct_Word_Tokens() { }
        }

        #region MMK_AGI_Dataset_Fields

        public static List<Struct_Word_Tokens> wordTokens = new List<Struct_Word_Tokens>();
        public static ulong num_Sentiments = 0;
        public static ulong num_Vocabs = 0;
        public static ulong num_TotalWords = 0;

        #endregion

        /// <summary>
        /// Generates word tokens from an input file.
        /// </summary>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IOException"></exception>
        public static void Dataset_GenerateWordTokens(string inputFile)
        {
            wordTokens = new List<Struct_Word_Tokens>();
            num_Sentiments = 0;
            num_Vocabs = 0;
            num_TotalWords = 0;
            Console.WriteLine("Begin Function Dataset_GenerateWordTokens:\n");

            StreamReader reader = new StreamReader(inputFile);
            Console.WriteLine("Opened: " + inputFile);

            // Iterate through each line in the file.
            while(!reader.EndOfStream)
            {
                string? sentiment = reader.ReadLine();
                if (sentiment == null)
                    continue;

                num_Sentiments++;

                Console.WriteLine("<s>");

                var words = sentiment.Split(' ');
                foreach ( var wordTk in words )
                {
                    if (wordTk == null)
                        continue;

                    num_TotalWords++;

                    int wordIndex = wordTokens.FindIndex(x => x.word == wordTk);
                    if(wordIndex < 0)
                    {
                        wordTokens.Add(new Struct_Word_Tokens() { word = wordTk, count = 1 });
                        num_Vocabs++;
                        Console.WriteLine("Word \"" + wordTk + "\" not found in dictionary. Added!");
                    }
                    else
                    {
                        var foundToken = wordTokens[wordIndex];
                        foundToken.count++;
                        wordTokens[wordIndex] = foundToken;
                        Console.WriteLine("Word \"" + wordTk + "\" was already present in dictionary. Increased count to " + foundToken.count.ToString());
                    }
                }

                Console.WriteLine("</s>");
            }

            Console.WriteLine("Number of processed words:\t" + num_TotalWords);
            Console.WriteLine("Number of sentiments:\t" + num_Sentiments.ToString());
            Console.WriteLine("Vocabs (V)=\t" + num_Vocabs.ToString());

            reader.Close();
            Console.WriteLine("End Function:  Dataset_GenerateWordTokens");
        }
    }
}
