using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public CinemachineVirtualCamera doorCamera; // the camera positioned at door for close up
    public Animator doorAnimator;
    public AudioSource audioSource;
    public AudioClip doorOpenSound;
    public AudioClip doorJiggleSound;
    public AudioClip keyTurnSound;
    public float maxDistance = 2f; // Max distance at which the door can be interacted with
    public LayerMask doorLayerMask;
    public GameObject key; // the key positioned for unlocking & animations
    private Animator keyAnimator;
    public TextMeshProUGUI doorText; // screen text for door related messages

    public bool isDoorOpen = false;

    private void Start()
    {
        keyAnimator = key.GetComponent<Animator>();
        doorText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isDoorOpen && !MansionGameManager.Instance.IsInteracting)
        {
            if (IsPlayerFacingDoor())
            {
                audioSource.PlayOneShot(doorJiggleSound);
                ShowLockedMessage();
            }
        }
    }

    public void UnlockDoorWithKey()
    {
        doorCamera.Priority = 11;
        key.SetActive(true);
        StartCoroutine(PlayAnimations());
    }

    IEnumerator PlayAnimations()
    {
        audioSource.PlayOneShot(keyTurnSound);
        keyAnimator.SetTrigger("TurnKey");
        yield return new WaitForSeconds(3f); 
        key.SetActive(false);
        doorAnimator.SetTrigger("OpenTheDoor");
        audioSource.PlayOneShot(doorOpenSound);
        yield return new WaitForSeconds(2f);
        doorCamera.Priority = 1;
        GetComponent<Collider>().enabled = false;
        isDoorOpen = true; 
    }

    private void ShowLockedMessage()
    {
        doorText.text = "The door stands firm and locked. Perhaps there's a key nearby?";
        doorText.gameObject.SetActive(true);
        StartCoroutine(HideMessageAfterDelay(3f)); 
    }

    IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        doorText.gameObject.SetActive(false);
    }

    public bool IsPlayerFacingDoor()
    {
        RaycastHit hit;
        Transform cameraTransform = Camera.main.transform;
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (Physics.Raycast(ray, out hit, maxDistance, doorLayerMask))
        {
            return hit.collider.gameObject == gameObject;
        }
        return false;
    }
}
