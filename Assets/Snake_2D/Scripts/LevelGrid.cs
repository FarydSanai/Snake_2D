using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snake_2D
{
    public class LevelGrid
    {

        private Vector2Int FruitPos;
        private GameObject FruitGameObj;
        private SnakeControl Snake;

        private int _width;
        private int _height;
        public LevelGrid(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public void InitSnake(SnakeControl snake)
        {
            this.Snake = snake;

            SpawnFruit();
        }

        private void SpawnFruit()
        {
            do
            {
                FruitPos = new Vector2Int(Random.Range(1, _width), Random.Range(1, _height));
            }
            while (Snake.GetAllSnakePartsPos().IndexOf(FruitPos) != -1);

            FruitGameObj = new GameObject("Food", typeof(SpriteRenderer));
            FruitGameObj.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.FruitSprite;
            FruitGameObj.transform.position = new Vector3(FruitPos.x, FruitPos.y);
        }

        public bool SnakeOverlapFruit(Vector2Int snakeGridPosition)
        {
            if (snakeGridPosition == FruitPos)
            {
                Object.Destroy(FruitGameObj);
                SpawnFruit();
                return true;
            }
            return false;
        }
    }
}
