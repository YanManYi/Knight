using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{

    private NavMeshAgent agent;
    private Animator anim;
    private GameObject   attackTarget;

    private float lastAttackTime;

    private CharacterStats characterStats;//里面包含两个ScriptableObject数据读取出来的属性

    private bool isDie;

    private void Awake()
    {  
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
    }

    private void Start()
    {
        //TODO
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
        MouseManager.Instance.OnEnemyClicked += EventAttack;
    }

    private void Update()
    {
        isDie = characterStats.CurrentHealth == 0;
        if (isDie)
        {
            GetComponent<BoxCollider>().enabled = false;           
        }
        SwitchAnimation();
      lastAttackTime  -= Time.deltaTime;
    }

    /// <summary>
    /// 订阅移动的事件
    /// </summary>
    /// <param name="targetPoint">地面坐标</param>
    public void MoveToTarget(Vector3 target)
    {
        StopCoroutine("MoveToAttackTarget");
        agent.isStopped = false;
        agent.SetDestination(target);
    }

    /// <summary>
    /// 订阅攻击的事件
    /// </summary>
    /// <param name="attackTarget">攻击目标</param>
    public void EventAttack(GameObject target)
    {
        if (target)
        {

            attackTarget = target;
          
            StartCoroutine("MoveToAttackTarget");

          
        }
    }



    IEnumerator MoveToAttackTarget()
    {
       
        agent.isStopped = false;

        //  transform.LookAt(attackTarget.transform);
        Vector3 target = (attackTarget.transform.position - transform.position).normalized;

        //原因：在结束循环时候不好判断接近值，lerp最后插值他们不会相等，无法跳出while循环，这很烦。
        //解决：利用点乘的绝对值和四象限的cos的值比较大小，人物相差角度越小，dot值越大
        while (Mathf.Abs(Vector3.Dot(transform.forward, target)) <= 0.95f)
        {
            transform.forward = Vector3.Lerp(transform.forward, target, 0.1f);
            yield return null;
        }

        //TODO:根据武器长度修改攻击距离
        while (Vector3.Distance(attackTarget.transform.position, transform.position) > characterStats .AttackRange)
        {

            agent.SetDestination(attackTarget.transform.position);
            yield return null;
        }


       




        //Attack
        agent.isStopped = true;

        if (lastAttackTime<=0)
        {
           
            lastAttackTime = characterStats.CoolDown;

            //暴击判断
           characterStats.isCritical = Random.value <= characterStats.CriticalChance;
            anim.SetBool("Critical",characterStats.isCritical);

            anim.SetTrigger("Skill01");           
            anim.SetTrigger("Skill02");
          


        }

        yield break;
        
    }





   

    private void SwitchAnimation()
    {
        anim.SetFloat("Speed", agent.velocity.magnitude);
        anim.SetBool("Die", isDie);
    }


    //Animation event
    void Hit()
    {
        CharacterStats targetStats = attackTarget.GetComponent<CharacterStats>();


        targetStats.TakeDamage(characterStats,targetStats);
    }


}
