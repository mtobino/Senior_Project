using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerTrigger : MonoBehaviour
{
    [SerializeField] private GameObject youWinScreen;
    private const int PLAYER_LAYER = 7;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Object triggered this on layer:  " + other.gameObject.layer);
        if (other.gameObject.layer == PLAYER_LAYER)
        {
            youWinScreen.SetActive(true);
        }
    }
}
