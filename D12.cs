namespace aoc2024.Solutions
{
    internal static class D12
    {
        private class Tile
        {
            public char Value { get; set; }

            public int RegionId { get; set; } = 0;

            public int X { get; set; }

            public int Y { get; set; }

            public Tile(char value, int x, int y)
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

            private int _nextRegionId = 0;

            public Map(string fileName)
            {
                var lines = File.ReadAllLines(fileName);

                _data = new List<Tile>();

                Height = lines.Length;
                Width = lines[0].Length;

                for (int y = 0; y < Height; y++)
                    for (int x = 0; x < Width; x++)
                        _data.Add(new Tile(lines[y][x], x, y));
            }

            public bool IsWithinBounds(int x, int y)
            {
                return x >= 0 && y >= 0 && x < Height && y < Width;
            }

            public Tile GetTile(int x, int y)
            {
                int index = y * Height + x;
                return _data[index];
            }

            public void PrintToConsole()
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                        Console.Write(GetTile(x, y).RegionId);
                    Console.WriteLine();
                }
            }

            private void AddAdjacentTiles(Region region, int x, int y)
            {
                var thisTile = region.Tiles.First(t => t.X == x && t.Y == y);

                var tiles = _data.Where(t => t.RegionId == 0 && t.Value == thisTile.Value &&
                    (t.X == x - 1 && t.Y == y || t.X == x + 1 && t.Y == y || t.X == x && t.Y == y - 1 || t.X == x && t.Y == y + 1)
                );

                foreach (var tile in tiles)
                {
                    tile.RegionId = region.RegionId;
                    region.Tiles.Add(tile);
                    AddAdjacentTiles(region, tile.X, tile.Y);
                }
            }

            public Region ExtractRegion(int x, int y)
            {
                _nextRegionId++;

                var region = new Region(_nextRegionId);
                GetTile(x, y).RegionId = _nextRegionId;
                region.Tiles.Add(GetTile(x, y));

                AddAdjacentTiles(region, x, y);

                return region;
            }
        }

        private class Region
        {
            public List<Tile> Tiles = new List<Tile>();

            public int RegionId;

            public Region(int id)
            {
                RegionId = id;
            }

            public int GetArea()
            {
                return Tiles.Count;
            }

            public int GetPerimeter()
            {
                int perimeter = 0;
                foreach (var tile in Tiles)
                {
                    int x = tile.X;
                    int y = tile.Y;
                    var adjacent = Tiles.Where(t =>
                    t.X == x - 1 && t.Y == y || t.X == x + 1 && t.Y == y || t.X == x && t.Y == y - 1 || t.X == x && t.Y == y + 1
                    );

                    int adjacentCount = adjacent.Count();
                    perimeter += 4 - adjacentCount;
                }
                return perimeter;
            }

            public bool HaveTileAt(int x, int y)
            {
                return Tiles.Exists(t => t.X == x && t.Y == y);
            }

            public int CountGroups(string s)
            {
                int groupCount = 0;
                bool inGroup = false;
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == 'X' && !inGroup)
                    {
                        groupCount++;
                        inGroup = true;
                    }
                    else if (s[i] == ' ' && inGroup)
                    {
                        inGroup = false;
                    }
                }
                return groupCount;
            }

            public int GetSides()
            {
                int minX = Tiles.Min(t => t.X);
                int maxX = Tiles.Max(t => t.X);
                int minY = Tiles.Min(t => t.Y);
                int maxY = Tiles.Max(t => t.Y);

                int sides = 0;
                for (int y = minY; y <= maxY; y++)
                {
                    string above = "";
                    string below = "";
                    for (int x = minX; x <= maxX; x++)
                    {
                        above += (HaveTileAt(x, y) && !HaveTileAt(x, y - 1)) ? "X" : " ";
                        below += (HaveTileAt(x, y) && !HaveTileAt(x, y + 1)) ? "X" : " ";
                    }
                    sides += CountGroups(above);
                    sides += CountGroups(below);
                }
                for (int x = minX; x <= maxX; x++)
                {
                    string left = "";
                    string right = "";
                    for (int y = minY; y <= maxY; y++)
                    {
                        left += (HaveTileAt(x, y) && !HaveTileAt(x - 1, y)) ? "X" : " ";
                        right += (HaveTileAt(x, y) && !HaveTileAt(x + 1, y)) ? "X" : " ";
                    }
                    sides += CountGroups(left);
                    sides += CountGroups(right);
                }
                return sides;
            }

            public int GetPrice()
            {
                return GetArea() * GetPerimeter();
            }

            public int GetAlternatePrice()
            {
                return GetArea() * GetSides();
            }
        }

        /// <summary>
        /// What is the total price of fencing all regions on your map?
        /// </summary>
        internal static void Solve1()
        {
            var map = new Map("Data\\d12.txt");

            List<Region> regions = [];
            for (int y = 0; y < map.Height; y++)
                for (int x = 0; x < map.Width; x++)
                    if (map.GetTile(x, y).RegionId == 0)
                        regions.Add(map.ExtractRegion(x, y));

            int totalPrice = regions.Sum(x => x.GetPrice());

            Console.WriteLine("1533024");
            Console.WriteLine(totalPrice);
        }

        /// <summary>
        /// What is the total price of fencing all regions on your map?
        /// </summary>
        internal static void Solve2()
        {
            var map = new Map("Data\\d12.txt");

            List<Region> regions = [];
            for (int y = 0; y < map.Height; y++)
                for (int x = 0; x < map.Width; x++)
                    if (map.GetTile(x, y).RegionId == 0)
                        regions.Add(map.ExtractRegion(x, y));

            int totalPrice = regions.Sum(x => x.GetAlternatePrice());

            Console.WriteLine("910066");
            Console.WriteLine(totalPrice);
        }
    }
}
