using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject settingsMenuUI;
    [SerializeField] private GameObject editControlsMenuUI;
    [SerializeField] private GameObject howToPlayMenuUI;
    [SerializeField] private GameObject settingsMainMenuUI;
    [SerializeField] private GameObject[] menus;
    public bool isPaused;

    private void Awake()
    {
        isPaused = false;
        //GameInput.instance.OnMenuToggle += GameInput_OnMenuToggle;

    }

    /*private void GameInput_OnMenuToggle(object sender, System.EventArgs e)
    {
        if (isPaused)
        {
            TurnOffMenus();
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }*/
    public void PauseMenuPressed()
    {
        if (isPaused)
        {
            TurnOffMenus();
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused= false;
    }

    public void EditSettings()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
    }

    public void EditSettingsMainMenu()
    {
        settingsMainMenuUI.SetActive(true);
    }

    public void HowToPlay()
    {
        pauseMenuUI.SetActive(false);
        howToPlayMenuUI.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit(); 
    }
    private void TurnOffMenus()
    {
        foreach(GameObject menu in menus)
        {
            menu.SetActive(false);
        }
    }
}
