namespace MMK_AI_SentenceGenerator
{
    internal class Program
    {
        static void Main(string[] args)
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

            // Open corpus.txt from Dataset.
            try
            {
                Dataset.Dataset_GenerateWordTokens(@"Dataset\corpus.txt");
            }
            catch(Exception ex)
            {

            }
        }
    }
}