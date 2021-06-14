using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snake_2D
{
    public class GameHandler : MonoBehaviour
    {
        [SerializeField]
        private SnakeControl Snake;
        private LevelGrid LevelGrid;
        private void Start()
        {
            LevelGrid = new LevelGrid(20, 20);
            Snake.InitGrid(LevelGrid);
            LevelGrid.InitSnake(Snake);
        }
    }
}
