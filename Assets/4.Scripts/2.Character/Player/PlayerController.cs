using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
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

        FindObjectOfType<CinemachineFreeLook>().Follow = transform.DG_FindChild("FollowPoint", transform);
        FindObjectOfType<CinemachineFreeLook>().LookAt= transform.DG_FindChild("FollowPoint", transform); ;

       
    }
    private void OnEnable()
    {
        //TODO
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
        MouseManager.Instance.OnEnemyClicked += EventAttack;

        GameManager.Instance.RigisterPlayer(characterStats);
    }

    private void Start()
    {
   
        SaveManager.Instance.LoadPlayerData();
    }
    private void OnDisable()
    {
        //防止编辑器报错
        if (!GameManager.IsInitialized) return;
        if (MouseManager.Instance)
        {
            //去到另外一个场景需要注销订阅
            MouseManager.Instance.OnMouseClicked -= MoveToTarget;
            MouseManager.Instance.OnEnemyClicked -= EventAttack;
        }
    }

    private void Update()
    {
        isDie = characterStats.CurrentHealth == 0;
        if (isDie)
        {
            GetComponent<BoxCollider>().enabled = false;
           GameManager.Instance.NotifyObserver();
           
        }
        SwitchAnimation();
      lastAttackTime  -= Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            agent.speed =5.5f;
        }
        else
        {
            agent.speed = 4;
        }
    }

    /// <summary>
    /// 订阅移动的事件
    /// </summary>
    /// <param name="targetPoint">地面坐标</param>
    public void MoveToTarget(Vector3 target)
    {
        if (isDie) return;
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
        if (isDie) return;
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
        //解决：利用点乘的绝对值和四象限的cos的值比较大小，人物相差角度越小，dot值越大。 Vector3.Dot(transform.forward, target)<0是考虑背对的时候第一个条件也满足的尴尬，背对攻击
        while (Mathf.Abs(Vector3.Dot(transform.forward, target)) <= 0.95f&& Vector3.Dot(transform.forward, target)<0)
        {
            transform.forward = Vector3.Lerp(transform.forward, target, 0.2f);
            yield return null;
        }
        transform.LookAt(attackTarget.transform);

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
           


            //skill03  
            if (Random.value <= characterStats.CriticalChance * 0.5f)
            {
                anim.SetTrigger("Skill03");
            }
            else {

                anim.SetBool("Critical", characterStats.isCritical);

                anim.SetTrigger("Skill01");
                anim.SetTrigger("Skill02");
            }

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


       
        //如果skill03动画攻击，就需要群攻目标

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
        {
            var v = Physics.OverlapSphere(transform.position, 2.1f, 1 << LayerMask.NameToLayer("Enemy"));
            //360°的敌人都在范围
            for (int i = 0; i < v.Length; i++)
            {
                //在正前方180才造成伤害
                if (Vector3.Dot(transform.forward, (v[i].transform.position - transform.position).normalized) >=0f)
                {
                    targetStats.TakeDamage(characterStats, v[i].GetComponent<CharacterStats>());
                }
            }


        }
        else
        {
            //在正前方120°才造成伤害,Cos60=0.5
            if (Vector3.Dot(transform.forward, (attackTarget.transform.position - transform.position).normalized) >= 0.5f)
                targetStats.TakeDamage(characterStats, targetStats);

        }
    }


}
//playerattack不转头