using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Attack", menuName = "Character Stats/Attack")]
public class AttackData_SO : ScriptableObject
{


    public float attackRange;
    [Header("Monster��Զ�̹����������ֵ������Ҫ�޸�״̬�����Attack02����")]
    public float skillRange;
    [Space]
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
