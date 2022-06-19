using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;
using System;
using UnityEngine.EventSystems;

//[System.Serializable]
//public class EventVector3 : UnityEvent<Vector3>{}
public class MouseManagement : Singleton<MouseManagement> //MonoBehaviour
{
    //public static MouseManagement Instance; 改用泛型单例

    public Texture2D point,doorway,attack,target,arrow;
    RaycastHit hitInfo;

    //public EventVector3 onMouseClicked;
    public event Action<Vector3> onMouseClicked;
    public event Action<GameObject> onEnemyClicked;

    //void Awake() 
    //    if(Instance != null)
    //    {
    //        Destroy(gameObject);
    //    }
    //    else 
    //    {
    //        Instance = this;
    //    }
    //}

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    void Update() 
    {
        SetCursorTexture();

        if(InteractWithUI())
        {
            return;
        }
        
        MouseControl();
    }
    

    void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input .mousePosition);

        if(Physics.Raycast(ray, out hitInfo))
        {
            //切换贴图
            switch(hitInfo.collider.gameObject.tag)
            {

                default:
                Cursor.SetCursor(arrow, new Vector2(16,16),CursorMode.Auto);
                break;

                case "Ground":
                Cursor.SetCursor(target,new Vector2(16,16),CursorMode.Auto);
                break;

                case "Enemy":
                Cursor.SetCursor(attack,new Vector2(16,16),CursorMode.Auto);
                break;

                case "Portal":
                Cursor.SetCursor(doorway,new Vector2(16,16),CursorMode.Auto);
                break;

                case "Item":
                Cursor.SetCursor(point,new Vector2(16,16),CursorMode.Auto);
                break;



            }
        }

    }

    void MouseControl()
    {
        if(Input .GetMouseButtonDown(0) && hitInfo.collider != null)
        {
            if(hitInfo.collider.gameObject.CompareTag("Ground"))
            {
                onMouseClicked?.Invoke(hitInfo.point);
            }
             if(hitInfo.collider.gameObject.CompareTag("Enemy"))
            {
                onEnemyClicked?.Invoke(hitInfo.collider.gameObject);
            }
             if(hitInfo.collider.gameObject.CompareTag("Attackable"))
            {
                onEnemyClicked?.Invoke(hitInfo.collider.gameObject);
            }
            if(hitInfo.collider.gameObject.CompareTag("Portal"))
            {
                onMouseClicked?.Invoke(hitInfo.point);
            }
            if(hitInfo.collider.gameObject.CompareTag("Item"))
            {
                onMouseClicked?.Invoke(hitInfo.point);
            }
            
        }
        
    }

    bool InteractWithUI()
    {
        if(EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
