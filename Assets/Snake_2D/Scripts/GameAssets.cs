using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snake_2D
{
    public class GameAssets : MonoBehaviour
    {
        public static GameAssets Instance;

        public Sprite SnakeBodySprite;
        public Sprite FruitSprite;
        public Sprite Obstackle;
        private void Awake()
        {
            Instance = this;
        }
    }
}
