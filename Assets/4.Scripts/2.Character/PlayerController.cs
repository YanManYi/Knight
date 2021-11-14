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
    

    private void Awake()
    {  
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        //TODO
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
        MouseManager.Instance.OnEnemyClicked += EventAttack;
    }

    private void Update()
    {
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
        while (Vector3.Distance(attackTarget.transform.position, transform.position) > 2)
        {

            agent.SetDestination(attackTarget.transform.position);
            yield return null;
        }


       




        //Attack
        agent.isStopped = true;

        if (lastAttackTime<=0)
        {
            //TODO:暂时没有攻击cd数据
            lastAttackTime = 1;

            anim.SetTrigger("Attack01");
        }

        yield break;
        
    }





   

    private void SwitchAnimation()
    {
        anim.SetFloat("Speed", agent.velocity.magnitude);

    }
}
