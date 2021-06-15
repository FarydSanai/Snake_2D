using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Snake_2D
{
    public class LevelGrid
    {
        private Vector2Int FruitPos;
        private GameObject FruitGameObj;
        private SnakeControl Snake;
        public GridNode[,] NodeArr;

        private int _width;
        private int _height;
        public LevelGrid(int width, int height)
        {
            _width = width;
            _height = height;

            NodeArr = new GridNode[_width + 1, _height + 1];

            for (int x = 0; x < _width + 1; x++)
            {
                for (int y = 0; y < _height + 1; y++)
                {
                    NodeArr[x, y] = new GridNode(x,y);
                }
            }
        }
        public void GenerateObstackles()
        {
            int rand = Random.Range(3, 6);
            List<Vector2Int> prevObsPos = new List<Vector2Int>();

            for (int i = 0; i < rand; i++)
            {
                int randX;
                int randY;
                do
                {
                    randX = Random.Range(2, _width - 1);
                    randY = Random.Range(2, _height - 1);
                } while ((randX < 13 && randX > 7) &&
                         (randY < 13 && randY > 7) &&
                         NodeArr[randX, randY].ComparePos(prevObsPos, NodeArr[randX, randY].position));

                SetObstackle(randX, randY);
                NodeArr[randX, randY].HasObstackle = true;
                prevObsPos.Add(NodeArr[randX, randY].position);
            }
        }
        private void SetObstackle(int x, int y)
        {
            GameObject obst = new GameObject("Obst", typeof(SpriteRenderer));
            obst.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.ObstackleSprite;
            obst.transform.position = new Vector3(x, y);
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
            while (Snake.GetAllSnakePartsPos().IndexOf(FruitPos) != -1 &&
                   NodeArr[FruitPos.x, FruitPos.y].HasObstackle);

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
        public Vector2Int GetSize()
        {
            return new Vector2Int(_width, _height);
        }
    }
}
