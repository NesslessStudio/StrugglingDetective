using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject UI;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private TMP_Dropdown qualitySettings;
    [SerializeField] private TMP_Dropdown screenResolution;
    [SerializeField] private TMP_Dropdown fpsLimit;
    [SerializeField] private Toggle toggleVsync;
    [SerializeField] private Toggle toggleFullscreen;
    [SerializeField] private Slider mouseSensitivity;
    [SerializeField] private Slider volume;

    private void Awake()
    {
        qualitySettings.value = PlayerPrefs.GetInt("qualitySettings", 0);
        OnQualitySettingsChanged();

        screenResolution.ClearOptions();
        foreach(Resolution res in Screen.resolutions)
        {
            screenResolution.options.Add(new TMP_Dropdown.OptionData(res.ToString()));
        }

        if(PlayerPrefs.GetInt("selectedScreenRes", -1) == -1)
        {
            screenResolution.value = screenResolution.options.Count - 1;
        }
        else
        {
            screenResolution.value = PlayerPrefs.GetInt("selectedScreenRes");
        }
        toggleFullscreen.isOn = PlayerPrefs.GetInt("fullscreen", 1) == 1;
        ChangeResolution();

        fpsLimit.value = PlayerPrefs.GetInt("fpsLimit");
        OnFPSLimitChanged();
        toggleVsync.isOn = PlayerPrefs.GetInt("vsync") == 1 ? true : false;
        OnToggleVsync();
        mouseSensitivity.value = PlayerPrefs.GetFloat("mouseSens", 1);
        OnMouseSensChanged();
        volume.value = PlayerPrefs.GetFloat("volume", 1);
        OnVolumeChanged();
    }

    public void OnQualitySettingsChanged()
    {
        QualitySettings.SetQualityLevel(qualitySettings.value + 1);
        QualitySettings.antiAliasing = qualitySettings.value + 1;
        PlayerPrefs.SetInt("qualitySettings", qualitySettings.value);
    }

    public void OnScreenResChanged()
    {
        ChangeResolution();
    }

    public void OnFPSLimitChanged()
    {
        int fps = int.Parse(fpsLimit.options[fpsLimit.value].text);
        Application.targetFrameRate = fps;
        Debug.Log(PlayerPrefs.GetInt("fpsLimit"));
        PlayerPrefs.SetInt("fpsLimit", fpsLimit.value);
        Debug.Log(PlayerPrefs.GetInt("fpsLimit"));
    }

    public void OnToggleVsync()
    {
        QualitySettings.vSyncCount = toggleVsync.isOn ? 1 : 0;
        PlayerPrefs.SetInt("vsync", QualitySettings.vSyncCount);
    }

    public void OnToggleFullscreen()
    {
        Screen.fullScreen = toggleFullscreen.isOn;
        PlayerPrefs.SetInt("fullscreen", toggleFullscreen.isOn ? 1 : 0);
    }

    public void OnMouseSensChanged()
    {
        if (playerController != null)
        {
            playerController.SetMouseSensitivity(mouseSensitivity.value);
        }
        PlayerPrefs.SetFloat("mouseSens", mouseSensitivity.value);
    }

    public void OnVolumeChanged()
    {
        AudioListener.volume = volume.value;
        PlayerPrefs.SetFloat("volume", volume.value);
    }

    public void OnBackButtonClicked()
    {
        if(mainMenu == null)
        {
            pauseMenu.SetActive(true);
        }
        else
        {
            mainMenu.SetActive(true);
        }
        UI.SetActive(false);
    }

    private void ChangeResolution()
    {
        string selectedResolution = screenResolution.options[screenResolution.value].text;
        string[] resAndRefRate = selectedResolution.Split("@"); 
        string[] widthHeight = resAndRefRate[0].Split("x");
        int refreshRate = int.Parse(resAndRefRate[1].Split("H")[0]);
        int resWidth = int.Parse(widthHeight[0]);
        int resHeight = int.Parse(widthHeight[1]);
        Screen.SetResolution(resWidth, resHeight, toggleFullscreen.isOn, refreshRate);

        PlayerPrefs.SetInt("selectedScreenRes", screenResolution.value);
    }
}
