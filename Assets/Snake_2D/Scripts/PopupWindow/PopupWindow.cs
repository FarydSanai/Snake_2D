using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snake_2D
{
    public class PopupWindow : MonoBehaviour
    {
        public static PopupWindow Instance;
        [SerializeField]
        private GameObject WinMessage;
        [SerializeField]
        private GameObject LoseMessage;

        private void Awake()
        {
            Instance = this;
        }
        public void SetWinPopup()
        {
            WinMessage.SetActive(true);
        }
        public void SetLosePopup()
        {
            LoseMessage.SetActive(true);
        }
    }
}
