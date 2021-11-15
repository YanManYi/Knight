using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Attack", menuName = "Character Stats/Attack")]
public class AttackData_SO : ScriptableObject
{


    public float attackRange;
    public float skillRange;
    public float coolDown;

    //public int minDamage;
    //public int maxDamage;
    [Header("������ֵ")]
    public int baseAttackDamage;//�����˺�
    [Header("��ֵ������Χ")]
    public int changeRange;

    [Space]

    public float criticalMultiplier;//�����ı����ֵ
    public float criticalChance;//��������

    

}
