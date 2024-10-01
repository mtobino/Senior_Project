using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player Object")]
    [SerializeField] GameObject player;

    [Header("Wooden Plank GameObjects")]
    [SerializeField] GameObject plank1;
    [SerializeField] GameObject plank2;
    [SerializeField] GameObject plank3;
    [SerializeField] GameObject plank4;
    [SerializeField] GameObject plank5;
    [SerializeField] GameObject plank6;
    [SerializeField] GameObject plank7;
    [SerializeField] GameObject plank8;

    [Header("Rope GameObjects")]
    [SerializeField] GameObject rope1;
    [SerializeField] GameObject rope2;
    [SerializeField] GameObject rope3;
    [SerializeField] GameObject rope4;

    [Header("Gear GameObjects")]
    [SerializeField] GameObject gear1;
    [SerializeField] GameObject gear2;
    [SerializeField] GameObject gear3;
    [SerializeField] GameObject gear4;

    [Header("Bridge GameObjects")]
    [SerializeField] GameObject bridge1;
    [SerializeField] GameObject bridge2;
    [SerializeField] GameObject bridge3;
    [SerializeField] GameObject bridge4;

    [Header("Ability Selector Menu")]
    [SerializeField] GameObject abilitySelectorMenu;

    [Header("Ability Icons")]
    [SerializeField] Sprite speedAbilityIcon;
    [SerializeField] Sprite invisibilityIcon;
    [SerializeField] Sprite healerAbilityIcon;

    [Header("UI References")]
    [SerializeField] UnityEngine.UI.Image abilityIcon;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject keybindsMenu;
    [SerializeField] GameObject HowToPlayMenu;
    [SerializeField] GameObject MainMenu;

    [Header("End-The-Game References")]
    [SerializeField] HealthManager healthManager;
    [SerializeField] GameObject gameOverPanel;
   
    [Header("Fences")]
    [SerializeField] GameObject fence;
    [SerializeField] GameObject fenceWithHole;

    private bool isGameOver = false;

    private string abilityName = "";
    //private Player playerRef;

    [SerializeField] private Player playerRef;

    public bool IsInteracting { get; set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pickWoodLocations();
        pickRopeLocations();
        pickGearLocations();
        pickBridgeLocation();
        //playerRef = Player.instance;

    }

    // Update is called once per frame
    void Update()
    {
        //will add stuff here later, probably
        if (abilitySelectorMenu.activeSelf || pauseMenu.activeSelf || 
            settingsMenu.activeSelf || HowToPlayMenu.activeSelf ||
            keybindsMenu.activeSelf || MainMenu.activeSelf || gameOverPanel.activeSelf)
        {
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        }
        checkForGameOver();

        if (Input.GetKeyDown(KeyCode.K))
        {
            loadSecondScene();
        }
    }

    public void startGame()
    {
        SceneManager.LoadScene("StarterScene");
        abilitySelectorMenu.SetActive(true);
    }

    public void loadSecondScene()
    {
        SceneManager.LoadScene("SecondScene");
    }

    public void selectSpeedAbility()
    {
        abilityName = "SpeedAbility";
        Debug.Log("Player Selected: " + abilityName);
        abilityIcon.sprite = speedAbilityIcon;
    }

    public void selectInvisibleAbility()
    {
        abilityName = "InvisibleAbility";
        Debug.Log("Player Selected: " + abilityName);
        abilityIcon.sprite = invisibilityIcon;
    }

    public void selectHealerAbility()
    {
        abilityName = "HealerAbility";
        Debug.Log("Player Selected: " + abilityName);
        abilityIcon.sprite = healerAbilityIcon;
    }

    public void confirmAbility()
    {
        Type abilityType = Type.GetType(abilityName); // Get the Type of the ability by its name

        if (abilityType != null)
        {
            // Add the ability component to the player GameObject
            player.AddComponent(abilityType);
            Debug.Log("Player spawned in with: " + abilityName);
        }
        else
        {
            Debug.LogError("Unable to find ability with name: " + abilityName);
        }

        abilitySelectorMenu.SetActive(false);
    }
   
    private void pickWoodLocations()
    {
        // This list will hold all 4 locations that have been chosen
        List<int> chosenNums = new List<int>();
        while (chosenNums.Count < 4)
        {
            int randomNum = UnityEngine.Random.Range(0, 8); //8 possible spawn locations for the wooden planks (0-7)

            //Only IF the list doesn't already have the chosen random num, add it
            if (!chosenNums.Contains(randomNum))
                chosenNums.Add(randomNum);

        }
        // These are the 4 wooden plank spawn locations that have been chosen, no duplicates
        for (int i = 0; i < chosenNums.Count; i++)
            Debug.Log("Wood location spawned: " + chosenNums[i]);

        //This is where the actual gameobjects are activated
        for (int i = 0; i < chosenNums.Count; i++)
        {
            switch (chosenNums[i])
            {
                case 0:
                    plank1.SetActive(true);
                    break;
                case 1:
                    plank2.SetActive(true);
                    break;
                case 2:
                    plank3.SetActive(true);
                    break;
                case 3:
                    plank4.SetActive(true);
                    break;
                case 4:
                    plank5.SetActive(true);
                    break;
                case 5:
                    plank6.SetActive(true);
                    break;
                case 6:
                    plank7.SetActive(true);
                    break;
                case 7:
                    plank8.SetActive(true);
                    break;
                default:
                    Debug.Log("Something went terribly wrong...Check pickWoodLocations method");
                    break;
            }

        }

    }

    private void pickRopeLocations()
    {
        // This list will hold all 4 locations that have been chosen
        List<int> chosenNums = new List<int>();
        while (chosenNums.Count < 2)
        {
            int randomNum = UnityEngine.Random.Range(0, 4); //4 possible spawn locations for the rope (0-3)

            //Only IF the list doesn't already have the chosen random num, add it
            if (!chosenNums.Contains(randomNum))
                chosenNums.Add(randomNum);

        }
        // These are the 2 rope spawn locations that have been chosen, no duplicates
        for (int i = 0; i < chosenNums.Count; i++)
            Debug.Log("Rope location spawned: " + chosenNums[i]);

        //This is where the actual gameobjects are activated
        for (int i = 0; i < chosenNums.Count; i++)
        {
            switch (chosenNums[i])
            {
                case 0:
                    rope1.SetActive(true);
                    break;
                case 1:
                    rope2.SetActive(true);
                    break;
                case 2:
                    rope3.SetActive(true);
                    break;
                case 3:
                    rope4.SetActive(true);
                    break;
                default:
                    Debug.Log("Something went terribly wrong...Check pickRopeLocations method");
                    break;
            }

        }
    }

    private void pickGearLocations()
    {
        // This list will hold all 4 locations that have been chosen
        List<int> chosenNums = new List<int>();
        while (chosenNums.Count < 2)
        {
            int randomNum = UnityEngine.Random.Range(0, 4); //4 possible spawn locations for the gears (0-3)

            //Only IF the list doesn't already have the chosen random num, add it
            if (!chosenNums.Contains(randomNum))
                chosenNums.Add(randomNum);

        }
        // These are the 2 gear spawn locations that have been chosen, no duplicates
        for (int i = 0; i < chosenNums.Count; i++)
            Debug.Log("Gear location spawned: " + chosenNums[i]);

        //This is where the actual gameobjects are activated
        for (int i = 0; i < chosenNums.Count; i++)
        {
            switch (chosenNums[i])
            {
                case 0:
                    gear1.SetActive(true);
                    break;
                case 1:
                    gear2.SetActive(true);
                    break;
                case 2:
                    gear3.SetActive(true);
                    break;
                case 3:
                    gear4.SetActive(true);
                    break;
                default:
                    Debug.Log("Something went terribly wrong...Check pickGearLocations method");
                    break;
            }

        }
    }

    private void pickBridgeLocation()
    {
        int chosenBridgeIndex = UnityEngine.Random.Range(0, 4);

        // Activate the selected bridge
        switch (chosenBridgeIndex)
        {
            case 0:
                bridge1.SetActive(true);
                // South wall
                Instantiate(fence, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                // West wall with hole
                Instantiate(fenceWithHole, new Vector3(0, 0, 500f), Quaternion.Euler(0, 90f, 0));
                // North Wall
                Instantiate(fence, new Vector3(0, 0, 500f), Quaternion.Euler(0, 0, 0));
                // East Wall
                Instantiate(fence, new Vector3(500f, 0, 0), Quaternion.Euler(0, -90f, 0));
                break;
            case 1:
                bridge2.SetActive(true);
                // South wall
                Instantiate(fence, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                // West wall with hole
                Instantiate(fence, new Vector3(0, 0, 500f), Quaternion.Euler(0, 90f, 0));
                // North Wall
                Instantiate(fenceWithHole, new Vector3(0, 0, 500f), Quaternion.Euler(0, 0, 0));
                // East Wall
                Instantiate(fence, new Vector3(500f, 0, 0), Quaternion.Euler(0, -90f, 0));
                break;
            case 2:
                bridge3.SetActive(true);
                // South wall
                Instantiate(fence, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                // West wall with hole
                Instantiate(fence, new Vector3(0, 0, 500f), Quaternion.Euler(0, 90f, 0));
                // North Wall
                Instantiate(fence, new Vector3(0, 0, 500f), Quaternion.Euler(0, 0, 0));
                // East Wall
                Instantiate(fenceWithHole, new Vector3(500f, 0, 0), Quaternion.Euler(0, -90f, 0));
                break;
            case 3:
                bridge4.SetActive(true);
                // South wall
                Instantiate(fenceWithHole, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                // West wall with hole
                Instantiate(fence, new Vector3(0, 0, 500f), Quaternion.Euler(0, 90f, 0));
                // North Wall
                Instantiate(fence, new Vector3(0, 0, 500f), Quaternion.Euler(0, 0, 0));
                // East Wall
                Instantiate(fence, new Vector3(500f, 0, 0), Quaternion.Euler(0, -90f, 0));
                break;
            default:
                Debug.LogError("Invalid bridge index chosen. Check pickBridgeLocation method.");
                break;
        }

        Debug.Log("Bridge location spawned: " + chosenBridgeIndex);
    }

    private void checkForGameOver()
    {
        if (!isGameOver)
        {
            if (playerRef == null)
            {
                Debug.LogError("Player reference is null.");
            }
            else if (playerRef.healthManager == null)
            {
                Debug.LogError("HealthManager on playerRef is null.");
            }
            else
            {
                if (playerRef.healthManager.healthAmount <= 0)
                {
                    gameOverPanel.SetActive(true);
                    Debug.Log("Game over...");
                    isGameOver = true;
                }
                else
                {
                    Debug.Log("Health is above zero.");
                }
            }
        }
        else
        {
            Debug.Log("Game over state has already been set.");
        }
    }

}
