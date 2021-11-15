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

    private CharacterStats characterStats;//�����������ScriptableObject���ݶ�ȡ����������

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

            //�����ж�
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
