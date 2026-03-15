namespace MarkovTextGenerator;

public class Program
{
    static void Main(string[] args)
    {
        Chain chain = new Chain();

        Console.WriteLine("Welcome to Marky Markov's Random Text Generator!");

        string dataFile = args.Length > 0 ? args[0] : "Sample.txt";
        int loadedLines = LoadText(dataFile, chain);

        if (loadedLines == 0)
        {
            Console.WriteLine("No training data loaded. Please fix the file path/name and run again.");
            return;
        }

        // Now let's update all the probabilities with the new data
        chain.UpdateProbabilities();

        // Okay now for the fun part
        Console.WriteLine("Done learning!  Now give me a word and I'll tell you what comes next.");
        Console.Write("> ");

        var word = Console.ReadLine() ?? string.Empty;
        var nextWord = chain.GetNextWord(word);
        Console.WriteLine("I predict the next word will be " + (string.IsNullOrEmpty(nextWord) ? "<end of sentence>" : nextWord));

        string startingWord = chain.GetRandomStartingWord();
        string generatedSentence = chain.GenerateSentence(startingWord);
        Console.WriteLine($"Random generated sentence: {generatedSentence}");
    }

    static int LoadText(string filename, Chain chain)
    {
        string[] candidatePaths =
        [
            Path.Combine(Environment.CurrentDirectory, "Data", filename),
            Path.Combine(Environment.CurrentDirectory, "MarkovTextGenerator", "Data", filename),
            Path.Combine(AppContext.BaseDirectory, "Data", filename),
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Data", filename),
        ];

        string? existingPath = candidatePaths
            .Select(Path.GetFullPath)
            .FirstOrDefault(File.Exists);

        if (existingPath is null)
        {
            Console.WriteLine("Training file not found. Checked:");
            foreach (var candidatePath in candidatePaths.Select(Path.GetFullPath))
            {
                Console.WriteLine($" - {candidatePath}");
            }

            return 0;
        }

        var lines = File.ReadAllLines(existingPath);
        foreach (var line in lines)
        {
            chain.AddSentence(line);
        }

        Console.WriteLine($"Loaded {lines.Length} training lines from {existingPath}.");
        return lines.Length;
    }
}
