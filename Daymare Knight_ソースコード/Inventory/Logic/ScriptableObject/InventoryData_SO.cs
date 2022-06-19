using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory",menuName = "Inventory/Inventory Data")]
public class InventoryData_SO : ScriptableObject
{
    public List<InventoryItem>items = new List<InventoryItem>();

    public void AddItems(ItemData_SO newItemData, int amount)
    {
        bool isFound = false;

        if(newItemData.isStackable)
        {
            foreach(var item in items)
            {
                if(item.itemData == newItemData)
                {
                    item.amount += amount;
                    isFound = true;
                    break;
                }
            }
        }

        for(int i = 0; i < items.Count; i++)
        {
            if(items[i].itemData == null && !isFound)
            {
                items[i].itemData = newItemData;
                items[i].amount = amount;
                break;
            }
        }

    }
}

[System.Serializable]
public class InventoryItem
{

    public ItemData_SO itemData;

    public int amount;

}