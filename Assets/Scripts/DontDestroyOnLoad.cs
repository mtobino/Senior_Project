using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private static bool exists;

    void Awake()
    {
        if (!exists)
        {
            DontDestroyOnLoad(gameObject);
            exists = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
