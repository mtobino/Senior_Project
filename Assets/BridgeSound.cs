using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeSound : MonoBehaviour
{
    public AudioSource audioSource; // Assign this in the inspector with the AudioSource component

    // This method will be called by the Animation Event
    public void PlayBridgeSound()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource or AudioClip missing!");
        }
    }
}
