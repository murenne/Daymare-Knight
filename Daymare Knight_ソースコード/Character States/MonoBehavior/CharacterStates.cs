using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStates : MonoBehaviour
{
    public event Action<int,int> UpdataHealthBarOnAttack;
    public CharacterData_SO templateData;
    public CharacterData_SO characterData;
    public AttackData_SO attackData;
    private AttackData_SO baseAttackData;
    private RuntimeAnimatorController baseAnimator;

    [Header("Weapon")]
    public Transform weaponSlot;

    [HideInInspector]
    public bool isCritical;


    void Awake() 
    {
        if(templateData != null)
        {
            characterData = Instantiate(templateData); //复制一份数据，避免公用一份数据
        }
        baseAttackData = Instantiate(attackData);
        baseAnimator = GetComponent<Animator>().runtimeAnimatorController;
    }


    #region  read from CharaterData_SO

    public int MaxHealth
    {
        get
        {
            if(characterData != null)
            {
                return characterData.maxHealth;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            characterData.maxHealth = value;
        }
    }

        public int  CurrentHealth
    {
        get
        {
            if(characterData != null)
            {
                return characterData.currentHealth;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            characterData.currentHealth = value;
        }
    }

        public int Basedefence
    {
        get
        {
            if(characterData != null)
            {
                return characterData.baseDefence;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            characterData.baseDefence = value;
        }
    }

        public int  CurrentDefence
    {
        get
        {
            if(characterData != null)
            {
                return characterData.currentDefence;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            characterData.currentDefence = value;
        }
    }
    #endregion

    #region Character Combat

    public void TakeDamage(CharacterStates attacker,CharacterStates defender)
    {
        int damage = Mathf.Max((attacker.currentDamage() - defender.CurrentDefence),1);
        CurrentHealth = Mathf.Max(CurrentHealth - damage , 0);

        if(attacker.isCritical)
        {
            defender.GetComponent<Animator>().SetTrigger("Gethit");
        }

        //UI,LEVEL UP
        UpdataHealthBarOnAttack?.Invoke(CurrentHealth,MaxHealth);

        if(CurrentHealth <= 0 )
        {
            attacker.characterData.UpdateExp(characterData.killPoint);
        }

    }

    public void TakeDamage(int damage,CharacterStates defender) //重载
    {
        int currentDamage = Mathf.Max(damage - defender.CurrentDefence,1);
        CurrentHealth = Mathf.Max(CurrentHealth - currentDamage , 0);
        UpdataHealthBarOnAttack?.Invoke(CurrentHealth,MaxHealth);
        
        if(CurrentHealth <= 0 )
        {
            GameManager.Instance.playerStates.characterData.UpdateExp(characterData.killPoint);
        }
    }

    private int currentDamage()
    {
        float coreDamage = UnityEngine.Random.Range(attackData.maxDamge,attackData.minDamge);
        if(isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
        }
        return (int)coreDamage;
    }





    #endregion

    #region Equip Weapon

    
    public void ChangeWeapon(ItemData_SO weapon)
    {
        UnEquipWeapon();
        EquipWeapon(weapon);
    }
    
    public void EquipWeapon(ItemData_SO weapon)
    {
        if(weapon.weaponPrefab != null)
        {
            Instantiate(weapon.weaponPrefab,weaponSlot);
        }

        attackData.ApplyWeaponData(weapon.weaponAttackData);
        GetComponent<Animator>().runtimeAnimatorController = weapon.weaponAnimator;
        //InventoryManagement.Instance.UpadateStateText(MaxHealth,(int)attackData.minDamge,(int)attackData.maxDamge);更改属性可以在这里调用
    }

    public void UnEquipWeapon()
    {
        if(weaponSlot.transform.childCount !=0)
        {
            for (int i = 0; i < weaponSlot.transform.childCount; i++)
            {
                Destroy(weaponSlot.transform.GetChild(i).gameObject);
            }
        }
        attackData.ApplyWeaponData(baseAttackData);
        GetComponent<Animator>().runtimeAnimatorController = baseAnimator;
    }

    #endregion

    #region Apply Data Change

    
    public void ApplyHealth(int amount)
    {
        if(CurrentHealth + amount <= MaxHealth)
        {
            CurrentHealth += amount;
        }
        else
        {
            CurrentHealth = MaxHealth;
        }
    }


    #endregion





}





