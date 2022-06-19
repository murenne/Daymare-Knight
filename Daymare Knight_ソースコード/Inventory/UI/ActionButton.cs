using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public KeyCode actionKey;
    private SlotHolder currnetSlotHolder;

    void Awake() 
    {
        currnetSlotHolder = GetComponent<SlotHolder>();
    }

    void Update() 
    {
        if(Input.GetKeyDown(actionKey) && currnetSlotHolder.itemUI.GetItem())
        {
            currnetSlotHolder.UseItem();
        }

    }   
}
