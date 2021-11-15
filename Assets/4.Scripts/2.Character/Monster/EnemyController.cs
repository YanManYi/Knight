using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BoxCollider))]

public class EnemyController : MonoBehaviour
{

    private  EnemyBaseState currentState;
    [Header("是否要巡逻")]
    public bool isPatrol = false;

    #region 状态切换

    public GuardState guardState = new GuardState();
    public PatrolState patrolState = new PatrolState();
    public AttackState attackState = new AttackState();
    public DeadState deadState = new DeadState();
       
    /// <summary>
    /// 状态切换调用
    /// </summary>
    public void TransitionToState(EnemyBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);//相当于Start函数
    }
    #endregion

    [Header("可视范围")]
    public float sightRadius;
    [Header("巡逻范围")]
    public float patrolRadius;
    [Header("持续眺望时间")]
    public float durationLookAt;
    [HideInInspector ]
    public float remainLookAt;
    [HideInInspector]
    public  float lastAttackTime;
    [HideInInspector]
    public  Vector3 wayPoint, InitPoint;//巡逻点，初始点
    [HideInInspector]
    public Quaternion InitRotation;//初始角度


    [HideInInspector]
    public  NavMeshAgent agent;
    [HideInInspector]
    public  CharacterStats characterStats;//里面包含两个ScriptableObject数据读取出来的属性

    [HideInInspector]
    public  Animator anim;
    [HideInInspector]
   public  bool isWalk, isAttack, isFollow;
    [HideInInspector]
    public bool isDie;
    /// <summary>
    /// 动画切换，Update
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
            //问题：这里的会重复进入状态,解决：&&currentState!=attackState解决
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
        //返回一个最近网格上的点
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
