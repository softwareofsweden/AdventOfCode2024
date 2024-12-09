namespace aoc2024.Solutions
{
    internal static class D08
    {
        /// <summary>
        /// How many unique locations within the bounds
        /// of the map contain an antinode?
        /// </summary>
        internal static void Solve1()
        {
            var lines = File.ReadAllLines("Data\\d08.txt");

            var antennas = new List<Tuple<int, int, char>>();
            var antinodeLocations = new HashSet<Tuple<int, int>>();

            var maxX = lines[0].Length;
            var maxY = lines.Length;

            for (int y = 0; y < maxY; y++)
                for (int x = 0; x < maxX; x++)
                    if (lines[y][x] != '.')
                        antennas.Add(new Tuple<int, int, char>(x, y, lines[y][x]));

            foreach (var antenna in antennas)
            {
                var otherAntennas = antennas.Where(x => x.Item3 == antenna.Item3 && (x.Item1 != antenna.Item1 || x.Item2 != antenna.Item2));
                foreach (var other in otherAntennas)
                {
                    var x = antenna.Item1 + (antenna.Item1 - other.Item1);
                    var y = antenna.Item2 + (antenna.Item2 - other.Item2);
                    if (x >= 0 && y >= 0 && x < maxX && y < maxY)
                        antinodeLocations.Add(new Tuple<int, int>(x, y));
                }
            }

            Console.WriteLine(antinodeLocations.Count);
        }

        /// <summary>
        /// How many unique locations within the bounds of the map contain 
        /// an antinode?
        /// After updating your model, it turns out that an antinode occurs 
        /// at any grid position exactly in line with at least two antennas 
        /// of the same frequency, regardless of distance.
        /// </summary>
        internal static void Solve2()
        {
            var lines = File.ReadAllLines("Data\\d08.txt");

            var antennas = new List<Tuple<int, int, char>>();
            var antinodeLocations = new HashSet<Tuple<int, int>>();

            var maxX = lines[0].Length;
            var maxY = lines.Length;

            for (int y = 0; y < maxY; y++)
                for (int x = 0; x < maxX; x++)
                    if (lines[y][x] != '.')
                        antennas.Add(new Tuple<int, int, char>(x, y, lines[y][x]));

            foreach (var antenna in antennas)
            {
                var otherAntennas = antennas.Where(x => x.Item3 == antenna.Item3 && (x.Item1 != antenna.Item1 || x.Item2 != antenna.Item2));
                foreach (var other in otherAntennas)
                {
                    var dx = antenna.Item1 - other.Item1;
                    var dy = antenna.Item2 - other.Item2;

                    var x = antenna.Item1;
                    var y = antenna.Item2;
                    while (x >= 0 && y >= 0 && x < maxX && y < maxY)
                    {
                        antinodeLocations.Add(new Tuple<int, int>(x, y));
                        x += dx;
                        y += dy;
                    }
                }
            }

            Console.WriteLine(antinodeLocations.Count);
        }
    }
}
