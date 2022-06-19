using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotType{BAG,WEAPON,ARMOR,ACTION}
public class SlotHolder : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public SlotType SlotType;
    public ItemUI itemUI;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.clickCount %2 == 0)
        {
            UseItem();
        }
    }

    public void UseItem()
    {
        if(itemUI.GetItem() != null)
        {
            //if(itemUI.Bag.items[itemUI.Index].itemData.itemType == ItemType.USEABLE)
            if(itemUI.GetItem().itemType == ItemType.USEABLE && itemUI.Bag.items[itemUI.Index].amount > 0)
            {
                GameManager.Instance.playerStates.ApplyHealth(itemUI.GetItem().useableItemData.healthPoint);
                itemUI.Bag.items[itemUI.Index].amount -= 1;
            }
            UpdateItem();

        }
        
    }

    public void UpdateItem()
    {
        switch(SlotType)
        {
            case SlotType.BAG:
                itemUI.Bag = InventoryManagement.Instance.inventoryData;
            break;

            case SlotType.WEAPON:
                itemUI.Bag = InventoryManagement.Instance.equipmentData;
                //切换武器
                if(itemUI.Bag.items[itemUI.Index].itemData != null)
                {
                    GameManager.Instance.playerStates.ChangeWeapon(itemUI.Bag.items[itemUI.Index].itemData);
                }
                else
                {
                    GameManager.Instance.playerStates.UnEquipWeapon();
                }
            break;

            case SlotType.ARMOR:
                itemUI.Bag = InventoryManagement.Instance.equipmentData;
            break;

            case SlotType.ACTION:
                itemUI.Bag = InventoryManagement.Instance.actionData;
            break;
        }

        var item = itemUI.Bag.items[itemUI.Index];
        itemUI.SetUpItemUI(item.itemData,item.amount);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(itemUI.GetItem())
        {
            InventoryManagement.Instance.tooltip.SetupTooltip(itemUI.GetItem());
            InventoryManagement.Instance.tooltip.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManagement.Instance.tooltip.gameObject.SetActive(false);
    }
    void OnDisable() 
    {
        InventoryManagement.Instance.tooltip.gameObject.SetActive(false);
    }
}
