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

    [HideInInspector]
    public  NavMeshAgent agent;
    [HideInInspector]
    public  GameObject attackTarget;
    [HideInInspector]
    public float InitSpeed;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        InitSpeed = agent.speed;
    }

    private void Start()
    {
        if (isPatrol)
            TransitionToState(patrolState);
        else
            TransitionToState(guardState);
    }
    private void Update()
    {
      Debug.Log(FoundPlayer());
        if (FoundPlayer() && currentState != attackState)
        {
            //问题：这里的会重复进入状态,解决：&&currentState!=attackState解决
            TransitionToState(attackState);
        }

        currentState.OnUpdate(this);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }
}
