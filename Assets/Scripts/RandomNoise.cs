using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNoise : MonoBehaviour
{
    [SerializeField] private AudioSource noise;
    private const int PLAYER_LAYER = 7;


    /**
     * When the player walks through the collider, it will play the sonnd and 
     * remove the game object from the scene after 5 seconds.
     */
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == PLAYER_LAYER)
        {
            PlaySound();
        }
        Destroy(gameObject, 5);
        
    }
    /**
     * Play the sound, If it is already playing, do not play the sound. 
     */
    private void PlaySound()
    {
        if (noise.isPlaying)
        {
            return;
        }
        noise.Play();
    }
}
