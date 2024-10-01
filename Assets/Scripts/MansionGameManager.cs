using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class MansionGameManager : MonoBehaviour
{
   public static MansionGameManager Instance { get; private set; }

    [Header("Player Object")]
    [SerializeField] Player player;

    [Header("Key GameObjects - Section 1")]
    [SerializeField] GameObject s1Key1;
    [SerializeField] GameObject s1Key2;
    [SerializeField] GameObject s1Key3;
    [SerializeField] GameObject s1Key4;

    [Header("Key GameObjects - Section 2")]
    [SerializeField] GameObject s2Key1;
    [SerializeField] GameObject s2Key2;
    [SerializeField] GameObject s2Key3;
    [SerializeField] GameObject s2Key4;

    [Header("Key GameObjects - Section 3")]
    [SerializeField] GameObject s3Key1;
    [SerializeField] GameObject s3Key2;
    [SerializeField] GameObject s3Key3;
    [SerializeField] GameObject s3Key4;

    [Header("UI References")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject keybindsMenu;
    [SerializeField] GameObject HowToPlayMenu;

    [Header("End-The-Game References")]
    [SerializeField] HealthManager healthManager;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] private GameObject youWinScreen;
    private const int PLAYER_LAYER = 7;
    private bool isGameOver = false;

    public bool IsInteracting { get; set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        pickSection1KeyLocations();
        pickSection2KeyLocations(); 
        pickSection3KeyLocations(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseMenu.activeSelf || settingsMenu.activeSelf || 
            HowToPlayMenu.activeSelf || keybindsMenu.activeSelf || 
            youWinScreen.activeSelf || gameOverPanel.activeSelf)
        {
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        }
        checkForGameOver(); //constantly checking for game over status
    }

    private void pickSection1KeyLocations2()
    {
        // This list will hold the locations chosen
        List<int> chosenNums = new List<int>();
        while (chosenNums.Count < 2) // Now choosing 2 locations
        {
            int randomNum = UnityEngine.Random.Range(0, 4); //4 possible spawn locations for the keys (0-3)

            //Only IF the list doesn't already have the chosen random num, add it
            if (!chosenNums.Contains(randomNum))
                chosenNums.Add(randomNum);
        }
        // Log the chosen key locations
        foreach (int location in chosenNums)
        {
            Debug.Log("Section 1 key location spawned: " + location);
        }

        // Activate the correct key at the first chosen location
        GameObject correctKey = null;
        switch (chosenNums[0]) // Correct key
        {
            case 0:
                correctKey = s1Key1;
                break;
            case 1:
                correctKey = s1Key2;
                break;
            case 2:
                correctKey = s1Key3;
                break;
            case 3:
                correctKey = s1Key4;
                break;
        }
        correctKey.SetActive(true);
        correctKey.GetComponent<Key>().isCorrectKey = true; // Ensure this key is marked as correct

        // Activate the incorrect key at the second chosen location
        GameObject incorrectKey = null;
        switch (chosenNums[1]) // Incorrect key
        {
            case 0:
                incorrectKey = s1Key1;
                break;
            case 1:
                incorrectKey = s1Key2;
                break;
            case 2:
                incorrectKey = s1Key3;
                break;
            case 3:
                incorrectKey = s1Key4;
                break;
        }
        if (incorrectKey != correctKey) // Check to avoid reactivating the correct key
        {
            incorrectKey.SetActive(true);
            incorrectKey.GetComponent<Key>().isCorrectKey = false; // Mark this key as incorrect
        }
    }
    private void pickSection1KeyLocations()
    {
        // This list will hold the location chosen
        List<int> chosenNums = new List<int>();
        while (chosenNums.Count < 1)
        {
            int randomNum = UnityEngine.Random.Range(0, 4); //4 possible spawn locations for the wooden planks (0-3)

            //Only IF the list doesn't already have the chosen random num, add it
            if (!chosenNums.Contains(randomNum))
                chosenNums.Add(randomNum);

        }
        // This is the key location chosen for sec 1
        for (int i = 0; i < chosenNums.Count; i++)
            Debug.Log("Section 1 key location spawned: " + chosenNums[i]);

        //This is where the actual gameobjects are activated
        for (int i = 0; i < chosenNums.Count; i++)
        {
            switch (chosenNums[i])
            {
                case 0:
                    s1Key1.SetActive(true);
                    break;
                case 1:
                    s1Key2.SetActive(true);
                    break;
                case 2:
                    s1Key3.SetActive(true);
                    break;
                case 3:
                    s1Key4.SetActive(true);
                    break;
                default:
                    Debug.Log("Something went terribly wrong...Check pickSec1KeyLocations method");
                    break;
            }

        }
    }

    private void pickSection2KeyLocations()
    {
        // This list will hold the location chosen
        List<int> chosenNums = new List<int>();
        while (chosenNums.Count < 1)
        {
            int randomNum = UnityEngine.Random.Range(0, 4); //4 possible spawn locations for the wooden planks (0-3)

            //Only IF the list doesn't already have the chosen random num, add it
            if (!chosenNums.Contains(randomNum))
                chosenNums.Add(randomNum);

        }
        // This is the key location chosen for sec 1
        for (int i = 0; i < chosenNums.Count; i++)
            Debug.Log("Section 1 key location spawned: " + chosenNums[i]);

        //This is where the actual gameobjects are activated
        for (int i = 0; i < chosenNums.Count; i++)
        {
            switch (chosenNums[i])
            {
                case 0:
                    s2Key1.SetActive(true);
                    break;
                case 1:
                    s2Key2.SetActive(true);
                    break;
                case 2:
                    s2Key3.SetActive(true);
                    break;
                case 3:
                    s2Key4.SetActive(true);
                    break;
                default:
                    Debug.Log("Something went terribly wrong...Check pickSec1KeyLocations method");
                    break;
            }

        }
    }

    private void pickSection3KeyLocations()
    {
        // This list will hold the location chosen
        List<int> chosenNums = new List<int>();
        while (chosenNums.Count < 1)
        {
            int randomNum = UnityEngine.Random.Range(0, 4); //4 possible spawn locations for the wooden planks (0-3)

            //Only IF the list doesn't already have the chosen random num, add it
            if (!chosenNums.Contains(randomNum))
                chosenNums.Add(randomNum);

        }
        // This is the key location chosen for sec 1
        for (int i = 0; i < chosenNums.Count; i++)
            Debug.Log("Section 1 key location spawned: " + chosenNums[i]);

        //This is where the actual gameobjects are activated
        for (int i = 0; i < chosenNums.Count; i++)
        {
            switch (chosenNums[i])
            {
                case 0:
                    s3Key1.SetActive(true);
                    break;
                case 1:
                    s3Key2.SetActive(true);
                    break;
                case 2:
                    s3Key3.SetActive(true);
                    break;
                case 3:
                    s3Key4.SetActive(true);
                    break;
                default:
                    Debug.Log("Something went terribly wrong...Check pickSec1KeyLocations method");
                    break;
            }

        }
    }

    private void checkForGameOver()
    {
        if (!isGameOver)
        {
            if (player.healthManager.healthAmount <= 0)
            {
                gameOverPanel.SetActive(true);
                Debug.Log("Game over...");
                isGameOver = true;
            }
        }
    }

    

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Object triggered this on layer:  " + other.gameObject.layer);
        if (other.gameObject.layer == PLAYER_LAYER)
        {
            youWinScreen.SetActive(true);
        }
    }
}
