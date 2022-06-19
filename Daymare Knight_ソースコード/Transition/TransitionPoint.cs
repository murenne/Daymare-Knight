using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPoint : MonoBehaviour
{
    public enum TransitionType{ SameScene, DifferentScene }
    [Header("Transition Infomation")]
    public string sceneName;
    public TransitionType transitionType;
    public TransitionDestination.DestinationTag destinationTag;
    private bool canTrans;

    void Update() 
    {
        if(Input.GetKeyDown(KeyCode.W)&& canTrans)
        {
            SceneController.Instance.TransitionToDestination(this);
        }
    }

    void OnTriggerStay(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            canTrans = true;
        }
        
    }

    void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            canTrans = false;
        }
        
    }
}
