using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.UI;

public class SettingsMenuController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown grpahicsDropdown;
    [SerializeField] private GameObject settingsUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject controlsUI;

    private const string MASTER_VOLUME = "MASTER_VOLUME";

    private Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;

        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width+ " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].Equals(Screen.currentResolution))
            {
                currentResolutionIndex= i;
            }
        }

        resolutionDropdown.AddOptions(options); 
        resolutionDropdown.value= currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        grpahicsDropdown.value = QualitySettings.GetQualityLevel();
        grpahicsDropdown.RefreshShownValue();

    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat(MASTER_VOLUME, volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void GoBack()
    {
        settingsUI.SetActive(false);
        pauseUI.SetActive(true);
    }

    public void GoBackMainMenu()
    {
        settingsUI.SetActive(false);
    }

    public void EditControls()
    {
        controlsUI.SetActive(true);
        settingsUI.SetActive(false);
    }
}
