using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New Data",menuName ="Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{

    [Header("Stats Info")]

    public int maxHealth;
    public int currentHealth;

    public int baseDefence;
    public int currentDefence;


    [Header("Monster的经验")]
    public int killScore;

    [Header("PlayerLevel")]


    public int currentLevel;
    public int maxLevel;

    public int baseExp;
    public int currentExp;

    public float levelBuff;
    [HideInInspector]
    public AttackData_SO attackData;

    public float LevelMultiplier { get { return 1 + (currentLevel - 1) * levelBuff; } }

   public  void UpdateExp(int killScore)
    {

        currentExp += killScore;

        if (currentExp >= baseExp) LeveUP();
    }

    private void LeveUP()
    {
        //数据升级
        currentLevel = Mathf.Clamp(currentLevel + 1,0,maxLevel) ;
        baseExp += (int)(baseExp * LevelMultiplier);

        maxHealth *= 2;
        currentHealth = maxHealth;

        baseDefence *= 2;
        currentDefence = baseDefence;

        attackData.baseAttackDamage *=2;
        attackData.changeRange *= 2;
        attackData.criticalChance += 0.05f;
        attackData.criticalMultiplier += 0.2f;
        Debug.Log("升级成功");

    }
}
