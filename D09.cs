using System.Security.Cryptography;

namespace aoc2024.Solutions
{
    internal static class D09
    {
        internal static List<int> ExpandDiskmap(string diskMap)
        {
            var disk = new List<int>();
            var fileId = 0;
            for (int i = 0; i < diskMap.Length; i++)
                if (i % 2 == 0)
                {
                    fileId++;
                    for (int j = 0; j < int.Parse(diskMap[i].ToString()); j++)
                        disk.Add(fileId);
                }
                else
                {
                    for (int j = 0; j < int.Parse(diskMap[i].ToString()); j++)
                        disk.Add(0);
                }

            return disk;
        }

        internal static long CalculateChecksum(List<int> disk)
        {
            long sum = 0;
            for (int i = 0; i < disk.Count; i++)
                if (disk[i] != 0)
                    sum += ((disk[i] - 1) * i);
            return sum;
        }


        /// <summary>
        /// Compact the amphipod's hard drive using the process he requested. 
        /// What is the resulting filesystem checksum?
        /// </summary>
        internal static void Solve1()
        {
            var diskMap = File.ReadAllText("Data\\d09.txt");

            var disk = ExpandDiskmap(diskMap);

            int rdPos = disk.Count - 1;
            int wrPos = 0;

            while (true)
            {
                while (disk[rdPos] == 0)
                    rdPos--;
                while (disk[wrPos] != 0)
                    wrPos++;
                if (wrPos >= rdPos)
                    break;

                // Move
                disk[wrPos] = disk[rdPos];
                disk[rdPos] = 0;
            }

            long checksum = CalculateChecksum(disk);

            Console.WriteLine(checksum);
        }

        /// <summary>
        /// Start over, now compacting the amphipod's hard drive using 
        /// this new method instead. What is the resulting filesystem 
        /// checksum?
        /// </summary>
        internal static void Solve2()
        {
            var diskMap = File.ReadAllText("Data\\d09.txt");

            var disk = ExpandDiskmap(diskMap);

            var fileIds = disk.Distinct();

            var files = new Stack<Tuple<int, int>>();

            foreach (var fileId in fileIds)
            {
                var fileSize = disk.Count(x => x == fileId);
                files.Push(new Tuple<int, int>(fileId, fileSize));
            }

            while (files.TryPop(out Tuple<int,int>? file))
            {
                if (files.Count == 0) break;

                var fileId = file.Item1;
                var fileSize = file.Item2;

                var originalFileLocation = disk.IndexOf(fileId);

                // check if space available
                var destLocation = -1;
                for (var i = 0; i < originalFileLocation - 1; i++)
                {
                    bool spaceAvailable = true;
                    for (var j = i; j < i + fileSize; j++)
                        if (disk[j] != 0)
                        {
                            spaceAvailable = false;
                            break;
                        }
                    if (spaceAvailable)
                    {
                        destLocation = i;
                        break;
                    }
                }

                if (destLocation != -1)
                {
                    for (int i = originalFileLocation; i < originalFileLocation + fileSize; i++)
                        if (disk[i] == fileId)
                            disk[i] = 0;
                    for (int i = 0; i < fileSize; i++)
                        disk[destLocation+i] = fileId;
                }
            }

            long checksum = CalculateChecksum(disk);

            Console.WriteLine(checksum);
        }
    }
}
