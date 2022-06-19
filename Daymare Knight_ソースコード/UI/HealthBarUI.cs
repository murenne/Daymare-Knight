using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{

    public GameObject healthBarUI;
    public Transform barPoint;
    public bool isAlwaysVisible;
    public float visibleTime = 5f;
    private float timeLeft;

    Image healthSlider;
    Transform UIBar;
    Transform cameraPoint;

    CharacterStates currentStates; 
    
    void Awake() 
    {
        currentStates = GetComponent<CharacterStates>();
        currentStates.UpdataHealthBarOnAttack += UpdateHealthBar;
    }

    void OnEnable() 
    {
        cameraPoint = Camera.main.transform;

        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if(canvas.renderMode == RenderMode.WorldSpace)//偷懒，如果有多个worldspace会出错
            {
              UIBar =  Instantiate(healthBarUI,canvas.transform).transform;
              healthSlider =  UIBar.GetChild(0).GetComponent<Image>();
              UIBar.gameObject.SetActive(isAlwaysVisible);
            }
            
        }
        
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if(currentHealth <= 0)
        {
            Destroy(UIBar.gameObject);
        }

        UIBar.gameObject.SetActive(true);
        timeLeft = visibleTime;

        float sliderPercent = (float)currentHealth/maxHealth;
        healthSlider.fillAmount = sliderPercent; 
    }

    void LateUpdate() 
    {
        if(UIBar != null)
        {
            UIBar.position = barPoint.position;
            UIBar.forward = -cameraPoint.forward;

            if(timeLeft <= 0 && !isAlwaysVisible)
            {
                UIBar.gameObject.SetActive(false);
            }
            else
            {
                timeLeft -= Time.deltaTime;
            }
        }
        
    }
}
