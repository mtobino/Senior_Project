using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HowToPlayController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelDescription;
    [SerializeField] private TextMeshProUGUI levelTitle;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject howToPlayMenu;


    private void Awake()
    {
        Scene currScene = SceneManager.GetActiveScene();

        if (currScene.Equals(SceneManager.GetSceneByName("StarterScene")))
        {
            SetTextFields("Forest Level", "You are being chased by a vicious beast! Be careful, if you move too suddenly, it will hear you and start coming after you. Quickly search for the items to rebuild the bridge and escape into the mansion. \r\n\r\nSearch for shacks in the surrounding area that contain wood, rope, or gears. To complete the bridge you are going to need 4 pieces of wood, 2 gears, and 2 pieces of rope. ");
        }
        else if (currScene.Equals(SceneManager.GetSceneByName("MainMenuScene")))
        {
            SetTextFields("Main Menu", "On a dark and seeminly quiet night, you are doing what you love best, taking a nice stroll through the woods. Suddenly, you hear sounds in the distance and they do not sound like any animal you have heard before. Tread carefully and try to escape the beats that waits you. ");
        }
        else if (currScene.Equals(SceneManager.GetSceneByName("SecondScene")))
        {
            SetTextFields("Mansion Level", "You found shelter, but so to did the beast. While the beast took a size decrease they are still just a deadly. Quickly look for the keys to the doors and make your way to the staircase in the center of the mansion. \r\n\r\nOnce the staircase key has beeen collected, you can escape through the helicopter on the roof and make it to safety.");
        }
    }

    private void SetTextFields(string levelTitleText,string levelDescriptionText)
    {
        levelTitle.SetText(levelTitleText);
        levelDescription.SetText(levelDescriptionText);      
    }

    public void BackToPause()
    {
        howToPlayMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void ToSettings()
    {
        howToPlayMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
}
