using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject mmGameTitle; //the main menu scene game title
    public GameObject mmStartGame; //the main menu scene start game button
    public GameObject mmSettings; //the main menu scene settings button
    public GameObject mmQuitGame; //the main menu scene quit game button
    public GameObject inventoryButton; //the starter & second scene inventory access button
    public GameObject pauseButton; //the starter & second scene pause button
    public GameObject playerHealth; //the starter & secodn scene player health bar
    public GameObject bridgeRepairMenu; //the starter scene bridge menu
    public GameObject itemsCounterPanel; //the starter scene item count panel

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        InitializeUIForScene(SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeUIForScene(scene.name);
    }

    private void InitializeUIForScene(string sceneName)
    {
        switch (sceneName)
        {
            case "MainMenuScene":
                mmGameTitle.SetActive(true);
                mmStartGame.SetActive(true);
                mmSettings.SetActive(true);
                mmQuitGame.SetActive(true);
                inventoryButton.SetActive(false); 
                pauseButton.SetActive(false);
                itemsCounterPanel.SetActive(false);
                playerHealth.SetActive(false);
                break;
            case "StarterScene":
                Destroy(mmGameTitle);
                Destroy(mmStartGame);
                Destroy(mmSettings);
                Destroy(mmQuitGame);
                inventoryButton.SetActive(true);
                pauseButton.SetActive(true);
                itemsCounterPanel.SetActive(true);
                playerHealth.SetActive(true);
                break;
            case "SecondScene":
                Destroy(bridgeRepairMenu);
                Destroy(itemsCounterPanel);
                break;
        }
    }

}
