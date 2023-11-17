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
            public int count = 0;

            /// <summary>
            /// Default ctor.
            /// </summary>
            public Struct_Word_Tokens() { }
        }

        #region MMK_AGI_Dataset_Constants

        public const int SENTENCE_GENERATION_THRESHOLD = 7;

        #endregion

        #region MMK_AGI_Dataset_Fields

        public static List<Struct_Word_Tokens> wordTokens = new List<Struct_Word_Tokens>();
        public static int num_Sentiments = 0;
        public static int num_Vocabs = 0;
        public static int num_TotalWords = 0;

        public static List<string> corpus = new List<string>();

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

                // Update corpus content.
                corpus.Add(sentiment);

                Console.WriteLine("<s>");

                var words = sentiment.Split(' ');
                foreach ( var wordTk in words )
                {
                    if (wordTk == null || String.IsNullOrEmpty(wordTk) || String.IsNullOrWhiteSpace(wordTk))
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

        public static string NLP_GuessNextWord(string wi, float? threshold = null)
        {
            Console.WriteLine("Begin Function:  NLP_GuessNextWord");

            float max_Probability = 0.0f;
            string bestWord = wi;

            List<string> thresholdCandidates = new List<string>();

            foreach(var token in wordTokens)
            {
                float p = NLP_CalculateProbabilityAdd1BiGram(wi, token.word);
                if (p < 0.0f)
                    continue;

                Console.WriteLine("P(" + wi + " | " + token.word + ")=\t" + p.ToString());

                if(threshold != null)
                {
                    if (p >= threshold)
                        thresholdCandidates.Add(token.word);
                }
                else if (max_Probability < p)
                {
                    max_Probability = p;
                    bestWord = token.word;
                }
            }

            if(thresholdCandidates.Count > 0 && threshold != null)
            {
                Random random = new Random((int)DateTime.Now.Ticks);
                int randomNumber = random.Next(thresholdCandidates.Count);

                bestWord = thresholdCandidates[randomNumber];
                max_Probability = (float)threshold;
            }
            else if(thresholdCandidates.Count == 0 && threshold != null)
            {
                Random random = new Random((int)DateTime.Now.Ticks);
                int randomNumber = random.Next(wordTokens.Count);

                bestWord = wordTokens[randomNumber].word;
                max_Probability = (float)threshold;
            }

            Console.WriteLine("Best guess:\t" + bestWord + "\twith probability:\t" + max_Probability.ToString());
            Console.WriteLine("End Function:  NLP_GuessNextWord");

            return bestWord;
        }

        public static float NLP_CalculateProbabilityAdd1BiGram(string wi, string wj)
        {
            var token_WiIndex = wordTokens.FindIndex(x => x.word == wi);
            if (token_WiIndex < 0)
                return -1.0f;

            int N_WiWj = NLP_CountBiGrams(wi, wj);
            int N_Wi = wordTokens[token_WiIndex].count;

            return ((float)(N_WiWj + 1)) / ((float)(N_Wi + num_TotalWords + 1));
        }

        public static int NLP_CountBiGrams(string wi, string wj)
        {
            int resultOccs = 0;

            foreach(var line in corpus)
            {
                if (String.IsNullOrEmpty(line) || String.IsNullOrWhiteSpace(line))
                    continue;

                var wordTokens = line.Split(' ');
                for(int i = 0; i < wordTokens.Length; i++)
                {
                    string word = wordTokens[i];
                    string nextWord = (i + 1 < wordTokens.Length) ? wordTokens[i + 1] : String.Empty;

                    if (String.IsNullOrEmpty(word) ||
                        String.IsNullOrWhiteSpace(word))
                        continue;

                    if (word == wi && nextWord == wj)
                        resultOccs++;
                }
            }

            return resultOccs;
        }
    }
}
