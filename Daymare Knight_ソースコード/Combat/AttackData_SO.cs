using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Data",menuName = "Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    public float attackRange;
    public float SkillRange;
    public float coolDownTime;
    public float minDamge;
    public float maxDamge;
    public float criticalMultiplier;
    public float criticalChance;
    public void ApplyWeaponData(AttackData_SO weapon)
    {
        attackRange = weapon.attackRange;
        SkillRange = weapon.SkillRange;
        coolDownTime = weapon.coolDownTime;

        minDamge = weapon.minDamge;
        maxDamge = weapon.maxDamge;

        criticalMultiplier = weapon.criticalMultiplier;
        criticalChance = weapon.criticalChance;
    }

}
