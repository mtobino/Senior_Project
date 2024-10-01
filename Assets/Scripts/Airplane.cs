using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : MonoBehaviour
{
    // Speed of movement
    public float speed = 10.0f;
    public AudioSource flyingSound;

    private void Start()
    {
        Invoke("playSound", 2);

    }

    // Update is called once per frame
    void Update()
    {
        // Move forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (transform.position.x <= -400) //-200
        {
            Destroy(gameObject);
        }
    }

    void playSound()
    {
        if(!flyingSound.isPlaying)
        {
            flyingSound.Play();
        }
    }
}
