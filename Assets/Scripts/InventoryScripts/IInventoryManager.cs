using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryManager
{
    List<Item> Items { get; }
    bool AddItem(Item item);
    void RemoveItem(Item item);
    void UseItem(int slotIndex);
    void DropItem(int index);
    void SelectItem(int index);
    void UseSelectedItem();
    void DropSelectedItem();
    void UpdateInventoryUI();

}
