using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterStats : MonoBehaviour
{
    public event Action<int, int> UpdateHealthBarOnAttack;


    public CharacterData_SO templateData;
    [Header("����Null״̬")]
    public CharacterData_SO characterData;//�ȴ�����ֵ�����ֱ����Ӿͻ���ʵ�޸�CharacterStats�ļ�����
    [Space]
    [Header("Monster�ֲ�����������壬���п���")]
    public AttackData_SO TempAttackData;
    [Header("Player����Null״̬")]
    public AttackData_SO attackData;

    [HideInInspector]
    public bool isCritical;


    #region characterData��ȡ����
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

    #region attackData��ȡ����
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



    GameObject doTweenUICanvasList;
    GameObject prefab_KillDamageCanvas;


    private void Awake()
    {
        if (templateData != null) characterData = Instantiate(templateData);
        if (TempAttackData != null) attackData = Instantiate(TempAttackData);

        characterData.attackData = attackData;//������


        //ÿ������һ��ʼ����ȡ��һ��
        doTweenUICanvasList = GameObject.FindGameObjectWithTag("DoTweenUICanvasList");
        prefab_KillDamageCanvas = Resources.Load<GameObject>("DoTweenUICanvas/KillDamageCanvas");
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="defener"></param>
    public void TakeDamage(CharacterStats attacker, CharacterStats defener)
    {

        int damage = Mathf.Max(attacker.CurrentDamage(attacker) - defener.CurrentDefence, 0);

        defener.CurrentHealth = Mathf.Max(defener.CurrentHealth - damage, 0);


        if (attacker.isCritical)
        {
            defener.GetComponent<Animator>().SetTrigger("Hit");
            defener.GetComponent<NavMeshAgent>().isStopped = true;
            defener.GetComponent<NavMeshAgent>().velocity = (attacker.transform.position - defener.transform.position).normalized * -10;


            defener.transform.LookAt(attacker.transform);
        }




        //TODO: Update UI,����,DoTweenUI

        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);

        if (CurrentHealth <= 0)
        {
            attacker.characterData.UpdateExp(characterData.killScore);
        }

        //�����
        if (doTweenUICanvasList)
        {
            if (doTweenUICanvasList.transform.childCount > 0)
            {

                GameObject gg = ChildObj(doTweenUICanvasList);
                if (gg)
                {
                    gg.SetActive(true);

                    gg.GetComponent<KillDamageCanvas>().DamageActive(attacker, defener, damage);
                }
                else { Instantiate(prefab_KillDamageCanvas, GameObject.FindGameObjectWithTag("DoTweenUICanvasList").transform).GetComponent<KillDamageCanvas>().DamageActive(attacker, defener, damage); }

            }


            else
            {

                Instantiate(prefab_KillDamageCanvas, GameObject.FindGameObjectWithTag("DoTweenUICanvasList").transform).GetComponent<KillDamageCanvas>().DamageActive(attacker, defener, damage);


            }



        }












    }

    private int CurrentDamage(CharacterStats attacker)
    {
        float coreDamage = attacker.AttackDamage;

        if (isCritical)
        {
            
            coreDamage *= attacker.CriticalMultiplier;

        }

        return (int)coreDamage;
    }





    public GameObject ChildObj(GameObject doTweenUICanvasList)
    {

        for (int i = 0; i < doTweenUICanvasList.transform.childCount; i++)
        {

            if (doTweenUICanvasList.transform.GetChild(i).gameObject.activeSelf == false)
            {
                return doTweenUICanvasList.transform.GetChild(i).gameObject;

            }


        }
        return null;

    }
}
