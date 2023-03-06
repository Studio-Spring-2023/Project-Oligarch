using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    public MainMenu MenuFunctions;

    public struct MainMenu
    {
        public void StartGame()
        {
            SceneManager.LoadScene("Player Ship");
        }

        public void Settings()
        {

        }

        public void Quit()
        {
            Application.Quit();
        }
    }

    public struct PlayerShip
    {

    }
}
