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
    /// �����ƶ����¼�
    /// </summary>
    /// <param name="targetPoint">��������</param>
    public void MoveToTarget(Vector3 target)
    {
        StopCoroutine("MoveToAttackTarget");
        agent.isStopped = false;
        agent.SetDestination(target);
    }

    /// <summary>
    /// ���Ĺ������¼�
    /// </summary>
    /// <param name="attackTarget">����Ŀ��</param>
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

        //ԭ���ڽ���ѭ��ʱ�򲻺��жϽӽ�ֵ��lerp����ֵ���ǲ�����ȣ��޷�����whileѭ������ܷ���
        //��������õ�˵ľ���ֵ�������޵�cos��ֵ�Ƚϴ�С���������Ƕ�ԽС��dotֵԽ��
        while (Mathf.Abs(Vector3.Dot(transform.forward, target)) <= 0.95f)
        {
            transform.forward = Vector3.Lerp(transform.forward, target, 0.1f);
            yield return null;
        }

        //TODO:�������������޸Ĺ�������
        while (Vector3.Distance(attackTarget.transform.position, transform.position) > 2)
        {

            agent.SetDestination(attackTarget.transform.position);
            yield return null;
        }


       




        //Attack
        agent.isStopped = true;

        if (lastAttackTime<=0)
        {
            //TODO:��ʱû�й���cd����
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
