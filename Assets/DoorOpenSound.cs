using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenSound : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // This will be called by the AnimationEvent
    public void PlaySound()
    {
        audioSource.Play();
    }
}
