namespace aoc2024.Solutions
{
    internal static class D07
    {
        internal class Equation
        {
            public long Answer { get; set; }

            public List<int> Numbers { get; set; } = [];

            private readonly List<long> _totals = [];

            private void CalcTotals(long total, int index)
            {
                if (index == Numbers.Count)
                {
                    _totals.Add(total);
                    return;
                }
                if (index == 0)
                {
                    CalcTotals(Numbers[index], index + 1);
                }
                else
                {
                    CalcTotals(total + Numbers[index], index + 1);
                    CalcTotals(total * Numbers[index], index + 1);
                }
            }

            private void CalcTotals2(long total, int index)
            {
                if (index == Numbers.Count)
                {
                    _totals.Add(total);
                    return;
                }
                if (index == 0)
                {
                    CalcTotals2(Numbers[index], index + 1);
                }
                else
                {
                    CalcTotals2(total + Numbers[index], index + 1);
                    CalcTotals2(total * Numbers[index], index + 1);
                    CalcTotals2(long.Parse(total.ToString() + Numbers[index].ToString()), index + 1);
                }
            }

            public bool IsValid()
            {
                _totals.Clear();
                CalcTotals(0, 0);

                foreach (long total in _totals)
                    if (total == Answer)
                        return true;

                return false;
            }

            public bool IsValid2()
            {
                _totals.Clear();
                CalcTotals2(0, 0);

                foreach (long total in _totals)
                    if (total == Answer)
                        return true;

                return false;
            }
        }

        internal static IEnumerable<Equation> ParseEquations(string[] lines)
        {
            var result = new List<Equation>();

            for (int i = 0; i < lines.Length; i++)
            {
                var e = new Equation();
                e.Answer = long.Parse(lines[i].Substring(0, lines[i].IndexOf(':')));
                var numbers = lines[i].Substring(lines[i].IndexOf(":") + 2).Split(' ');
                foreach (var number in numbers)
                    e.Numbers.Add(int.Parse(number));
                result.Add(e);
            }

            return result;
        }


        /// <summary>
        /// Determine which equations could possibly be true.
        /// What is their total calibration result?
        /// </summary>
        internal static void Solve1()
        {
            var lines = File.ReadAllLines("Data\\d07.txt");

            var equations = ParseEquations(lines);

            long total = 0;
            foreach (var equation in equations)
                if (equation.IsValid())
                    total += equation.Answer;

            Console.WriteLine(total);
        }

        /// <summary>
        /// Determine which equations could possibly be true.
        /// What is their total calibration result?
        /// </summary>
        internal static void Solve2()
        {
            var lines = File.ReadAllLines("Data\\d07.txt");

            var equations = ParseEquations(lines);

            long total = 0;

            foreach (var equation in equations)
                if (equation.IsValid2())
                    total += equation.Answer;

            Console.WriteLine(total);
        }
    }
}
