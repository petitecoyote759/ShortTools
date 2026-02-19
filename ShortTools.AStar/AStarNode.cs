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


        public AStarNode(int inpX, int inpY, AStarNode? parent, float pathLength, int endX, int endY, Func<int, int, float> GetTileHeuristic, bool diagonals)
        {
            x = inpX; y = inpY;
            this.pathLength = pathLength;
            this.parent = parent;
            GenerateHeuristic(endX, endY, GetTileHeuristic, diagonals);
        }


        public float heuristic;

        private void GenerateHeuristic(int endX, int endY, Func<int, int, float> GetTileHeuristic, bool diagonals)
        {
            if (diagonals)
            {
                int dx = Math.Abs(endX - x);
                int dy = Math.Abs(endY - y);
                heuristic = Math.Max(dx, dy);
            }
            else
            {
                heuristic = Math.Abs(endX - x) + Math.Abs(endY - y);
            }
        }
    }
}
