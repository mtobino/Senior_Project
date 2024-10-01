using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotButton : MonoBehaviour
{
    public int slotIndex; 

    public InventoryUI inventoryUI; 

    public void OnClick()
    {
        inventoryUI.OnSlotButtonClicked(slotIndex);
    }
}
