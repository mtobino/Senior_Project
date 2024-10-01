using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ForestInventoryManager : MonoBehaviour, IInventoryManager
{
    public static ForestInventoryManager Instance { get; private set; }
    public List<Item> items = new List<Item>();
    public List<Item> Items => items;
    public InventoryUI inventoryUI;
    public TextMeshProUGUI warningMessageText;
    private Transform playerTransform;

    private int collectedPlanks = 0;
    private int collectedGears = 0;
    private int collectedRopes = 0;
    public TextMeshProUGUI plankCounterText;
    public TextMeshProUGUI ropeCounterText;
    public TextMeshProUGUI gearCounterText;

    private BridgeInteraction bridgeInteraction;

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
        Invoke("AssignBridgeInteraction", 0.2f);
    }

    void AssignBridgeInteraction()
    {
        bridgeInteraction = FindObjectOfType<BridgeInteraction>();
        if (bridgeInteraction == null)
        {
            Debug.LogError("BridgeInteraction component not found in the scene!");
        }
    }

    public bool AddItem(Item item)
    {
        if (items.Count >= 4) // Inventory limit
        {
            warningMessageText.gameObject.SetActive(true);
            StartCoroutine(HideWarningMessageAfterDelay(5f)); 
            return false; 
        }

        Item newItem = Instantiate(item);
        items.Add(newItem);
        UpdateInventoryUI();
        UpdateCollectibleCount(newItem.itemType, true);
        return true;
    }

    private IEnumerator HideWarningMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        warningMessageText.gameObject.SetActive(false);
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);

        Destroy(item);

        UpdateInventoryUI();
    }

    public void UseItem(int slotIndex)
    {
        if (!bridgeInteraction.IsBridgeMenuOpen)
        {
            GameManager.Instance.IsInteracting = false;
            return;
        }

        if (slotIndex < 0 || slotIndex >= items.Count)
        {
            Debug.LogWarning("UseItem called with invalid slot index.");
            return;
        }

        Item itemToUse = items[slotIndex];
        Debug.Log($"Using item: {itemToUse.itemName}, and removing it from inventory.");
        items.RemoveAt(slotIndex);

        inventoryUI.HideItemDetails();
        inventoryUI.UpdateInventorySlots();
        bridgeInteraction.UpdateBridgeRepairState(itemToUse.itemType);
        StartCoroutine(ResetInteracting());
    }

    IEnumerator ResetInteracting()
    {
        yield return new WaitForSeconds(3f); // Adjust the time based on your animation length or other factors
        GameManager.Instance.IsInteracting = false;
    }

    public void DropItem(int index)
    {
        Item itemToDrop = items[index];
        Vector3 dropPosition = playerTransform.position + playerTransform.forward;
        Instantiate(itemToDrop.prefab, dropPosition, Quaternion.identity);

        items.RemoveAt(index);
        inventoryUI.HideItemDetails();
        UpdateInventoryUI();

        UpdateCollectibleCount(itemToDrop.itemType, false);
    }

    public int selectedItemIndex = -1;

    public void SelectItem(int index)
    {
        GameManager.Instance.IsInteracting = true;

        if (index >= 0 && index < items.Count)
        {
            selectedItemIndex = index;
        }
    }
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

    public void UpdateInventoryUI()
    {
        if (inventoryUI != null)
            inventoryUI.UpdateInventorySlots();
    }

    private string GetDashesForCount(int count)
    {
        return new string('-', count); 
    }

    public void UpdateCollectibleCount(string itemType, bool isAdding)
    {
        int change = isAdding ? 1 : -1;

        switch (itemType)
        {
            case "Plank":
                collectedPlanks += change;
                plankCounterText.text = GetDashesForCount(collectedPlanks);
                break;
            case "Gear":
                collectedGears += change;
                gearCounterText.text = GetDashesForCount(collectedGears);
                break;
            case "Rope":
                collectedRopes += change;
                ropeCounterText.text = GetDashesForCount(collectedRopes);
                break;
        }
    }
}
