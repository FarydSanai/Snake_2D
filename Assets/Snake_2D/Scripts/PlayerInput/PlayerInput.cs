using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snake_2D
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField]
        private SnakeControl snakeControl;
        public void TurnLeft()
        {
            snakeControl.TurnRight = false;
            snakeControl.TurnLeft = true;
        }
        public void TurnRight()
        {
            snakeControl.TurnLeft = false;
            snakeControl.TurnRight = true;
        }
    }
}