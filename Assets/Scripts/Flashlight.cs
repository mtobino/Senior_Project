using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public GameObject flashlight;

    public AudioSource turnOn;
    public AudioSource turnOff;
    [SerializeField] private GameInput gameInput;

    public bool isOn;

    void Start()
    {
        isOn = false;
        flashlight.SetActive(false);
        gameInput.OnFlashlightToggle += GameInput_OnFlashlightToggle;
    }

    /**
     * Updated to Unity's input controller. This allows us to not check in every frame if the button is pressed but 
     * instead just listen for when it is pressed. 
     */
    private void GameInput_OnFlashlightToggle(object sender, System.EventArgs e)
    {
        isOn = !isOn;

        flashlight.SetActive(isOn);

        if (turnOn)
            turnOn.Play();
        else
            turnOff.Play();

    }
}
