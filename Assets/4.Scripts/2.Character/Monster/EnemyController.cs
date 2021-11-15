using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BoxCollider))]

public class EnemyController : MonoBehaviour
{

    private  EnemyBaseState currentState;
    [Header("�Ƿ�ҪѲ��")]
    public bool isPatrol = false;

    #region ״̬�л�

    public GuardState guardState = new GuardState();
    public PatrolState patrolState = new PatrolState();
    public AttackState attackState = new AttackState();
    public DeadState deadState = new DeadState();
       
    /// <summary>
    /// ״̬�л�����
    /// </summary>
    public void TransitionToState(EnemyBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);//�൱��Start����
    }
    #endregion

    [Header("���ӷ�Χ")]
    public float sightRadius;
    [Header("Ѳ�߷�Χ")]
    public float patrolRadius;
    [Header("��������ʱ��")]
    public float durationLookAt;
    [HideInInspector ]
    public float remainLookAt;
    [HideInInspector]
    public  float lastAttackTime;
    [HideInInspector]
    public  Vector3 wayPoint, InitPoint;//Ѳ�ߵ㣬��ʼ��
    [HideInInspector]
    public Quaternion InitRotation;//��ʼ�Ƕ�


    [HideInInspector]
    public  NavMeshAgent agent;
    [HideInInspector]
    public  CharacterStats characterStats;//�����������ScriptableObject���ݶ�ȡ����������

    [HideInInspector]
    public  Animator anim;
    [HideInInspector]
   public  bool isWalk, isAttack, isFollow;
    [HideInInspector]
    public bool isDie;
    /// <summary>
    /// �����л���Update
    /// </summary>
    void SwitchAnimation()
    {
        anim.SetBool("Walk",isWalk);
        anim.SetBool("Attack",isAttack);
        anim.SetBool("Follow",isFollow);
        anim.SetBool("Critical", characterStats.isCritical);
        anim.SetBool("Die", isDie);

    }

    [HideInInspector]
    public  GameObject attackTarget;
    [HideInInspector]
    public float InitSpeed;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        InitSpeed = agent.speed;
        characterStats = GetComponent<CharacterStats>();
    }

    private void Start()
    {
         GetNewWayPoint();
        InitPoint = transform.position;
        InitRotation = transform.rotation;


        if (isPatrol)
            TransitionToState(patrolState);
        else
            TransitionToState(guardState);
    }
    private void Update()
    {
        isDie = characterStats.CurrentHealth == 0;
        if (isDie)
        {    
            TransitionToState(deadState);
           
        }
        else 
        if (FoundPlayer() && currentState != attackState)
        {
            //���⣺����Ļ��ظ�����״̬,�����&&currentState!=attackState���
            TransitionToState(attackState);
        }

        currentState.OnUpdate(this);

        SwitchAnimation();
        lastAttackTime -= Time.deltaTime;
    }


    bool FoundPlayer() {

        Collider[] player = Physics.OverlapSphere(transform.position,sightRadius, 1 << LayerMask.NameToLayer("Player"));
       
        foreach (var item in player)
        {
          attackTarget = player[0].gameObject;
          return true;      
        }

        attackTarget = null;
        return false;


    }


   public  void GetNewWayPoint()
    {
       remainLookAt = durationLookAt;
        float randomX = Random.Range(-patrolRadius,patrolRadius);
        float randomZ = Random.Range(-patrolRadius,patrolRadius);


        Vector3 randomPoint = new Vector3(InitPoint.x+ randomX,transform.position.y, InitPoint.z+ randomZ);
        NavMeshHit hit;
        wayPoint = randomPoint;
        //����һ����������ϵĵ�
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRadius, 1) ? hit.position : transform.position;
    }





    public bool TargetInAttackRange()
    {
        if (attackTarget)
        {
            return Vector3.Distance(attackTarget.transform.position,transform.position)<=characterStats.AttackRange;
        }
        return false;
    }
    public bool TargetInSkillRange()
    {
        if (attackTarget)
        {
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.SkillRange;
        }
        return false;

    }



    //Animation event
    void Hit()
    {
        if (attackTarget)
        {
            CharacterStats targetStats = attackTarget.GetComponent<CharacterStats>();

            targetStats.TakeDamage(characterStats, targetStats);

        }




    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,patrolRadius);
    }
}
