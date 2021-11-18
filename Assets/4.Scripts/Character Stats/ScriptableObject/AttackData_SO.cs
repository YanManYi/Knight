using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Attack", menuName = "Character Stats/Attack")]
public class AttackData_SO : ScriptableObject
{


    public float attackRange;
    [Header("Monster有远程攻击的情况赋值，还需要修改状态机里的Attack02条件")]
    public float skillRange;
    [Space]
    public float coolDown;

    //public int minDamage;
    //public int maxDamage;
    [Header("攻击数值")]
    public int baseAttackDamage;//攻击伤害
    [Header("数值波动范围")]
    public int changeRange;

    [Space]

    public float criticalMultiplier;//暴击改变的数值
    public float criticalChance;//暴击概率

    

}
