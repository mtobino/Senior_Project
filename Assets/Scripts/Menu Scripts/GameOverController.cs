using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private AudioSource losingSound;
    [SerializeField] private TextMeshProUGUI losingPercentMessage;
    // Start is called before the first frame update

    public void Restart()
    {
        string currScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currScene, LoadSceneMode.Single);
        
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void OnEnable()
    {
        // Freeze the game
        Time.timeScale = 0f;
        //Set the text of with how much the user competed the level
        SetText();
        // lock the input
        GameInput.instance.playerInput.DeactivateInput();
        // stop all sounds
        AudioSource[] allAudios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in allAudios)
        {
            audio.Stop();
        }
        // play the victory chime
        losingSound.Play();
    }
    private void SetText()
    {
        // DO CALCULATIONS HERE
        //int percent = 5;
        //int level = 1;
        //string message = "You only completed " + percent + "% of level " + level + " :p";
        // update the textbox with the message
        losingPercentMessage.SetText("Better Luck Next Time!");
    }
}
