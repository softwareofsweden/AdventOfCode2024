namespace aoc2024.Solutions
{
    internal static class D04
    {
        internal static char GetChar(string[] lines, int x, int y)
        {
            if (x < 0)
                return ' ';
            if (x >= lines[0].Length)
                return ' ';
            if (y < 0)
                return ' ';
            if (y >= lines.Length)
                return ' ';
            return lines[y][x];
        }

        internal static int SearchD(string[] lines, int x, int y, int dx, int dy, string text)
        {
            var word = "";

            for (int i = 0; i < text.Length; i++)
                word += GetChar(lines, x + (dx * i), y + (dy * i));

            if (word == text)
                return 1;

            return 0;
        }

        internal static int Search(string[] lines, int x, int y, string text)
        {
            var count = 0;

            for (int dx = -1; dx < 2; dx++)
                for (int dy = -1; dy < 2; dy++)
                    if (dx != 0 || dy != 0)
                        count += SearchD(lines, x, y, dx, dy, text);

            return count;
        }

        /// <summary>
        /// How many times does XMAS appear?
        /// </summary>
        internal static void Solve1()
        {
            var lines = File.ReadAllLines("Data\\d04.txt");
            var width = lines[0].Length;
            var height = lines.Length;

            var nbrXmas = 0;

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    if (GetChar(lines, x, y) == 'X')
                        nbrXmas += Search(lines, x, y, "XMAS");

            Console.WriteLine(nbrXmas);
        }

        /// <summary>
        /// How many times does this appear?
        /// 
        /// M.S
        /// .A.
        /// M.S
        /// 
        /// </summary>
        internal static void Solve2()
        {
            var lines = File.ReadAllLines("Data\\d04.txt");
            var width = lines[0].Length;
            var height = lines.Length;

            var nbrXmas = 0;

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    if (GetChar(lines, x, y) == 'A')
                        if (SearchD(lines, x - 1, y - 1, 1, 1, "MAS") +
                            SearchD(lines, x - 1, y - 1, 1, 1, "SAM") +
                            SearchD(lines, x - 1, y + 1, 1, -1, "MAS") +
                            SearchD(lines, x - 1, y + 1, 1, -1, "SAM") == 2
                            )
                            nbrXmas++;

            Console.WriteLine(nbrXmas);
        }
    }
}
