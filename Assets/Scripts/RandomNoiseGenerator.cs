using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNoiseGenerator : MonoBehaviour
{
    [SerializeField] private GameObject sound1;
    [SerializeField] private GameObject sound2;
    [SerializeField] private GameObject sound3;
    [SerializeField] private GameObject sound4;
    [SerializeField] private GameObject sound5;

    [SerializeField] private int MAX = 450;
    [SerializeField] private int MIN = 50;
    [SerializeField] private int NUM_OF_SPAWNS_PER_SOUND = 25;

    // Start is called before the first frame update
    void Start()
    {
        SpawnAllSounds();
            
    }

    private void SpawnAllSounds()
    {
        SpawnSound(sound4);
        SpawnSound(sound2);
        SpawnSound(sound3);
        SpawnSound(sound1);
        SpawnSound(sound5);
    }

    private void SpawnSound(GameObject sound)
    {
        for(int i = 0; i < NUM_OF_SPAWNS_PER_SOUND; i++)
        {
            float x = Random.Range(MIN, MAX);
            float z = Random.Range(MIN, MAX);
            Instantiate(sound, new Vector3(x, 0, z), Quaternion.identity);
        }
    }

}
