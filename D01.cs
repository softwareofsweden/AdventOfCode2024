namespace aoc2024.Solutions
{
    internal static class D01
    {
        /// <summary>
        /// Pair up the smallest number in the left list with the smallest number 
        /// in the right list, then the second-smallest left number with the 
        /// second-smallest right number, and so on.
        /// What is the total distance between your lists?
        /// </summary>
        internal static void Solve1()
        {
            var data = File.ReadAllLines("Data\\d01.txt");

            var left = new List<int>();
            var right = new List<int>();

            for (int i = 0; i < data.Length; i++)
            {
                var parts = data[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var leftNumber = int.Parse(parts[0].Trim());
                var rightNumber = int.Parse(parts[1].Trim());
                left.Add(leftNumber);
                right.Add(rightNumber);
            }

            left.Sort();
            right.Sort();

            int dist = 0;
            for (int i = 0; i < left.Count; i++)
                dist += Math.Abs(left[i] - right[i]);

            Console.WriteLine(dist);
        }

        /// <summary>
        /// This time, you'll need to figure out exactly how often each number from 
        /// the left list appears in the right list. Calculate a total similarity 
        /// score by adding up each number in the left list after multiplying it by 
        /// the number of times that number appears in the right list.
        /// What is their similarity score?
        /// </summary>
        internal static void Solve2()
        {
            var data = File.ReadAllLines("Data\\d01.txt");

            var left = new List<int>();
            var right = new List<int>();

            for (int i = 0; i < data.Length; i++)
            {
                var parts = data[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var leftNumber = int.Parse(parts[0].Trim());
                var rightNumber = int.Parse(parts[1].Trim());
                left.Add(leftNumber);
                right.Add(rightNumber);
            }

            int score = 0;
            for (int i = 0; i < left.Count; i++)
                score += left[i] * right.Count(x => x == left[i]);

            Console.WriteLine(score);
        }
    }
}
