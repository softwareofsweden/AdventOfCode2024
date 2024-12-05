namespace aoc2024.Solutions
{
    internal static class D02
    {
        internal static List<int> GetLevels(string report)
        {
            var levels = new List<int>();
            var lev = report.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lev.Length; i++)
                levels.Add(int.Parse(lev[i]));
            return levels;
        }

        internal static bool LevelsIncreasing(List<int> levels)
        {
            for (int i = 0; i < levels.Count - 1; i++)
                if (!(levels[i + 1] > levels[i]))
                    return false;
            return true;
        }

        internal static bool LevelsDecreasing(List<int> levels)
        {
            for (int i = 0; i < levels.Count - 1; i++)
                if (!(levels[i + 1] < levels[i]))
                    return false;
            return true;
        }

        internal static bool AnyTwoAdjacentLevelsDifferByAtLeastOneAndAtMostThree(List<int> levels)
        {
            for (int i = 0; i < levels.Count - 1; i++)
            {
                var diff = Math.Abs(levels[i + 1] - levels[i]);
                if (!(diff > 0 && diff <= 3))
                    return false;
            }
            return true;
        }

        internal static bool IsReportSafe(List<int> levels)
        {
            return (LevelsIncreasing(levels) || LevelsDecreasing(levels)) &&
                    AnyTwoAdjacentLevelsDifferByAtLeastOneAndAtMostThree(levels);
        }

        /// <summary>
        /// Analyze the unusual data from the engineers. How many reports are safe?
        /// </summary>
        internal static void Solve1()
        {
            var reports = File.ReadAllLines("Data\\d02.txt");
            int safe = 0;
            foreach (var report in reports)
            {
                var levels = GetLevels(report);
                if (IsReportSafe(levels))
                    safe++;
            }

            Console.WriteLine(safe);
        }

        /// <summary>
        /// Analyze the unusual data from the engineers. How many reports are safe?
        /// Now, the same rules apply as before, except if removing a single level 
        /// from an unsafe report would make it safe, the report instead counts as safe.
        /// </summary>
        internal static void Solve2()
        {
            var reports = File.ReadAllLines("Data\\d02.txt");
            int safe = 0;
            foreach (var report in reports)
            {
                var levels = GetLevels(report);
                if (IsReportSafe(levels))
                {
                    safe++;
                }
                else
                {
                    // removing a single level from an unsafe report would make it safe
                    for (int i = 0; i < levels.Count; i++)
                    {
                        var levels2 = new List<int>();
                        for (int j = 0; j < levels.Count; j++)
                            if (j != i) levels2.Add(levels[j]);

                        if (IsReportSafe(levels2))
                        {
                            safe++;
                            break;
                        }
                    }
                }
            }

            Console.WriteLine(safe);
        }
    }
}
