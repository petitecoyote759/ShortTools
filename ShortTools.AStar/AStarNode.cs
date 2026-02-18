using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ShortTools.AStar
{
    internal class AStarNode
    {
        public int x;
        public int y;
        public float pathLength;
        public AStarNode? parent;


        public AStarNode(Vector2 inp, AStarNode? parent, float pathLength, Vector2 end, Func<int, int, float> GetTileHeuristic)
        {
            x = (int)inp.X; y = (int)inp.Y;
            this.pathLength = pathLength;
            this.parent = parent;
            GenerateHeuristic(end, GetTileHeuristic);
        }
        public AStarNode(int x, int y, AStarNode? parent, float pathLength, Vector2 end, Func<int, int, float> GetTileHeuristic)
        {
            this.x = x;
            this.y = y;
            this.parent = parent;
            this.pathLength = pathLength;
            GenerateHeuristic(end, GetTileHeuristic);
        }


        public float heuristic;

        private float GenerateHeuristic(Vector2 end, Func<int, int, float> GetTileHeuristic)
        {
            return (end - new Vector2(x, y)).Length();
        }
    }
}
