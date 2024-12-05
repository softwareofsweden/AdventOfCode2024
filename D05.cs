namespace aoc2024.Solutions
{
    internal static class D05
    {
        internal static string[] GetPageOrderingRules(string[] lines)
        {
            var rules = new List<string>();
            for (int i = 0; i < lines.Length; i++)
                if (lines[i] == "")
                    return [.. rules];
                else
                    rules.Add(lines[i]);
            return [.. rules];
        }

        internal static string[] GetPagesToProduceList(string[] lines)
        {
            var list = new List<string>();
            for (int i = 0; i < lines.Length; i++)
                if (lines[i].Contains(','))
                    list.Add(lines[i]);
            return [.. list];
        }

        internal static bool CanPageBeProducedAfter(string page, string otherPage, string[] pageOrderingRules)
        {
            for (int i = 0; i < pageOrderingRules.Length; i++)
            {
                var rule = pageOrderingRules[i].Split('|');

                if (rule[0] == page && rule[1] == otherPage)
                    return false;
            }

            return true;
        }

        internal static bool CanPageBeProducedBefore(string page, string otherPage, string[] pageOrderingRules)
        {
            for (int i = 0; i < pageOrderingRules.Length; i++)
            {
                var rule = pageOrderingRules[i].Split('|');

                if (rule[1] == page && rule[0] == otherPage)
                    return false;
            }

            return true;
        }

        internal static bool CanProduce(string pagesToProduce, string[] pageOrderingRules)
        {
            var pages = pagesToProduce.Split(',');

            for (int i = 0; i < pages.Length; i++)
            {
                for (int j = 0; j < i - 1; j++)
                    if (!CanPageBeProducedAfter(pages[i], pages[j], pageOrderingRules))
                        return false;
                for (int j = i + 1; j < pages.Length; j++)
                    if (!CanPageBeProducedBefore(pages[i], pages[j], pageOrderingRules))
                        return false;
            }

            return true;
        }

        internal static int GetMiddlePageNumber(string pagesToProduce)
        {
            var pages = pagesToProduce.Split(',');
            var middle = (int)pages.Length / 2;
            return int.Parse(pages[middle]);
        }

        internal static string OrderPages(string pagesToProduce, string[] pageOrderingRules)
        {
            var pages = pagesToProduce.Split(',').ToList();

            pages.Sort((p1, p2) =>
            {
                if (!CanPageBeProducedAfter(p1, p2, pageOrderingRules))
                    return -1;
                if (!CanPageBeProducedBefore(p1, p2, pageOrderingRules))
                    return 1;
                return 0;
            });

            return string.Join(',', pages);
        }

        /// <summary>
        /// You will need to find the middle page number of 
        /// each correctly-ordered update and add these page 
        /// numbers together
        /// </summary>
        internal static void Solve1()
        {
            var lines = File.ReadAllLines("Data\\d05.txt");

            var total = 0;

            var pageOrderingRules = GetPageOrderingRules(lines);
            var pagesToProduceList = GetPagesToProduceList(lines);

            foreach (var pagesToProduce in pagesToProduceList)
                if (CanProduce(pagesToProduce, pageOrderingRules))
                    total += GetMiddlePageNumber(pagesToProduce);

            Console.WriteLine(total);
        }

        /// <summary>
        /// Find the updates which are not in the correct order.
        /// What do you get if you add up the middle page numbers
        /// after correctly ordering just those updates?
        /// </summary>
        internal static void Solve2()
        {
            var lines = File.ReadAllLines("Data\\d05.txt");

            var total = 0;

            var pageOrderingRules = GetPageOrderingRules(lines);
            var pagesToProduceList = GetPagesToProduceList(lines);

            foreach (var pagesToProduce in pagesToProduceList)
                if (!CanProduce(pagesToProduce, pageOrderingRules))
                {
                    var orderedPages = OrderPages(pagesToProduce, pageOrderingRules);
                    total += GetMiddlePageNumber(orderedPages);
                }

            Console.WriteLine(total);
        }
    }
}
