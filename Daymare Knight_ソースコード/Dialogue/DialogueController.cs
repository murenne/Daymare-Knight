using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public DialogueData_SO currentData;

    bool canTalk = false;

    void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player") && currentData != null)
        {
            canTalk = true;
        }


    }

    void Update() 
    {
        if(canTalk && Input.GetKeyDown(KeyCode.Space))
        {
            OpenDialogue();
        }
    }

    private void OpenDialogue()
    {
        DialogueUI.Instance.UpdateDialogueData(currentData);
        DialogueUI.Instance.UpdateMainDialogue(currentData.dialoguePieces[0]);
    }
}
