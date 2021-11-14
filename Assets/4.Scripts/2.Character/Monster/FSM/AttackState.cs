using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState :EnemyBaseState
{
    public override void EnterState(EnemyController enemy)
    {
        Debug.Log("攻击状态");
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
            //拉脱回到上一个初始状态Guard或者Patrol
            if (enemy.isPatrol)
                enemy.TransitionToState(enemy.patrolState);
            else
                enemy.TransitionToState(enemy.guardState);

        }
    }


}
