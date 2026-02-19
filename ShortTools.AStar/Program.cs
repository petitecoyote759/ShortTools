using System.Diagnostics;
using System.Numerics;
using System.Security.Cryptography;
using System.Xml.Linq;
using ShortTools.AStar;




namespace ShortTools.AStar
{
    /// <summary>
    /// A 2d pathfinding algorithm designed for grid systems.
    /// </summary>
    public class PathFinder
    {

        Func<int, int, bool> Walkable;
        Func<int, int, float> GetTileHeuristic;


        Dictionary<(int x, int y), AStarNode> visitedNodes;
        PriorityQueue<AStarNode, float> toVisitNodes;
        public bool useDiagonals { private get; set; }
        public int maxDist { private get; set; }


        public PathFinder(Func<int, int, bool> Walkable, Func<int, int, float> GetTileHeuristic, int maxDist = 30, bool useDiagonals = true)
        {
            this.Walkable = Walkable;
            this.GetTileHeuristic = GetTileHeuristic; //TODO: use this to prioritise paths
            this.useDiagonals = useDiagonals;
            this.maxDist = maxDist;
        }





        private void UpdateTile(int x, int y, float pathLength, AStarNode parent, int endX, int endY, int startX, int startY)
        {
            if (Math.Abs(x - startX) > maxDist || Math.Abs(y - startY) > maxDist) { return; }
            if (!Walkable(x, y)) { return; }


            AStarNode? foundNode = visitedNodes.TryGetValue((x, y), out AStarNode? value) ? value : null;
            //foreach (AStarNode node in visitedNodes)
            //{
            //    if (node.x == x && node.y == y)
            //    {
            //        if (node.pathLength <= pathLength) { return; }
            //
            //        foundNode = node;
            //        break;
            //    }
            //}
            if (foundNode is not null)
            {
                if (foundNode.pathLength > pathLength)
                {
                    foundNode.pathLength = pathLength;
                    foundNode.parent = parent;
                }
            }
            else
            {
                AStarNode newNode = new AStarNode(x, y, parent, pathLength, endX, endY, GetTileHeuristic, useDiagonals);

                visitedNodes[(x, y)] = newNode;
                toVisitNodes.Enqueue(newNode, newNode.pathLength + newNode.heuristic);
            }


        }








        public Queue<Vector2> GetPath(int startX, int startY, int endX, int endY)
        {
            AStarNode startNode = new AStarNode(startX, startY, null, 0f, endX, endY, GetTileHeuristic, useDiagonals);

            visitedNodes = new Dictionary<(int x, int y), AStarNode>
            {
                { (startX, startY), startNode }
            };
            toVisitNodes = new PriorityQueue<AStarNode, float>();
            toVisitNodes.Enqueue(startNode, 0);

            while (toVisitNodes.Count > 0)
            {
                AStarNode? smallestNode = null;

                smallestNode = toVisitNodes.Dequeue();
                if (smallestNode?.x == endX && smallestNode?.y == endY)
                {
                    AStarNode? node = smallestNode;
                    Stack<Vector2> stack = new();
                    while (node != null)
                    {
                        stack.Push(new Vector2(node.x, node.y));
                        node = node.parent;
                    }
                    return new Queue<Vector2>(stack);
                }



                UpdateTile(smallestNode.x + 1, smallestNode.y, smallestNode.pathLength + (1 * GetTileHeuristic(smallestNode.x + 1, smallestNode.y)), smallestNode, endX, endY, startX, endY);
                UpdateTile(smallestNode.x - 1, smallestNode.y, smallestNode.pathLength + (1 * GetTileHeuristic(smallestNode.x - 1, smallestNode.y)), smallestNode, endX, endY, startX, endY);
                UpdateTile(smallestNode.x, smallestNode.y + 1, smallestNode.pathLength + (1 * GetTileHeuristic(smallestNode.x, smallestNode.y + 1)), smallestNode, endX, endY, startX, endY);
                UpdateTile(smallestNode.x, smallestNode.y - 1, smallestNode.pathLength + (1 * GetTileHeuristic(smallestNode.x, smallestNode.y - 1)), smallestNode, endX, endY, startX, endY);

                if (useDiagonals)
                {
                    UpdateTile(smallestNode.x + 1, smallestNode.y + 1, smallestNode.pathLength + (1.41f * GetTileHeuristic(smallestNode.x + 1, smallestNode.y)), smallestNode, endX, endY, startX, startY);
                    UpdateTile(smallestNode.x - 1, smallestNode.y + 1, smallestNode.pathLength + (1.41f * GetTileHeuristic(smallestNode.x - 1, smallestNode.y)), smallestNode, endX, endY, startX, startY);
                    UpdateTile(smallestNode.x + 1, smallestNode.y - 1, smallestNode.pathLength + (1.41f * GetTileHeuristic(smallestNode.x, smallestNode.y + 1)), smallestNode, endX, endY, startX, startY);
                    UpdateTile(smallestNode.x - 1, smallestNode.y - 1, smallestNode.pathLength + (1.41f * GetTileHeuristic(smallestNode.x, smallestNode.y - 1)), smallestNode, endX, endY, startX, startY);
                }


                //toVisitNodes.Remove(smallestNode); // no need to remove it anymore, dequeue does that
            }

            return new Queue<Vector2>();
        }
















    }




    internal class Tester
    {
        // <<Program Settings>> //
        const bool printMap = true;




        // <<Main Code>> //
        private static void Main()
        {
            char[][] map = new char[][]
            {
                new char[] { ' ', 'X', ' ', ' ', ' ', ' ', 'X', ' ', ' ', ' ' },
                new char[] { ' ', 'X', ' ', ' ', ' ', 'X', ' ', ' ', ' ', ' ' },
                new char[] { ' ', 'X', ' ', ' ', ' ', 'X', ' ', 'X', ' ', ' ' },
                new char[] { ' ', 'X', 'X', ' ', ' ', 'X', ' ', 'X', ' ', ' ' },
                new char[] { ' ', ' ', ' ', ' ', 'X', ' ', ' ', ' ', 'X', ' ' },
                new char[] { 'X', 'X', 'X', ' ', 'X', ' ', ' ', ' ', 'X', ' ' },
                new char[] { ' ', ' ', ' ', ' ', 'X', ' ', 'X', ' ', 'X', ' ' },
                new char[] { ' ', ' ', ' ', ' ', 'X', ' ', ' ', 'X', 'X', ' ' },
                new char[] { ' ', ' ', ' ', 'X', ' ', ' ', 'X', ' ', ' ', ' ' },
                new char[] { ' ', ' ', ' ', ' ', ' ', ' ', 'X', ' ', ' ', ' ' },
            };

            const int width = 50;
            const int height = 50;

            map = GenerateMap(width, height);


            PathFinder pather = new PathFinder(
                (x, y) => Walkable(x, y, map),
                (x, y) => 1,
                useDiagonals: true,
                maxDist: 500
                );

            if (printMap)
            {
                Console.Write(' ');
                PrintMap((character) => Console.Write($" {character} "), map);
            }

            Stopwatch watch = new Stopwatch();
            const long itterations = 10000;
            Queue<Vector2>? path = null;

            watch.Start();
            for (long i = 0; i < itterations; i++)
            {
                path = pather.GetPath(0, 0, width - 1, height - 1);
            }
            watch.Stop();

            Console.WriteLine((watch.ElapsedTicks / (float)Stopwatch.Frequency) / itterations);


            if (path is null) { return; }
            int count = 0;
            foreach (Vector2 node in path)
            {
                map[(int)node.X][(int)node.Y] = count.ToString()?.Last() ?? '!';
                count++;
            }

            if (printMap)
            {
                Console.Write("\n\n\n ");
                PrintMap((character) => Console.Write($" {character} "), map);
            }
        }

        private static bool Walkable(int x, int y, char[][] map)
        {
            if (0 <= x && x < map.Length)
            {
                char[] column = map[x];
                if (0 <= y && y < column.Length)
                {
                    return map[x][y] == ' ';
                }
            }
            return false;
        }

        private static void PrintMap(Action<char> Print, char[][] map)
        {
            int length = map.Length;
            if (length == 0) { Print('['); Print(']'); }
            int height = map[0].Length;


            for (int x = 0; x < length; x++)
            {
                Print('[');
                for (int y = 0; y < height; y++)
                {
                    Print(map[x][y]);
                }
                Print(']');
                Print('\n'); // new line each time, want this every time to ensure it ends on a newline
            }
        }

        private static char[][] GenerateMap(int width, int height)
        {
            char[][] map = new char[width][];

            for (int x = 0; x < width; x++)
            {
                map[x] = new char[height];
                for (int y = 0; y < height; y++)
                {
                    map[x][y] = RandomNumberGenerator.GetInt32(0, 3) != 0 ? ' ' : 'X'; // 25% chance for 'X', 75% chance for ' '
                }
            }

            map[0][0] = ' ';
            map[width - 1][height - 1] = ' ';

            return map;
        }
    }
}

