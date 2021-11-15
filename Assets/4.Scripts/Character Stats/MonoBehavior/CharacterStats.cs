using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterData_SO characterData;
    public AttackData_SO attackData;

    [HideInInspector]
    public bool isCritical;

    #region characterData读取数据
    public int MaxHealth
    {
        get { if (characterData) return characterData.maxHealth; else return 0; }
        set { characterData.maxHealth = value; }
    }

    public int CurrentHealth
    {
        get { if (characterData) return characterData.currentHealth; else return 0; }
        set { characterData.currentHealth = value; }
    }
    public int BaseDefence
    {
        get { if (characterData) return characterData.baseDefence; else return 0; }
        set { characterData.baseDefence = value; }
    }
    public int CurrentDefence
    {
        get { if (characterData) return characterData.currentDefence; else return 0; }
        set { characterData.currentDefence = value; }
    }
    #endregion

    #region attackData读取数据
    public float AttackRange
    {
        get { if (attackData) return attackData.attackRange; else return 0; }
        set { attackData.attackRange = value; }
    }
    public float SkillRange
    {
        get { if (attackData) return attackData.skillRange; else return 0; }
        set { attackData.skillRange = value; }
    }

    public float CoolDown
    {
        get { if (attackData) return attackData.coolDown; else return 0; }
        set { attackData.coolDown = value; }
    }

    public int AttackDamage
    {
        get
        {
            if (attackData)
            {
                int damage = UnityEngine.Random.Range(attackData.baseAttackDamage - attackData.changeRange, attackData.baseAttackDamage + attackData.changeRange);
                return damage;
            }
            else return 0;

        }

        set { attackData.baseAttackDamage = value; }

    }


    public float CriticalMultiplier
    {
        get { if (attackData) return attackData.criticalMultiplier; else return 0; }
        set { attackData.criticalMultiplier = value; }
    }
    public float CriticalChance
    {
        get { if (attackData) return attackData.criticalChance; else return 0; }
        set { attackData.criticalChance = value; }
    }

    #endregion



    public void TakeDamage(CharacterStats attacker,CharacterStats Defener)
    {

        int damage = Mathf.Max(attacker.CurrentDamage(attacker) - Defener.CurrentDefence,0);

        CurrentHealth = Mathf.Max(CurrentHealth-damage,0);

        if (attacker.isCritical) {

            Defener.GetComponent<Animator>().SetTrigger("Hit");
        }


        
        //TODO: Update UI,经验
        
    }

    private int CurrentDamage(CharacterStats attacker)
    {
        int  coreDamage = attacker.AttackDamage;

        if (isCritical)
        {
            Debug.Log("暴击");
            coreDamage *= (int)attacker.CriticalMultiplier;
        }

        return coreDamage;
    }
}
