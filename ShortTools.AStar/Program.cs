using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Xml.Linq;
using ShortTools.AStar;


#pragma warning disable CS8618 // visitedNodes and toVisitNodes must have value on exit, they are used exclusively in function so thats okay.
#pragma warning disable IDE0003 // Name can be simplified (this.). Improves clarity of variable declarations.
#pragma warning disable CA2201 // Exception type not specific, this is because I would have no idea how that could happen, so no idea how to specify it.
#pragma warning disable IDE0290 // Use primary constructor. Looks better like this and allows for easier expansion for more constructors.
#pragma warning disable IDE0090 // 'new' expression can be simplified. This is far more clear and looks better with the new expression.



namespace ShortTools.AStar
{
    /// <summary>
    /// A 2d pathfinding algorithm designed for regular grid systems.
    /// </summary>
    public class PathFinder
    {

        private readonly Func<int, int, bool> Walkable;
        private readonly Func<int, int, float> GetTileHeuristic;

        // Initialised in the GetPath function
        Dictionary<(int x, int y), AStarNode> visitedNodes;
        PriorityQueue<AStarNode, float> toVisitNodes;


        /// <summary>
        /// Boolean representing if the algorithm should look at diagonal tiles for each step.
        /// </summary>
        public bool UseDiagonals { get; set; }
        /// <summary>
        /// Integer representing the maximum distance that this algorithm should search to.
        /// </summary>
        public int MaxDist { get; set; }




        private static float DefaultGetTileHeuristic(int x, int y) { return 1f; }




        /// <summary>
        /// Creates an instance of the PathFinder class which can create paths between 2 seperate points.
        /// </summary>
        /// <param name="Walkable">A function that decides if a tile is walkable, given the coordinates.</param>
        /// <param name="maxDist">A value that specifies the maximum distance that this pathfinder will go.</param>
        /// <param name="useDiagonals">A boolean representing for if this pathfinder will search through diagonals.</param>
        /// <param name="getTileHeuristic">A function that returns a tile heuristic for each tile.</param>
        public PathFinder(Func<int, int, bool> Walkable, Func<int, int, float>? getTileHeuristic = null, int maxDist = 30, bool useDiagonals = true)
        {
            GetTileHeuristic = getTileHeuristic ?? DefaultGetTileHeuristic;

            this.Walkable = Walkable;
            this.UseDiagonals = useDiagonals;
            this.MaxDist = maxDist;
        }





        /// <summary>
        /// Updates the tile heuristic and adds the tiles nearby to the toVisitNodes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private void UpdateTile(int x, int y, float pathLength, Func<float> Heuristic, AStarNode parent, int endX, int endY, int startX, int startY)
        {
            // if its too far away ignore it.
            if (Math.Abs(x - startX) > MaxDist || Math.Abs(y - startY) > MaxDist) { return; }
            // its its not walkable, we dont care about it.
            if (!Walkable(x, y)) { return; }


            // now that we know it is in bounds, as checked by Walkable, heuristic can be added to pathLength
            pathLength += Heuristic();


            // Gets the node at the given coords in O(1) time.
            AStarNode? foundNode = visitedNodes.TryGetValue((x, y), out AStarNode? value) ? value : null;

            // Update path lengths if found.
            if (foundNode is not null)
            {
                if (foundNode.pathLength > pathLength)
                {
                    foundNode.pathLength = pathLength;
                    foundNode.parent = parent;
                }
            }
            else
            { // If there isnt already a node, make one.
                AStarNode newNode = new AStarNode(x, y, parent, pathLength, endX, endY, UseDiagonals);

                visitedNodes[(x, y)] = newNode;
                toVisitNodes.Enqueue(newNode, newNode.pathLength + newNode.heuristic);
            }
        }







        /// <summary>
        /// Gets a path from the first coordinate to the second coordinate. Returns null if there is no path, or a queue containing the path if there was a path.
        /// </summary>
        /// <param name="startX">The X coordinate of the start position.</param>
        /// <param name="startY">The Y coordinate of the start position.</param>
        /// <param name="endX">The X coordinate of the end position.</param>
        /// <param name="endY">The Y coordinate of the end position.</param>
        /// <returns>A Queue containing the path, or null if there is no path found.</returns>
        public Queue<Vector2>? GetPath(int startX, int startY, int endX, int endY)
        {
            // <<Initial Variable Creation>> //
            AStarNode startNode = new AStarNode(startX, startY, null, 0f, endX, endY, UseDiagonals);

            visitedNodes = new Dictionary<(int x, int y), AStarNode>
            {
                { (startX, startY), startNode }
            };
            toVisitNodes = new PriorityQueue<AStarNode, float>();
            toVisitNodes.Enqueue(startNode, 0);


            // Run through nodes.
            while (toVisitNodes.Count > 0)
            {
                // Priority queue allows closest node to be dequeued.
                AStarNode smallestNode = toVisitNodes.Dequeue();

                if (smallestNode?.x == endX && smallestNode?.y == endY)
                {
                    // Found the target, return the path to get there.
                    AStarNode? node = smallestNode;
                    Stack<Vector2> stack = new();
                    while (node != null)
                    {
                        stack.Push(new Vector2(node.x, node.y));
                        node = node.parent;
                    }
                    return new Queue<Vector2>(stack);
                }


                // This should absolutely never happen, but who knows.
                if (smallestNode is null) { throw new Exception("An unknown exception occured, please contact author with how to recreate."); } 


                // Update connected tiles.
                UpdateTile(smallestNode.x + 1, smallestNode.y, smallestNode.pathLength, () => (1 * GetTileHeuristic(smallestNode.x + 1, smallestNode.y)), smallestNode, endX, endY, startX, endY);
                UpdateTile(smallestNode.x - 1, smallestNode.y, smallestNode.pathLength, () => (1 * GetTileHeuristic(smallestNode.x - 1, smallestNode.y)), smallestNode, endX, endY, startX, endY);
                UpdateTile(smallestNode.x, smallestNode.y + 1, smallestNode.pathLength, () => (1 * GetTileHeuristic(smallestNode.x, smallestNode.y + 1)), smallestNode, endX, endY, startX, endY);
                UpdateTile(smallestNode.x, smallestNode.y - 1, smallestNode.pathLength, () => (1 * GetTileHeuristic(smallestNode.x, smallestNode.y - 1)), smallestNode, endX, endY, startX, endY);

                if (UseDiagonals)
                {
                    // Update diagonal tiles.
                    UpdateTile(smallestNode.x + 1, smallestNode.y + 1, smallestNode.pathLength, () => (1.41f * GetTileHeuristic(smallestNode.x + 1, smallestNode.y + 1)), smallestNode, endX, endY, startX, startY);
                    UpdateTile(smallestNode.x - 1, smallestNode.y + 1, smallestNode.pathLength, () => (1.41f * GetTileHeuristic(smallestNode.x - 1, smallestNode.y + 1)), smallestNode, endX, endY, startX, startY);
                    UpdateTile(smallestNode.x + 1, smallestNode.y - 1, smallestNode.pathLength, () => (1.41f * GetTileHeuristic(smallestNode.x + 1, smallestNode.y - 1)), smallestNode, endX, endY, startX, startY);
                    UpdateTile(smallestNode.x - 1, smallestNode.y - 1, smallestNode.pathLength, () => (1.41f * GetTileHeuristic(smallestNode.x - 1, smallestNode.y - 1)), smallestNode, endX, endY, startX, startY);
                }
            }

            return null;
        }
    }









#pragma warning disable // All warnings after this point are irrelevant as this is testing code.
    internal class Tester
    {
        // <<Program Settings>> //
        const bool printMap = true;



        // <<Program Variables>> //

        static readonly Dictionary<char, float> heuristicValues = new Dictionary<char, float>()
        {
            { ' ', 1f },
            { '-', 0.5f }
        };



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
                getTileHeuristic: (x, y) => heuristicValues[map[x][y]],
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
                    return map[x][y] != 'X';
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
                    map[x][y] = RandomNumberGenerator.GetInt32(0, 3) != 0 ? ' ' : 'X'; // 33% chance for 'X', 66% chance for ' '
                    if (map[x][y] == ' ')
                    {
                        map[x][y] = RandomNumberGenerator.GetInt32(0, 3) != 0 ? ' ' : '-';
                    }
                }
            }

            map[0][0] = ' ';
            map[width - 1][height - 1] = ' ';

            return map;
        }
    }
}

