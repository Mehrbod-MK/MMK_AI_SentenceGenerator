namespace MMK_AI_SentenceGenerator
{
    internal class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("In the name of God.");
            Console.WriteLine("Artificial Generative Intelligence for Sentence Generation appplication.");
            Console.WriteLine("Version 0.0.0.1");
            Console.WriteLine("Developed by:  Mehrbod Molla Kazemi");
            Console.WriteLine("Professor:  Dr. Ali Mahmudi Derakhsh");
            Console.WriteLine();
            Console.Write("Press ENTER key to begin...");
            Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine();

            // Open corpus.txt from Dataset and generate word tokens.
            try
            {
                Dataset.Dataset_GenerateWordTokens(@"Dataset\corpus.txt");
            }
            catch(Exception ex)
            {
                Console.WriteLine("ERROR Generating Word Tokens:  " + ex.Message);
                Console.ReadLine();
                return -2;
            }

            // Ask the user to enter a random number and choose from word tokens.
            Console.Write("\nEnter a random vocab index (0-" +
                (Dataset.num_Vocabs - 1).ToString() + "):\t");
            var tempStr = Console.ReadLine();

            int selected_VocabIndex = -1;
            if(String.IsNullOrEmpty(tempStr) || String.IsNullOrWhiteSpace(tempStr))
                selected_VocabIndex = 0;
            else try
                {
                    selected_VocabIndex = int.Parse(tempStr);
                }
                catch(Exception)
                {
                    selected_VocabIndex = 0;
                }

            // Guess the next probable word until the end of sentiment.
            int generatedWords = 0;
            string guessedWord = Dataset.wordTokens[selected_VocabIndex].word;
            string generatedSentence = guessedWord;
            while(generatedWords < Dataset.SENTENCE_GENERATION_THRESHOLD)
            {
                guessedWord = Dataset.NLP_GuessNextWord(guessedWord, 0.0009f);
                generatedSentence += ' ' + guessedWord;
                generatedWords++;
            }
            generatedSentence += ".";
            Console.WriteLine("\nRESULT => " + generatedSentence);
            Console.ReadLine();

            return 0;
        }
    }
}