using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snake_2D
{
    public class GridNode
    {
        public Vector2Int position;
        public bool HasObstackle;
        public GridNode(int x, int y)
        {
            position.x = x;
            position.y = y;
        }
        public bool ComparePos(List<Vector2Int> obstPositions, Vector2Int currentPos)
        {
            foreach (Vector2Int p in obstPositions)
            {
                if (p == currentPos)
                {
                    return true;
                }
            }
            return false;
        }
    }
}