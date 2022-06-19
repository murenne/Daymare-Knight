using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestNameButton : MonoBehaviour
{
    public Text questNameText;
    public QuestData_SO currentData;
    public Text questContentText;

    void Awake() 
    {
        GetComponent<Button>().onClick.AddListener(UpdateQuestContent);
    }

    void UpdateQuestContent()
    {
        questContentText.text = currentData.description;
        QuestUI.Instance.SetUpRequireList(currentData);

        foreach (Transform item in QuestUI.Instance.rewardTransform)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in currentData.rewards)
        {
            QuestUI.Instance.SetUpRewardItem(item.itemData,item.amount);
        }
        
    }

    public void SetUpNameButton(QuestData_SO questData)
    {
        currentData = questData;

        if(questData.isCompleted)
        {
            questNameText.text = questData.questName + "Completed";
        }
        else
        {
            questNameText.text = questData.questName;
        }
    }


}
