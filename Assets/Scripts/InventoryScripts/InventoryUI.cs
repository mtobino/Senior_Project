using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public class InventoryUI : MonoBehaviour 
{
    private IInventoryManager inventoryManager;
    public GameObject[] slotPanels;
    public GameObject inventoryPanel;
    public GameObject itemDetailsPanel; 
    public TextMeshProUGUI itemNameText; 
    public TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI itemSlotNumberText;
    public Image itemIcon;
    private bool isInventoryVisible = false;

    private void Start()
    {
        AssignInventoryManager();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { inventoryManager.SelectItem(0); } 
        if (Input.GetKeyDown(KeyCode.Alpha2)) { inventoryManager.SelectItem(1); } 
        if (Input.GetKeyDown(KeyCode.Alpha3)) { inventoryManager.SelectItem(2); } 
        if (Input.GetKeyDown(KeyCode.Alpha4)) { inventoryManager.SelectItem(3); }
        if (Input.GetKeyDown(KeyCode.E)) { inventoryManager.UseSelectedItem(); }
        if (Input.GetKeyDown(KeyCode.G)) { inventoryManager.DropSelectedItem(); }
        if (Input.GetKeyDown(KeyCode.I)) { ToggleInventory(); }
    }

    public void UpdateInventorySlots()
    {
        for (int i = 0; i < slotPanels.Length; i++)
        {
            Image slotIcon = slotPanels[i].transform.GetChild(0).GetComponent<Image>();

            if (i < inventoryManager.Items.Count)
            {
                Item item = inventoryManager.Items[i];
                slotIcon.sprite = item.icon;
                slotIcon.gameObject.SetActive(true);
            }
            else
            {
                slotIcon.sprite = null;
                slotIcon.gameObject.SetActive(false);
            }
        }
    }

    public void ToggleInventory()
    {
        isInventoryVisible = !isInventoryVisible;
        inventoryPanel.SetActive(isInventoryVisible);

    }

    public void OnSlotButtonClicked(int slotIndex)
    {
        inventoryManager.UseItem(slotIndex); 
    }

    public void ShowItemDetails(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= inventoryManager.Items.Count)
        {
            return;
        }

        Item itemToShow = inventoryManager.Items[slotIndex];

        itemNameText.text = itemToShow.itemName;
        itemDescriptionText.text = itemToShow.description;
        itemIcon.sprite = itemToShow.icon;

        char keyToPress = (char)('1' + slotIndex); 
        itemSlotNumberText.text = $"press    {keyToPress}    to select item";

        itemDetailsPanel.SetActive(true);
    }

    public void HideItemDetails()
    {
        itemDetailsPanel.SetActive(false);
    }

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
