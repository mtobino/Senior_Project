using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MansionInventoryManager : MonoBehaviour, IInventoryManager
{
    public static MansionInventoryManager Instance { get; private set; }
    public List<Item> items = new List<Item>();
    public List<Item> Items => items;
    public InventoryUI inventoryUI;
    public TextMeshProUGUI warningMessageText;
    public TextMeshProUGUI wrongDoorText;
    private Transform playerTransform;

    public GameObject door1;
    private DoorInteraction door1Script;
    public GameObject door2;
    private DoorInteraction door2Script;
    public GameObject door3;
    private DoorInteraction door3Script;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        door1Script = door1.GetComponent<DoorInteraction>();
        door2Script = door2.GetComponent<DoorInteraction>();
        door3Script = door3.GetComponent<DoorInteraction>();
    }

    public bool AddItem(Item item)
    {
        if (items.Count >= 4) // Inventory limit
        {
            warningMessageText.gameObject.SetActive(true);
            StartCoroutine(HideWarningMessage(5f));
            return false;
        }

        Item newItem = Instantiate(item);
        items.Add(newItem);
        UpdateInventoryUI();
        return true;
    }

    private IEnumerator HideWarningMessage(float delay)
    {
        yield return new WaitForSeconds(delay);
        warningMessageText.gameObject.SetActive(false);
    }

    private IEnumerator HideWrongDoorMessage(float delay)
    {
        yield return new WaitForSeconds(delay);
        wrongDoorText.gameObject.SetActive(false);
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);

        Destroy(item);

        UpdateInventoryUI();
    }

    public void UseItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= items.Count)
        {
            Debug.LogWarning("UseItem called with invalid slot index.");
            return;
        }

        Item itemToUse = items[slotIndex];
        if (door1Script.IsPlayerFacingDoor() && !door2Script.isDoorOpen && !door3Script.isDoorOpen)
        {
            door1Script.UnlockDoorWithKey();
            items.RemoveAt(slotIndex);
            inventoryUI.HideItemDetails();
            inventoryUI.UpdateInventorySlots();
        }
        else if (door2Script.IsPlayerFacingDoor() && door1Script.isDoorOpen && !door3Script.isDoorOpen)
        {
            door2Script.UnlockDoorWithKey();
            items.RemoveAt(slotIndex);
            inventoryUI.HideItemDetails();
            inventoryUI.UpdateInventorySlots();
        }
        else if (door3Script.IsPlayerFacingDoor() && door1Script.isDoorOpen && door2Script.isDoorOpen)
        {
            door3Script.UnlockDoorWithKey();
            items.RemoveAt(slotIndex);
            inventoryUI.HideItemDetails();
            inventoryUI.UpdateInventorySlots();
        }
        else
        {
            wrongDoorText.text = "hmm...this key doesnt seem to fit, try it on a different door.";
            wrongDoorText.gameObject.SetActive(true);
            StartCoroutine(HideWrongDoorMessage(3f));
        }

        StartCoroutine(ResetInteracting());   
    }

    IEnumerator ResetInteracting()
    {
        yield return new WaitForSeconds(5f); // Adjust the time based on your animation length or other factors
        MansionGameManager.Instance.IsInteracting = false;
    }

    public int selectedItemIndex = -1;

    public void UseSelectedItem()
    {
        if (selectedItemIndex >= 0 && selectedItemIndex < items.Count)
        {
            UseItem(selectedItemIndex);
            selectedItemIndex = -1;
        }
    }
    public void DropSelectedItem()
    {
        if (selectedItemIndex >= 0 && selectedItemIndex < items.Count)
        {
            DropItem(selectedItemIndex);
            selectedItemIndex = -1;
        }
    }
    public void DropItem(int index)
    {
        Item itemToDrop = items[index];
        Vector3 dropPosition = playerTransform.position + playerTransform.forward;
        Instantiate(itemToDrop.prefab, dropPosition, Quaternion.identity);

        items.RemoveAt(index);
        inventoryUI.HideItemDetails();
        UpdateInventoryUI();

    }
    public void SelectItem(int index)
    {
        MansionGameManager.Instance.IsInteracting = true;

        if (index >= 0 && index < items.Count)
        {
            selectedItemIndex = index;
        }
    }

    public void UpdateInventoryUI()
    {
        if (inventoryUI != null)
            inventoryUI.UpdateInventorySlots();
    }
}
