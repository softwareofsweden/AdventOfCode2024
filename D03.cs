using System.Text.RegularExpressions;

namespace aoc2024.Solutions
{
    internal static class D03
    {
        /// <summary>
        /// What do you get if you add up all of the results of the multiplications?
        /// </summary>
        internal static void Solve1()
        {
            var memory = File.ReadAllText("Data\\d03.txt");

            long sum = 0;

            // Find mul(x,y)
            var regex = new Regex(@"mul\(\d+,\d+\)");
            var matches = regex.Matches(memory);

            foreach (Match match in matches)
            {
                var values = match.Value.Replace("mul(", "").Replace(")", "").Split(',');
                var n1 = int.Parse(values[0]);
                var n2 = int.Parse(values[1]);
                sum += n1 * n2;
            }

            Console.WriteLine(sum);
        }

        /// <summary>
        /// What do you get if you add up all of the results of the multiplications?
        /// The do() instruction enables future mul instructions.
        /// The don't() instruction disables future mul instructions.
        /// </summary>
        internal static void Solve2()
        {
            var memory = File.ReadAllText("Data\\d03.txt");

            long sum = 0;

            // Find do() | don't() | mul(x,y)
            var regex = new Regex(@"do\(\)|don't\(\)|mul\(\d+,\d+\)");
            var matches = regex.Matches(memory);

            bool mulEnabled = true;

            foreach (Match match in matches)
            {
                if (match.Value == "do()")
                {
                    mulEnabled = true;
                    continue;
                }

                if (match.Value == "don't()")
                {
                    mulEnabled = false;
                    continue;
                }

                if (!mulEnabled)
                    continue;

                var values = match.Value.Replace("mul(", "").Replace(")", "").Split(',');
                var n1 = int.Parse(values[0]);
                var n2 = int.Parse(values[1]);
                sum += n1 * n2;
            }

            Console.WriteLine(sum);
        }
    }
}
