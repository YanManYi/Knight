using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState :EnemyBaseState
{
    public override void EnterState(EnemyController enemy)
    {
        Debug.Log("����״̬");
        enemy.agent.speed *= 1.5f;
    }

    public override void OnUpdate(EnemyController enemy)
    {

        if (enemy.attackTarget)
        {
            enemy.agent.SetDestination(enemy.attackTarget.transform.position);

        }
        else
        {
            //���ѻص���һ����ʼ״̬Guard����Patrol
            if (enemy.isPatrol)
                enemy.TransitionToState(enemy.patrolState);
            else
                enemy.TransitionToState(enemy.guardState);

        }
    }


}
