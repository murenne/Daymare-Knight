using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestRequirement : MonoBehaviour
{
    private Text requireName;
    private Text ProgressNumber;

    void Awake() {
        requireName = GetComponent<Text>();
        ProgressNumber = transform.GetChild(0).GetComponent<Text>();
    }

    public void SetUpRequirement(string name,int amount,int currentAmount)
    {
        requireName.text = name;
        ProgressNumber.text = currentAmount.ToString() + " / " + amount.ToString();

    }
}
