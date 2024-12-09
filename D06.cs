namespace aoc2024.Solutions
{
    internal static class D06
    {
        private class GuardSimulation
        {
            private int guardX;
            private int guardY;
            private int guardDx;
            private int guardDy;

            private readonly List<string> map;
            private readonly int mapW;
            private readonly int mapH;

            private readonly Dictionary<int, List<string>> directionsAtLocation;

            public GuardSimulation(string[] data)
            {
                directionsAtLocation = new Dictionary<int, List<string>>();
                SetGuardDirection(0, -1); // Moving up

                map = new List<string>();
                mapW = data[0].Length;
                mapH = data.Length;

                for (int y = 0; y < mapH; y++)
                {
                    for (int x = 0; x < mapW; x++)
                    {
                        if (data[y][x] == '^')
                        {
                            SetGuardPosition(x, y);
                            map.Add(" ");
                        }
                        else
                        {
                            map.Add(data[y][x] == '#' ? "#" : " ");
                        }
                    }
                }
            }

            private string GetGuardDirection()
            {
                if (guardDx == 0 && guardDy == -1)
                {
                    return "Up";
                }
                else if (guardDx == 1 && guardDy == 0)
                {
                    return "Right";
                }
                else if (guardDx == 0 && guardDy == 1)
                {
                    return "Down";
                }
                else
                {
                    return "Left";
                }
            }

            private void SaveDirectionAtLocation()
            {
                int location = guardY * mapW + guardX;
                var guardDirection = GetGuardDirection();

                if (directionsAtLocation.TryGetValue(location, out var directions))
                {
                    if (!directions.Contains(guardDirection))
                        directions.Add(guardDirection);
                    directionsAtLocation[location] = directions;
                }
                else
                {
                    var newDirections = new List<string>();
                    directionsAtLocation[location] = newDirections;
                }
            }

            private bool HasGuardBeenHereBefore()
            {
                int location = guardY * mapW + guardX;
                var guardDirection = GetGuardDirection();
                if (directionsAtLocation.TryGetValue(location, out var directions))
                {
                    if (directions.Contains(guardDirection))
                        return true;
                }
                return false;
            }

            private void SetGuardDirection(int dx, int dy)
            {
                guardDx = dx;
                guardDy = dy;
            }

            private void SetGuardPosition(int x, int y)
            {
                guardX = x;
                guardY = y;
            }

            public string GetMapData(int x, int y)
            {
                int idx = y * mapW + x;
                return map[idx];
            }

            public void PutMapData(int x, int y, string v)
            {
                int idx = y * mapW + x;
                map[idx] = v;
            }

            public int GetNumberVisitedLocations()
            {
                int count = 0;
                for (int y = 0; y < mapH; y++)
                    for (int x = 0; x < mapW; x++)
                        if (GetMapData(x, y) == "V")
                            count++;
                return count;
            }

            public void PrintMap()
            {
                for (int y = 0; y < mapH; y++)
                {
                    for (int x = 0; x < mapW; x++)
                    {
                        if (guardX == x && guardY == y)
                            Console.Write("G");
                        else
                            Console.Write(GetMapData(x, y));
                    }
                    Console.Write('\n');
                }
            }

            public bool IsInsideMap(int x, int y)
            {
                return x >= 0 && y >= 0 && x < mapW && y < mapH;
            }

            public bool IsObsticle(int x, int y)
            {
                var data = GetMapData(x, y);

                return data == "#" || data == "T";
            }

            public bool CanGuardMoveForward()
            {
                int newX = guardX + guardDx;
                int newY = guardY + guardDy;
                if (!IsInsideMap(newX, newY))
                {
                    return true;
                }
                return !IsObsticle(newX, newY);
            }

            public bool IsGuardOutsideMap()
            {
                return !IsInsideMap(guardX, guardY);
            }

            public void MoveGuard()
            {
                guardX += guardDx;
                guardY += guardDy;
                if (!IsGuardOutsideMap())
                    PutMapData(guardX, guardY, "V");
            }

            public void TurnGuardRight()
            {
                if (guardDx == 0 && guardDy == -1)
                {
                    guardDx = 1;
                    guardDy = 0;
                }
                else if (guardDx == 1 && guardDy == 0)
                {
                    guardDx = 0;
                    guardDy = 1;
                }
                else if (guardDx == 0 && guardDy == 1)
                {
                    guardDx = -1;
                    guardDy = 0;
                }
                else
                {
                    guardDx = 0;
                    guardDy = -1;
                }
            }

            public void Simulate()
            {
                while (true)
                {
                    if (CanGuardMoveForward())
                    {
                        MoveGuard();
                        if (IsGuardOutsideMap())
                        {
                            break;
                        }
                    }
                    else
                    {
                        TurnGuardRight();
                    }
                }
            }

            public int Simulate2()
            {
                var guardRoute = new List<Tuple<int, int>>();
                var startX = guardX;
                var startY = guardY;

                while (true)
                {
                    if (CanGuardMoveForward())
                    {
                        MoveGuard();
                        if (IsGuardOutsideMap())
                        {
                            break;
                        }
                        if (guardX != startX || guardY != startY)
                        {
                            var pos = new Tuple<int, int>(guardX, guardY);
                            if (!guardRoute.Contains(pos))
                                guardRoute.Add(pos);
                        }
                    }
                    else
                    {
                        TurnGuardRight();
                    }
                }

                var loopCount = 0;

                foreach (var pos in guardRoute)
                {
                    // Reset guard pos
                    guardX = startX;
                    guardY = startY;

                    // Reset guard direction
                    SetGuardDirection(0, -1);

                    // Reset map
                    for (int y = 0; y < mapH; y++)
                        for (int x = 0; x < mapW; x++)
                        {
                            if (GetMapData(x, y) == "V")
                                PutMapData(x, y, " ");
                            if (GetMapData(x, y) == "T")
                                PutMapData(x, y, " ");
                        }

                    // Put new obsticle
                    PutMapData(pos.Item1, pos.Item2, "T");

                    // Clear history for directions
                    directionsAtLocation.Clear();

                    // Check if guard ends up in a loop
                    var loop = false;
                    while (true)
                    {
                        if (CanGuardMoveForward())
                        {
                            MoveGuard();
                            if (IsGuardOutsideMap())
                            {
                                break;
                            }
                            if (HasGuardBeenHereBefore())
                            {
                                loop = true;
                                break;
                            }
                            SaveDirectionAtLocation();
                        }
                        else
                        {
                            TurnGuardRight();
                            SaveDirectionAtLocation();
                        }
                    }

                    if (loop)
                        loopCount++;
                }

                return loopCount;
            }
        }

        /// <summary>
        /// How many distinct positions will the guard visit 
        /// before leaving the mapped area?
        /// </summary>
        internal static void Solve1()
        {
            var lines = File.ReadAllLines("Data\\d06.txt");

            var sim = new GuardSimulation(lines);
            sim.Simulate();
            Console.WriteLine(sim.GetNumberVisitedLocations());
        }

        /// <summary>
        /// You need to get the guard stuck in a loop by adding a single new obstruction. 
        /// How many different positions could you choose for this obstruction?
        /// </summary>
        internal static void Solve2()
        {
            var lines = File.ReadAllLines("Data\\d06.txt");

            var sim = new GuardSimulation(lines);
            var count = sim.Simulate2();
            Console.WriteLine(count);
        }
    }
}
