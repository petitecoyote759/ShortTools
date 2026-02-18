using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortTools.AStar
{
    internal class QuadTree
    {
        private QuadTreeNode? root;

        public AStarNode? GetValue(int x, int y)
        {
            if (root is null) { return null; }
            return root.GetValue(x, y);
        }

        public void AddValue(AStarNode node)
        {
            if (root is null)
            {
                root = new QuadTreeNode(node);
            }
            else
            {
                root.AddValue(node);
            }
        }

        public QuadTree(AStarNode node)
        {
            root = new QuadTreeNode(node);
        }
    }


    internal class QuadTreeNode
    {
        public AStarNode value;

        public QuadTreeNode? NE; //                    N (y-)                       
        public QuadTreeNode? NW; //               W(x-)       E(x+)               
        public QuadTreeNode? SE; //                    S (y+)                       
        public QuadTreeNode? SW; //


        public QuadTreeNode(AStarNode node)
        {
            value = node;
            NE = null; NW = null;
            SE = null; SW = null;
        }




        public AStarNode? GetValue(int x, int y)
        {
            if (value.x == x && value.y == y) { return value; }

            if (value.x < x) // east
            {
                if (value.y < y) // south
                {
                    if (SE is null) { return null; }
                    return SE.GetValue(x, y);
                }
                else // north
                {
                    if (NE is null) { return null; }
                    return NE.GetValue(x, y);
                }
            }
            else // west
            {
                if (value.y < y) // south
                {
                    if (SW is null) { return null; }
                    return SW.GetValue(x, y);
                }
                else // north
                {
                    if (NW is null) { return null; }
                    return NW.GetValue(x, y);
                }
            }
        }







        public void AddValue(AStarNode node)
        {
            if (value.x == node.x && value.y == node.y) { value = node; return; }


            if (value.x < node.x) // east
            {
                if (value.y < node.y) // south
                {
                    if (SE is null) { SE = new QuadTreeNode(node); return; }
                    SE.AddValue(node);
                }
                else // north
                {
                    if (NE is null) { NE = new QuadTreeNode(node); return; }
                    NE.AddValue(node);
                }
            }
            else // west
            {
                if (value.y < node.y) // south
                {
                    if (SW is null) { SW = new QuadTreeNode(node); return; }
                    SW.AddValue(node);
                }
                else // north
                {
                    if (NW is null) { NW = new QuadTreeNode(node); return; }
                    NW.AddValue(node);
                }
            }
        }
    }
}
