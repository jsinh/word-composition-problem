namespace WordCompProb
{
    #region Namespace

    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    #endregion

    class Program
    {
        private static string firstOhYeah;
        private static string secondOhYeah;
        static void Main(string[] args)
        {
            // User input
            START:
            firstOhYeah = null;
            secondOhYeah = null;
            Console.WriteLine("Path to file full of wordzz:");

            var filePath = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(filePath))
            {
                Console.Clear();
                goto START;
            }

            string[] wordsAlphaOrdered;
            try
            {
                // Read file as lines into string array
                wordsAlphaOrdered = File.ReadAllLines(filePath);
            }
            catch (Exception)
            {
                Console.Clear();
                Console.WriteLine("Unable to read file - try again!");
                goto START;
            }

            // Measure time.
            var sw = Stopwatch.StartNew();
            GoForIt(wordsAlphaOrdered);
            sw.Stop();
            LetTheWorldKnow(sw.ElapsedMilliseconds);
            goto START;
            // Console.ReadLine();
        }

        public static void GoForIt(string[] wordsAlphaOrdered)
        {
            // Sort by descending for string length.
            var wordsSizeDescOrdered = wordsAlphaOrdered.OrderByDescending(item => item.Length).ThenByDescending(item => item).ToList();

            // Pick each item from above sort once by one to find a match of what we are looking for.
            foreach (var wordItem in wordsSizeDescOrdered)
            {
                // Already found our two matches - let's break this loop.
                if (null != firstOhYeah && null != secondOhYeah)
                    break;

                // Copy current word - will be used for replace manipulation ahead.
                var wordResidue = wordItem;

                // Use same list to get each word for a contains match with "wordItem"
                foreach (var targetWord in wordsSizeDescOrdered)
                {
                    // If both are the word return early - this will give false positive if not checked. ||
                    // If comparing word is bigger then what we are examining - no point - return early.
                    if (wordItem == targetWord || targetWord.Length > wordResidue.Length)
                        continue;

                    // Seek posibilities of a match.
                    var matchIndex = wordResidue.IndexOf(targetWord, StringComparison.Ordinal);

                    // If no match return early.
                    if (matchIndex == -1)
                        continue;

                    // Wipe out the matching word from examined word.
                    wordResidue = wordResidue.Replace(targetWord, "");
                }

                // Goal is to bring "wordResidue" copy to empty so we know we found a match.
                if (wordResidue.Length != 0)
                    continue;

                // Set the match - goal is to find two when we are done there break so we don't iterate without any reason.
                if (string.IsNullOrWhiteSpace(firstOhYeah))
                    firstOhYeah = wordItem;
                else if (string.IsNullOrWhiteSpace(secondOhYeah))
                    secondOhYeah = wordItem;
                else
                    break;
            }
        }

        public static void LetTheWorldKnow(long elapsed)
        {
            if (!string.IsNullOrWhiteSpace(firstOhYeah) && !string.IsNullOrWhiteSpace(firstOhYeah))
                Console.WriteLine($"Found two matches: {firstOhYeah} || {secondOhYeah}");
            else if (!string.IsNullOrWhiteSpace(firstOhYeah) && string.IsNullOrWhiteSpace(firstOhYeah))
                Console.WriteLine($"One match found: {firstOhYeah}");
            else
                Console.WriteLine("You are messing with me, no match found");

            Console.WriteLine($"Nailed it in: {elapsed}millisec");
        }
    }
}