using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : Singleton<QuestUI>
{
    [Header("Elements")]
    public GameObject questPanel;
    public ItemTooltip tooltip;
    bool isOpen;

    [Header("Quest Name")]
    public RectTransform questListTransform;
    public QuestNameButton questNameButton;

    [Header("Text Content")]
    public Text questContentText;

    [Header("Requirement")]
    public RectTransform requireTransform;
    public QuestRequirement requirement;

    [Header("Reward")]
    public RectTransform rewardTransform;
    public ItemUI rewardUI;

    void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            isOpen = !isOpen;
            questPanel.SetActive(isOpen);
            questContentText.text = "";
            SetUpQuestList();

            if(!isOpen)
            {
                tooltip.gameObject.SetActive(false);
            }
        }
    }

    public void  SetUpQuestList()
    {
        foreach (Transform item in questListTransform)
        {
            Destroy(item.gameObject);
        }

        foreach (Transform item in rewardTransform)
        {
            Destroy(item.gameObject);
        }

        foreach (Transform item in requireTransform)
        {
            Destroy(item.gameObject);
        }

        foreach (var task in QuestManagement.Instance.tasks)
        {
            var newTask = Instantiate(questNameButton,questListTransform);
            newTask.SetUpNameButton(task.questData);
            newTask.questContentText = questContentText;
        }
    }

        public void SetUpRequireList(QuestData_SO questData)
    {
        foreach (Transform item in requireTransform)
        {
            Destroy(item.gameObject);
        }

        foreach (var require in questData.questRequires)
        {
            var q = Instantiate(requirement,requireTransform);
            q.SetUpRequirement(require.Name,require.requireAmount,require.currentAmount);
        }
    }

    public void SetUpRewardItem(ItemData_SO itemData, int amount)
    {
        var item = Instantiate(rewardUI,rewardTransform);
        item.SetUpItemUI(itemData,amount);
    }
}
