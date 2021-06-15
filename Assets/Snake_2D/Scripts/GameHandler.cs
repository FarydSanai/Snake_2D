using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Snake_2D
{
    public enum SnakeScenes
    {
        MainSnakeScene,
    }
    public class GameHandler : MonoBehaviour
    {
        public static GameHandler Instance;

        [SerializeField]
        private SnakeControl Snake;
        private LevelGrid LevelGrid;
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            LevelGrid = new LevelGrid(20, 20);
            Snake.InitGrid(LevelGrid);
            LevelGrid.InitSnake(Snake);
            LevelGrid.GenerateObstackles();
        }
        public void StartNewGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SnakeScenes.MainSnakeScene.ToString());
        }
        public void EndGame()
        {
            Time.timeScale = 0f;
        }
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
