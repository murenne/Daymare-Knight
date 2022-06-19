using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemUI))]
public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    ItemUI currentItemUI;
    SlotHolder currentHolder;
    SlotHolder targetHolder;

    void Awake() 
    {
        currentItemUI = GetComponent<ItemUI>();
        currentHolder = GetComponentInParent<SlotHolder>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //记录原始数据
        InventoryManagement.Instance.currentDrag = new InventoryManagement.DragData();
        InventoryManagement.Instance.currentDrag.originalHolder = GetComponentInParent<SlotHolder>();
        InventoryManagement.Instance.currentDrag.originalParent = (RectTransform)transform.parent;

        transform.SetParent(InventoryManagement.Instance.dragCanvas.transform,true);
        
    }

    public void OnDrag(PointerEventData eventData)
    {
       //跟随鼠标
       transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //放下物品 交换数据
        //是否指向UI物品
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (InventoryManagement.Instance.CheckInActionUI(eventData.position) || InventoryManagement.Instance.CheckInEquipmentUI(eventData.position) ||InventoryManagement.Instance.CheckInInventoryUI(eventData.position))
            {
                if (eventData.pointerEnter.gameObject.GetComponent<SlotHolder>())
                    targetHolder = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();
                else
                    targetHolder = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();


                if(targetHolder != InventoryManagement.Instance.currentDrag.originalHolder) //判断是否目标holder是原holder
                

                    switch (targetHolder.SlotType)
                    {
                        case SlotType.BAG:

                            SwapItem();

                        break;

                        case SlotType.WEAPON:

                            if(currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.WEAPON)
                             {
                                SwapItem(); 
                            }

                        break;

                        case SlotType.ARMOR:
                            if(currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.ARMOR)
                            {
                                SwapItem(); 
                            }

                        break;

                        case SlotType.ACTION:

                            if(currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.USEABLE)
                            {
                                SwapItem(); 
                            }

                        break;
                    }
                
                
                currentHolder.UpdateItem();
                targetHolder.UpdateItem();
            }
        }
        transform.SetParent(InventoryManagement.Instance.currentDrag.originalParent);

        RectTransform t = transform as RectTransform;

        t.offsetMax = -Vector2.one * 5;
        t.offsetMin = Vector2.one * 5;
    }
    public void SwapItem()
    {   
        var targetItem = targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index];
        var tempItem = currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index];

        bool isSameItem = tempItem.itemData == targetItem.itemData;

        if(isSameItem && targetItem.itemData.isStackable)
        {
            targetItem.amount += tempItem.amount;
            tempItem.itemData = null;
            tempItem.amount = 0;
        }
        else
        {
            currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index] = targetItem;
            targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index] = tempItem;
        }

    }
}
