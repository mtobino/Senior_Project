using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneController : MonoBehaviour
{
    public List<GameObject> objectsToPersist;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        foreach (var obj in objectsToPersist)
        {
            DontDestroyOnLoad(obj);
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.K))
        {
            loadSecondScene();
        }
    }

    public void loadSecondScene()
    {
        SceneManager.LoadScene("SecondScene");
    }

    //This will reset whichever level the player is currently in
    public void resetTheLevel()
    {
        Debug.Log("Reloading Scene: " + SceneManager.GetActiveScene().buildIndex.ToString());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
