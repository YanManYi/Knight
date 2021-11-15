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
    public  Vector3 wayPoint, InitPoint;



    [HideInInspector]
    public  NavMeshAgent agent;


    private  Animator anim;
    [HideInInspector]
   public  bool isWalk, isAttack, isFollow;

    /// <summary>
    /// �����л���Update
    /// </summary>
    void SwitchAnimation()
    {
        anim.SetBool("Walk",isWalk);
        anim.SetBool("Attack",isAttack);
        anim.SetBool("Follow",isFollow);
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
    }

    private void Start()
    {
         GetNewWayPoint();
        InitPoint = transform.position;
        if (isPatrol)
            TransitionToState(patrolState);
        else
            TransitionToState(guardState);
    }
    private void Update()
    {
      
        if (FoundPlayer() && currentState != attackState)
        {
            //���⣺����Ļ��ظ�����״̬,�����&&currentState!=attackState���
            TransitionToState(attackState);
        }

        currentState.OnUpdate(this);

        SwitchAnimation();
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,patrolRadius);
    }
}
