using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsMenu.activeInHierarchy)
            {
                pauseMenu.SetActive(true);
                settingsMenu.SetActive(false);
            }
            else if (pauseMenu.activeInHierarchy) 
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
            }
        }
    }

    public void OnResumeButtonClicked()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void OnSettingsButtonClicked()
    {
        settingsMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void OnMainMenuButtonClicked()
    {
        SceneManager.LoadScene("StartScene");
    }
}
