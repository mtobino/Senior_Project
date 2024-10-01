using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemPickup : MonoBehaviour
{
    public Item itemData;
    private IInventoryManager inventoryManager;
    public Transform playerTransform; 
    public float pickupRange = 5f;
    public LayerMask collectable;

    void Start()
    {
        AssignInventoryManager();
        //playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (((1 << gameObject.layer) & collectable) != 0)
        {
            float distance = Vector3.Distance(playerTransform.position, GetComponent<Collider>().ClosestPoint(playerTransform.position));

            if (distance <= pickupRange && Input.GetKeyDown(KeyCode.E))
            {
                bool wasAdded = inventoryManager.AddItem(itemData);
                if (wasAdded)
                {
                    gameObject.SetActive(false);
                }
            }
        }

       /* if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
                Transform cameraTransform = Camera.main.transform;
                Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
                if (Physics.Raycast(ray, out hit, pickupRange, collectable))
                {
                ItemPickup itemPickup = hit.collider.GetComponent<ItemPickup>(); // Get the ItemPickup component from the hit object
                if (itemPickup != null && itemPickup.itemData != null) // Ensure there is an ItemPickup and it has valid item data
                {
                    bool wasAdded = inventoryManager.AddItem(itemPickup.itemData); // Use the item data from the hit object
                    if (wasAdded)
                    {
                        hit.collider.gameObject.SetActive(false); // Deactivate the hit object
                    }
                }
            }
        }*/
    }

   /* void OnTriggerStay(Collider other)
    {
        // Check if the collider is tagged as "Player" and the player presses "E"
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            // Try to add the item to the inventory
            bool wasAdded = inventoryManager.AddItem(itemData);
            if (wasAdded)
            {
                // Deactivate the item game object if added to the inventory
                gameObject.SetActive(false);
            }
        }
    }*/

    private void AssignInventoryManager()
    {
        if (SceneManager.GetActiveScene().name == "StarterScene")
        {
            inventoryManager = ForestInventoryManager.Instance;
        }
        else if (SceneManager.GetActiveScene().name == "SecondScene")
        {
            inventoryManager = MansionInventoryManager.Instance;
        }

    }
}