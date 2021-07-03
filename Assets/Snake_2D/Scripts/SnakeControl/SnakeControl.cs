using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snake_2D
{
    public enum SnakeCollisionType
    {
        Wall,
        Self,
        Obstackle,

        None,
    }
    public class SnakeControl : MonoBehaviour
    {
        private enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }

        public bool TurnLeft;
        public bool TurnRight;
        private SnakeCollisionType CurrentCollision = SnakeCollisionType.None;

        private Direction MoveDir;
        private Vector2Int GridPos;
        private float SnakeMoveDelay;
        private float MaxDelay;
        private LevelGrid LevelGrid;
        private int SnakeSize;
        private List<SnakeMovePosition> snakeMovePositionList;
        private List<SnakeBodyPart> snakeBodyPartList;

        public void InitGrid(LevelGrid levelGrid)
        {
            this.LevelGrid = levelGrid;
        }

        private void Awake()
        {
            GridPos = new Vector2Int(10, 10);
            MaxDelay = 0.5f;
            SnakeMoveDelay = MaxDelay;
            MoveDir = Direction.Right;

            snakeMovePositionList = new List<SnakeMovePosition>();
            SnakeSize = 0;

            snakeBodyPartList = new List<SnakeBodyPart>();
        }
        private void Start()
        {
            InitFirstBodyPart();
        }
        private void Update()
        {
            CheckPlayerInput();
            SnakeGridMovement();
        }
        private void CheckPlayerInput()
        {
            //Inverted control
            if (TurnLeft)
            {
                switch (MoveDir)
                {
                    case Direction.Left:
                        {
                            MoveDir = Direction.Down;
                        }
                        break;
                    case Direction.Right:
                        {
                            MoveDir = Direction.Up;
                        }
                        break;
                    case Direction.Up:
                        {
                            MoveDir = Direction.Left;
                        }
                        break;
                    case Direction.Down:
                        {
                            MoveDir = Direction.Left;
                        }
                        break;
                    default:
                        break;
                }
                TurnLeft = false;
                TurnRight = false;
            }
            if (TurnRight)
            {
                switch (MoveDir)
                {
                    case Direction.Left:
                        {
                            MoveDir = Direction.Up;
                        }
                        break;
                    case Direction.Right:
                        {
                            MoveDir = Direction.Down;
                        }
                        break;
                    case Direction.Up:
                        {
                            MoveDir = Direction.Right;
                        }
                        break;
                    case Direction.Down:
                        {
                            MoveDir = Direction.Right;
                        }
                        break;
                    default:
                        break;
                }
                TurnLeft = false;
                TurnRight = false;
            }
        }
        private Vector2Int SetSnakeMoveDir()
        {
            Vector2Int dir = new Vector2Int(0,1);
            switch (MoveDir)
            {
                case Direction.Left:
                    {
                        dir = new Vector2Int(-1, 0);
                    }
                    break;
                case Direction.Right:
                    {
                        dir = new Vector2Int(1, 0);
                    }
                    break;
                case Direction.Up:
                    {
                        dir = new Vector2Int(0, 1);
                    }
                    break;
                case Direction.Down:
                    {
                        dir = new Vector2Int(0, -1);
                    }
                    break;
                default:
                    break;
            }
            return dir;
        }
        private void InitFirstBodyPart()
        {
            SnakeMovePosition prevMovePos = null;
            if (snakeMovePositionList.Count > 0)
            {
                prevMovePos = snakeMovePositionList[0];
            }

            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(prevMovePos, GridPos, MoveDir);
            snakeMovePositionList.Insert(0, snakeMovePosition);

            Vector2Int snakeMoveDir = SetSnakeMoveDir();
            GridPos += snakeMoveDir;
            SnakeSize++;
            CreateSnakeBodyPart();
            UpdateSnakeBodyParts();
        }
        private void SnakeGridMovement()
        {
            SnakeMoveDelay += Time.deltaTime;
            if (SnakeMoveDelay >= MaxDelay)
            {
                SnakeMoveDelay -= MaxDelay;

                SnakeMovePosition prevMovePos = null;
                if (snakeMovePositionList.Count > 0)
                {
                    prevMovePos = snakeMovePositionList[0];
                }

                SnakeMovePosition snakeMovePosition = new SnakeMovePosition(prevMovePos, GridPos, MoveDir);
                snakeMovePositionList.Insert(0, snakeMovePosition);

                Vector2Int snakeMoveDir = SetSnakeMoveDir();               

                GridPos += snakeMoveDir;

                bool snakeAteFood = LevelGrid.SnakeOverlapFruit(GridPos);
                if (snakeAteFood)
                {
                    SnakeSize++;
                    MaxDelay -= MaxDelay * 0.1f;
                    CreateSnakeBodyPart();
                    if (SnakeSize >= 10)
                    {
                        GameHandler.Instance.EndGame();
                        PopupWindow.Instance.SetWinPopup();
                    }
                }

                if (snakeMovePositionList.Count >= SnakeSize + 1)
                {
                    snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
                }

                UpdateSnakeBodyParts();

                if (CheckCollision())
                {
                    HandleCollision(CurrentCollision);
                }

                transform.position = new Vector3(GridPos.x, GridPos.y);
                transform.eulerAngles = new Vector3(0, 0, GetAngle(snakeMoveDir) - 90);
            }
        }
        private void HandleCollision(SnakeCollisionType collision)
        {
            switch (collision)
            {
                case SnakeCollisionType.Wall:
                    {
                        PopupWindow.Instance.SetLosePopup();
                        GameHandler.Instance.EndGame();
                    }
                    break;
                case SnakeCollisionType.Self:
                    {
                        PopupWindow.Instance.SetLosePopup();
                        GameHandler.Instance.EndGame();
                    }
                    break;
                case SnakeCollisionType.Obstackle:
                    {
                        GameHandler.Instance.StartNewGame();
                    }
                    break;
                case SnakeCollisionType.None:
                    break;
                default:
                    break;
            }
        }
        private bool CheckCollision()
        {
            if (GridPos.x >= LevelGrid.GetSize().x || GridPos.x <= 0 ||
                GridPos.y >= LevelGrid.GetSize().y || GridPos.y <= 0)
            {
                CurrentCollision = SnakeCollisionType.Wall;
                return true;
            }
            if (LevelGrid.NodeArr[GridPos.x, GridPos.y].HasObstackle)
            {
                CurrentCollision = SnakeCollisionType.Obstackle;
                return true;
            }
            foreach (SnakeBodyPart sbp in snakeBodyPartList)
            {
                if (GridPos == sbp.GetGridPos())
                {
                    CurrentCollision = SnakeCollisionType.Self;
                    return true;
                }
            }
            return false;
        }
        private void CreateSnakeBodyPart()
        {
            snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
        }

        private void UpdateSnakeBodyParts()
        {
            for (int i = 0; i < snakeBodyPartList.Count; i++)
            {
                snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);
            }
        }
        private float GetAngle(Vector2Int dir)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (angle < 0)
            {
                angle += 360;
            }
            return angle;
        }
        public Vector2Int GetGridPosition()
        {
            return GridPos;
        }
        public List<Vector2Int> GetAllSnakePartsPos()
        {
            List<Vector2Int> gridPositionList = new List<Vector2Int>() { GridPos };
            foreach (SnakeMovePosition snakeMovePosition in snakeMovePositionList)
            {
                gridPositionList.Add(snakeMovePosition.GetGridPosition());
            }
            return gridPositionList;
        }
        private class SnakeBodyPart
        {
            private SnakeMovePosition snakeMovePosition;
            private Transform transform;
            public SnakeBodyPart(int bodyIndex)
            {
                GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
                snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.SnakeBodySprite;
                snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -1 - bodyIndex;
                snakeBodyGameObject.GetComponent<Renderer>().material = GameAssets.Instance.SnakeMaterial;

                transform = snakeBodyGameObject.transform;
            }
            public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition)
            {
                this.snakeMovePosition = snakeMovePosition;

                transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);

                float angle;
                switch (snakeMovePosition.GetDirection())
                {
                    default:
                    case Direction.Up: // Currently going Up
                        switch (snakeMovePosition.GetPreviousDirection())
                        {
                            default:
                                angle = 0;
                                break;
                            case Direction.Left: // Previously was going Left
                                angle = 0 + 45;
                                transform.position += new Vector3(.2f, .2f);
                                break;
                            case Direction.Right: // Previously was going Right
                                angle = 0 - 45;
                                transform.position += new Vector3(-.2f, .2f);
                                break;
                        }
                        break;
                    case Direction.Down: // Currently going Down
                        switch (snakeMovePosition.GetPreviousDirection())
                        {
                            default:
                                angle = 180;
                                break;
                            case Direction.Left: // Previously was going Left
                                angle = 180 - 45;
                                transform.position += new Vector3(.2f, -.2f);
                                break;
                            case Direction.Right: // Previously was going Right
                                angle = 180 + 45;
                                transform.position += new Vector3(-.2f, -.2f);
                                break;
                        }
                        break;
                    case Direction.Left: // Currently going to the Left
                        switch (snakeMovePosition.GetPreviousDirection())
                        {
                            default:
                                angle = +90;
                                break;
                            case Direction.Down: // Previously was going Down
                                angle = 180 - 45;
                                transform.position += new Vector3(-.2f, .2f);
                                break;
                            case Direction.Up: // Previously was going Up
                                angle = 45;
                                transform.position += new Vector3(-.2f, -.2f);
                                break;
                        }
                        break;
                    case Direction.Right: // Currently going to the Right
                        switch (snakeMovePosition.GetPreviousDirection())
                        {
                            default:
                                angle = -90;
                                break;
                            case Direction.Down: // Previously was going Down
                                angle = 180 + 45;
                                transform.position += new Vector3(.2f, .2f);
                                break;
                            case Direction.Up: // Previously was going Up
                                angle = -45;
                                transform.position += new Vector3(.2f, -.2f);
                                break;
                        }
                        break;
                }

                transform.eulerAngles = new Vector3(0, 0, angle);
            }
            public Vector2Int GetGridPos()
            {
                return snakeMovePosition.GetGridPosition();
            }
        }
        private class SnakeMovePosition
        {
            private SnakeMovePosition previousSnakeMovePosition;
            private Vector2Int gridPosition;
            private Direction direction;
            public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector2Int gridPosition, Direction direction)
            {
                this.previousSnakeMovePosition = previousSnakeMovePosition;
                this.gridPosition = gridPosition;
                this.direction = direction;
            }
            public Vector2Int GetGridPosition()
            {
                return gridPosition;
            }
            public Direction GetDirection()
            {
                return direction;
            }
            public Direction GetPreviousDirection()
            {
                if (previousSnakeMovePosition == null)
                {
                    return Direction.Right;
                }
                else
                {
                    return previousSnakeMovePosition.direction;
                }
            }
        }
    }
}
