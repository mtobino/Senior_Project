using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouWinController : MonoBehaviour
{
    [SerializeField] private AudioSource victorySound;
    private const string LEVEL_ONE = "StarterScene";
    private const string MAIN_MENU = "MainMenuScene";

    public void PlayAgain()
    {
        SceneManager.LoadScene(LEVEL_ONE, LoadSceneMode.Single);
        //gameObject.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(MAIN_MENU, LoadSceneMode.Single);
        //gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OnEnable()
    {
        // Freeze the game
        Time.timeScale= 0f;
        // lock the input
        GameInput.instance.playerInput.DeactivateInput(); 
        // stop all sounds
        AudioSource[] allAudios = FindObjectsOfType<AudioSource>();
        foreach(AudioSource audio in allAudios)
        {
            audio.Stop();
        }
        // play the victory chime
        victorySound.Play();
    }
}
