using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{

    [SerializeField] private GameObject settingsMenu;

    public void StartGame() {
        SceneManager.LoadScene(1);
    }

    public void ShowSettings()
    {
        settingsMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
