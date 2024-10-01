using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyboardBindingsUI : MonoBehaviour
{
    [SerializeField] private GameObject settingsUI;
    [SerializeField] private GameObject settingsMainUI;
    [SerializeField] private GameObject controlsUI;

    public void GoBack()
    {
        controlsUI.SetActive(false);
        settingsUI.SetActive(true);
    }

    public void GoBackMain() 
    {
        controlsUI.SetActive(false);
        settingsMainUI.SetActive(true);
    }

    public void AssignBack()
    {
        if (SceneManager.GetActiveScene().name == "MainMenuScene")
        {
            GoBackMain();
        }
        else
        {
            GoBack();
        }
    }

}
