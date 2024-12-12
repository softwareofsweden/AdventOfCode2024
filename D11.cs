namespace aoc2024.Solutions
{
    internal static class D11
    {
        internal static List<long> GetStones()
        {
            var data = File.ReadAllText("Data\\d11.txt");
            var stonesText = data.Split(' ');
            var stones = new List<long>();
            foreach (var stone in stonesText)
                stones.Add(long.Parse(stone));
            return stones;
        }

        internal static void PrintStones(List<long> stones)
        {
            foreach (var stone in stones)
                Console.Write(stone.ToString() + " ");
            Console.WriteLine();
        }
        internal static List<long> Blink(List<long> stones)
        {
            var newStones = new List<long>();

            foreach (var stone in stones)
            {
                // If the stone is engraved with the number 0, it is
                // replaced by a stone engraved with the number 1.
                if (stone == 0)
                {
                    newStones.Add(1);
                }
                // If the stone is engraved with a number that has an
                // even number of digits, it is replaced by two stones
                else if (stone.ToString().Length % 2 == 0)
                {
                    var stoneText = stone.ToString();
                    var halfLength = stoneText.Length / 2;
                    // The left half of the digits are engraved on the
                    // new left stone
                    newStones.Add(long.Parse(stoneText.Substring(0, halfLength)));
                    // The right half of the digits are engraved on the new right stone
                    newStones.Add(long.Parse(stoneText.Substring(halfLength)));
                }
                // If none of the other rules apply, the stone is replaced
                // by a new stone; the old stone's number multiplied by 2024
                // is engraved on the new stone.
                else
                {
                    newStones.Add(stone * 2024);
                }
            }

            return newStones;
        }

        internal static Dictionary<string, long> cache = new Dictionary<string, long>();

        internal static long SplitStone(long stone, int count, int maxCount)
        {
            var key = stone.ToString() + ' ' + count.ToString();

            if (cache.ContainsKey(key))
                return cache[key];

            if (count > maxCount)
                return 1;

            if (stone == 0)
                return SplitStone(1, count + 1, maxCount);

            if (stone.ToString().Length % 2 == 0)
            {
                var stoneText = stone.ToString();
                var halfLength = stoneText.Length / 2;
                long stoneLeft = long.Parse(stoneText.Substring(0, halfLength));
                long stoneRight = long.Parse(stoneText.Substring(halfLength));
                return SplitStone(stoneLeft, count + 1, maxCount) + SplitStone(stoneRight, count + 1, maxCount);
            }

            key = (stone * 2024).ToString() + "_" + count.ToString();
            if (cache.ContainsKey(key))
               return cache[key];

            long result = SplitStone(stone * 2024, count + 1, maxCount);

            if (!cache.ContainsKey(key))
                cache.Add(key, result);

            return result;
        }

        /// <summary>
        /// How many stones will you have after blinking 25 times?
        /// </summary>
        internal static void Solve1()
        {
            var stones = GetStones();

            for (var i = 0; i < 25; i++)
                stones = Blink(stones);

            Console.WriteLine("199982");
            Console.WriteLine(stones.Count);
        }

        /// <summary>
        /// How many stones would you have after blinking a total of 75 times?
        /// </summary>
        internal static void Solve2()
        {
            var stones = GetStones();

            long count = 0;
            for (var i = 0; i < stones.Count; i++)
                count += SplitStone(stones[i], 1, 75);

            Console.WriteLine("237149922829154");
            Console.WriteLine(count);
        }
    }
}
