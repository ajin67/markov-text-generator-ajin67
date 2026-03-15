namespace MarkovTextGenerator;

public class Program
{
    static void Main(string[] args)
    {
        Chain chain = new Chain();

        Console.WriteLine("Welcome to Marky Markov's Random Text Generator!");

        string dataFile = args.Length > 0 ? args[0] : "Sample.txt";
        LoadText(dataFile, chain);

        // Now let's update all the probabilities with the new data
        chain.UpdateProbabilities();

        // Okay now for the fun part
        Console.WriteLine("Done learning!  Now give me a word and I'll tell you what comes next.");
        Console.Write("> ");

        var word = Console.ReadLine() ?? string.Empty;
        var nextWord = chain.GetNextWord(word);
        Console.WriteLine("I predict the next word will be " + (string.IsNullOrEmpty(nextWord) ? "<end of sentence>" : nextWord));

        if (chain.Words.Count > 0)
        {
            string startingWord = chain.GetRandomStartingWord();
            string generatedSentence = chain.GenerateSentence(startingWord);
            Console.WriteLine($"Random generated sentence: {generatedSentence}");
        }
        else
        {
            Console.WriteLine("No training data was loaded, so no random sentence could be generated.");
        }
    }

    static void LoadText(string filename, Chain chain)
    {
        string path = Path.Combine(Environment.CurrentDirectory, "Data", filename);

        if (!File.Exists(path))
        {
            Console.WriteLine($"Training file not found: {path}");
            return;
        }

        var lines = File.ReadAllLines(path);
        foreach (var line in lines)
        {
            chain.AddSentence(line);
        }

        Console.WriteLine($"Loaded {lines.Length} training lines from {filename}.");
    }
}
