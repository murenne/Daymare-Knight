using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueUI : Singleton<DialogueUI>
{
    [Header("Basic Elments")]
    public Image icon;
    public Text mainText;
    public Button nextButton;

    public GameObject dialoguePanel;

     [Header("Options")]
    public RectTransform optionPanel;
    public OptionUI optionPrefab;

    [Header("Data")]
    public DialogueData_SO currentData;
    int currentIndex = 0;

    protected override void Awake()
    {
        base.Awake();
        nextButton.onClick.AddListener(ContinueDialogue);
    }

    private void ContinueDialogue()
    {
        if(currentIndex < currentData.dialoguePieces.Count)
        {
            UpdateMainDialogue(currentData.dialoguePieces[currentIndex]);
        }
        else
        {
            dialoguePanel.SetActive(false);
        }

        
    }

    public void UpdateDialogueData(DialogueData_SO data)
    {
        currentData = data;
        currentIndex = 0;
    }

    public void UpdateMainDialogue(DialoguePiece piece)
    {
        dialoguePanel.SetActive(true);
        currentIndex ++;

        if(piece.image != null)
        {
            icon.enabled = true;
            icon.sprite = piece.image;
        }
        else
        {
            icon.enabled = false;
        }

        mainText.text = "";
        //mainText.text = piece.text;
        mainText. DOText (piece.text,2f);


        if(piece.options.Count == 0 && currentData.dialoguePieces.Count > 0 )
        {
            nextButton.interactable = true;
            nextButton.gameObject.SetActive(true);
            nextButton.transform.GetChild(0).gameObject.SetActive(true);
            
        }
        else
        {
            //nextButton.gameObject.SetActive(false);//会改变布局
            nextButton.interactable = false;
            nextButton.transform.GetChild(0).gameObject.SetActive(false);//只把文本设置为false
        }

        //创建options
        CreatOptions(piece);

    }

    void CreatOptions(DialoguePiece piece)
    {
        if(optionPanel.childCount > 0)
        {
            for (int i = 0; i < optionPanel.childCount; i++)
            {
                Destroy(optionPanel.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < piece.options.Count; i++)
        {
            var option = Instantiate(optionPrefab,optionPanel);
            option.UpdateOption(piece,piece.options[i]);
        }
    }
}
