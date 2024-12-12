namespace aoc2024.Solutions
{
    internal static class D10
    {
        private class Tile
        {
            public int Value { get; set; }

            public int X { get; set; }

            public int Y { get; set; }

            public Tile(int value, int x, int y)
            {
                Value = value;
                X = x;
                Y = y;
            }
        }

        private class Map
        {
            public int Width { get; set; }

            public int Height { get; set; }

            private List<Tile> _data;

            public Map(string fileName)
            {
                var lines = File.ReadAllLines(fileName);

                _data = new List<Tile>();

                Height = lines.Length;
                Width = lines[0].Length;

                for (int y = 0; y < Height; y++)
                    for (int x = 0; x < Width; x++)
                        _data.Add(new Tile(int.Parse(lines[y][x].ToString()), x, y));
            }

            public Tile GetTile(int x, int y)
            {
                int index = y * Height + x;
                return _data[index];
            }

            /// <summary>
            /// A trailhead is any position that starts one or more hiking trails - here,
            /// these positions will always have height 0
            /// </summary>
            /// <returns></returns>
            public IEnumerable<Tile> GetTrailHeads()
            {
                return _data.Where(x => x.Value == 0);
            }

            public void PrintToConsole()
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                        Console.Write(GetTile(x, y).Value.ToString());
                    Console.WriteLine();
                }
            }
        }

        private class Walker
        {
            private readonly Map map;
            private readonly int startX;
            private readonly int startY;

            private readonly HashSet<Tuple<int, int>> foundTops = new();

            private readonly bool ignoreFound;

            public Walker(Map map, int startX, int startY, bool ignoreFound)
            {
                this.map = map;
                this.startX = startX;
                this.startY = startY;
                this.ignoreFound = ignoreFound;
            }

            private int Walk(int v, int x, int y)
            {
                // Check outside
                if (x < 0) return 0;
                if (y < 0) return 0;
                if (x >= map.Width) return 0;
                if (y >= map.Height) return 0;

                var tileValue = map.GetTile(x, y).Value;

                // Need to be exaclty one more
                if (tileValue != v + 1)
                    return 0;

                // Found goal?
                if (tileValue == 9)
                {
                    if (!ignoreFound)
                    {
                        var top = new Tuple<int, int>(x, y);

                        // Already found?
                        if (foundTops.Contains(top))
                            return 0;

                        foundTops.Add(top);
                    }
                    return 1;
                }

                // Keep walking
                return Walk(v + 1, x - 1, y) // Left
                + Walk(v + 1, x + 1, y) // Right
                + Walk(v + 1, x, y - 1) // Up
                + Walk(v + 1, x, y + 1); // Down
            }

            public int FindTop()
            {
                foundTops.Clear();

                return Walk(0, startX - 1, startY) // Left
                + Walk(0, startX + 1, startY) // Right
                + Walk(0, startX, startY - 1) // Up
                + Walk(0, startX, startY + 1); // Down
            }
        }

        /// <summary>
        /// What is the sum of the scores of all trailheads on your topographic map?
        /// </summary>
        internal static void Solve1()
        {
            var map = new Map("Data\\d10.txt");

            var trailHeads = map.GetTrailHeads();

            var totalScore = 0;

            foreach (var trailHead in trailHeads)
                totalScore += new Walker(map, trailHead.X, trailHead.Y, false).FindTop();

            Console.WriteLine("820");
            Console.WriteLine(totalScore);
        }

        /// <summary>
        /// What is the sum of the ratings of all trailheads?
        /// </summary>
        internal static void Solve2()
        {
            var map = new Map("Data\\d10.txt");

            var trailHeads = map.GetTrailHeads();

            var totalScore = 0;

            foreach (var trailHead in trailHeads)
                totalScore += new Walker(map, trailHead.X, trailHead.Y, true).FindTop();

            Console.WriteLine("1786");
            Console.WriteLine(totalScore);
        }
    }
}
