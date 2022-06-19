using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{

    public ItemData_SO itemData;
  void OnTriggerEnter(Collider other) {
      
      if(other.CompareTag("Player"))
      {
          InventoryManagement.Instance.inventoryData.AddItems(itemData,itemData.itemAmount);
          InventoryManagement.Instance.inventoryUI.RefreshUI();

        //GameManager.Instance.playerStates.EquipWeapon(itemData);//直接装备武器

        Destroy(gameObject);
      }
  }
}
