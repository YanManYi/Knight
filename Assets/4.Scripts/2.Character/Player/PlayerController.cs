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

    private CharacterStats characterStats;//�����������ScriptableObject���ݶ�ȡ����������

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
        //��ֹ�༭������
        if (!GameManager.IsInitialized) return;
        if (MouseManager.Instance)
        {
            //ȥ������һ��������Ҫע������
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
    /// �����ƶ����¼�
    /// </summary>
    /// <param name="targetPoint">��������</param>
    public void MoveToTarget(Vector3 target)
    {
        if (isDie) return;
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

        //ԭ���ڽ���ѭ��ʱ�򲻺��жϽӽ�ֵ��lerp����ֵ���ǲ�����ȣ��޷�����whileѭ������ܷ���
        //��������õ�˵ľ���ֵ�������޵�cos��ֵ�Ƚϴ�С���������Ƕ�ԽС��dotֵԽ�� Vector3.Dot(transform.forward, target)<0�ǿ��Ǳ��Ե�ʱ���һ������Ҳ��������Σ����Թ���
        while (Mathf.Abs(Vector3.Dot(transform.forward, target)) <= 0.95f&& Vector3.Dot(transform.forward, target)<0)
        {
            transform.forward = Vector3.Lerp(transform.forward, target, 0.2f);
            yield return null;
        }
        transform.LookAt(attackTarget.transform);

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


       
        //���skill03��������������ҪȺ��Ŀ��

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
        {
            var v = Physics.OverlapSphere(transform.position, 2.1f, 1 << LayerMask.NameToLayer("Enemy"));
            //360��ĵ��˶��ڷ�Χ
            for (int i = 0; i < v.Length; i++)
            {
                //����ǰ��180������˺�
                if (Vector3.Dot(transform.forward, (v[i].transform.position - transform.position).normalized) >=0f)
                {
                    targetStats.TakeDamage(characterStats, v[i].GetComponent<CharacterStats>());
                }
            }


        }
        else
        {
            //����ǰ��120�������˺�,Cos60=0.5
            if (Vector3.Dot(transform.forward, (attackTarget.transform.position - transform.position).normalized) >= 0.5f)
                targetStats.TakeDamage(characterStats, targetStats);

        }
    }


}
//playerattack��תͷ