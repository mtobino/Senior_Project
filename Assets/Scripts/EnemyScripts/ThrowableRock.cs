using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableRock : MonoBehaviour
{
    
    private void Start()
    {
        Invoke("destroyMyself", 4);
    }

    private void destroyMyself()
    {
        Destroy(this.gameObject);
    }
}
