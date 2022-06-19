using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManagement : Singleton<InventoryManagement>
{
    public class DragData
    {
        public SlotHolder originalHolder;
        public RectTransform originalParent;
    }

    [Header("Inventory Data")]
    public InventoryData_SO inventoryTemplate;
    public InventoryData_SO inventoryData;
    public InventoryData_SO actionTemplate;
    public InventoryData_SO actionData;
    public InventoryData_SO equipmentTemplate;
    public InventoryData_SO equipmentData;

    [Header("ContainerS")]
    public ContainerUI inventoryUI;
    public ContainerUI actionUI;
    public ContainerUI equipmentUI;

    [Header("Drag Canvas")]
    public Canvas dragCanvas;
    public DragData currentDrag;
    


    [Header("UI Panel")]
    bool isOpen = false;
    public GameObject bagPanel;
    public GameObject statesPanel;

    [Header("States Text")]
    public Text healthText;
    public Text attackText;

    [Header("Tooltip")]
    public ItemTooltip tooltip;

    protected override void Awake()
    {
        base.Awake();
        if(inventoryTemplate != null)
        {
            inventoryData = Instantiate(inventoryTemplate);
        }
        if(actionTemplate != null)
        {
            actionData = Instantiate(actionTemplate);
        }
        if(equipmentTemplate != null)
        {
            equipmentData = Instantiate(equipmentTemplate);
        }
    }

    void Start() 
    {
        LoadData();
        inventoryUI.RefreshUI();
        actionUI.RefreshUI();
        equipmentUI.RefreshUI();
    }
    void Update() 
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            isOpen = !isOpen;
            bagPanel.SetActive(isOpen);
            statesPanel.SetActive(isOpen);
        }

        UpadateStateText(GameManager.Instance.playerStates.MaxHealth,(int)GameManager.Instance.playerStates.attackData.minDamge,(int)GameManager.Instance.playerStates.attackData.maxDamge);
    }

    public void SaveData()
    {
        SaveManagement.Instance.Save(inventoryData,inventoryData.name);
        SaveManagement.Instance.Save(actionData,actionData.name);
        SaveManagement.Instance.Save(equipmentData,equipmentData.name);
    }

    public void LoadData()
    {
        SaveManagement.Instance.Load(inventoryData,inventoryData.name);
        SaveManagement.Instance.Load(actionData,actionData.name);
        SaveManagement.Instance.Load(equipmentData,equipmentData.name);
    }

    public void UpadateStateText(int health, int minattack, int maxattack)
    {
        healthText.text = health.ToString();
        attackText.text = minattack + " - " + maxattack;
    }

    #region 检查拖拽物品是否在每一个slot范围内

    public bool CheckInInventoryUI(Vector3 position)
    {
        for(int i = 0; i < inventoryUI.slotHolders.Length; i ++)
        {
            RectTransform t = (RectTransform)inventoryUI.slotHolders[i].transform;

            if(RectTransformUtility.RectangleContainsScreenPoint(t,position))
            {
                return true;
            }
        }

        return false;
    }

        public bool CheckInActionUI(Vector3 position)
    {
        for(int i = 0; i < actionUI.slotHolders.Length; i ++)
        {
            RectTransform t = (RectTransform)actionUI.slotHolders[i].transform;

            if(RectTransformUtility.RectangleContainsScreenPoint(t,position))
            {
                return true;
            }
        }
        
        return false;
    }

        public bool CheckInEquipmentUI(Vector3 position)
    {
        for(int i = 0; i < equipmentUI.slotHolders.Length; i ++)
        {
            RectTransform t = (RectTransform)equipmentUI.slotHolders[i].transform;

            if(RectTransformUtility.RectangleContainsScreenPoint(t,position))
            {
                return true;
            }
        }
        
        return false;
    }
    



    #endregion
}
