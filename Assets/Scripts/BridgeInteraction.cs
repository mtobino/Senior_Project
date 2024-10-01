using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BridgeInteraction : MonoBehaviour
{
    public GameObject bridgeRepairMenu;
    public GameObject bridge;
    public float maxDistance = 5f;
    public Image[] plankImages;
    public Image[] ropeImages;
    public Image[] gearImages;
    public bool IsBridgeMenuOpen { get; private set; }
    public TextMeshProUGUI successMessage;
    private Animator bridgeAnimator;

    private int plankUsed = 0;
    private int ropesUsed = 0;
    private int gearsUsed = 0;
    private bool bridgeCompleted = false;

    public AudioClip plankSound;
    public AudioClip ropeSound;
    public AudioClip gearSound;
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        bridgeRepairMenu.SetActive(false);
        //bridge.GetComponent<Collider>().enabled = false;
        bridgeAnimator = bridge.GetComponent<Animator>();
        audioSource = bridgeRepairMenu.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (IsPlayerFacingBridge())
        {
            if (Input.GetKeyDown(KeyCode.E) && !bridgeCompleted && !GameManager.Instance.IsInteracting)
            {
                IsBridgeMenuOpen = !IsBridgeMenuOpen;
                bridgeRepairMenu.SetActive(IsBridgeMenuOpen);
            }
        }
    }

    private bool IsPlayerFacingBridge()
    {
        RaycastHit hit;
        Transform cameraTransform = Camera.main.transform;
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.collider.gameObject == bridge)
            {
                return true;
            }
        }
        return false;
    }

    public void UpdateBridgeRepairState(string itemType)
    {
        switch (itemType)
        {
            case "Plank":
                plankUsed++;
                ActivateNextItem(plankImages, plankSound);
                break;
            case "Rope":
                ropesUsed++;
                ActivateNextItem(ropeImages, ropeSound);
                break;
            case "Gear":
                gearsUsed++;
                ActivateNextItem(gearImages, gearSound);
                break;
        }

        if (plankUsed >= 4 && ropesUsed >= 2 && gearsUsed >= 2)
        {
            bridgeCompleted = true;
            bridge.GetComponent<Collider>().enabled = false;
            successMessage.gameObject.SetActive(true);
            StartCoroutine(PerformBridgeActivation());
        }
    }
    void ActivateNextItem(Image[] itemImages, AudioClip itemSound)
    {
        foreach (var img in itemImages)
        {
            if (!img.gameObject.activeSelf)
            {
                img.gameObject.SetActive(true);
                audioSource.PlayOneShot(itemSound);
                break; 
            }
        }
    }

    private IEnumerator CloseBridgeMenu()
    {
        yield return new WaitForSeconds(10f);
        bridgeRepairMenu.SetActive(false);
    }

    private IEnumerator PerformBridgeActivation()
    {
        yield return new WaitForSeconds(3.5f);
        bridgeRepairMenu.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        bridgeAnimator.SetTrigger("LowerTheBridge");
    }
}
