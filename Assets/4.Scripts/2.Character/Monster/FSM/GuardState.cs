using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardState : EnemyBaseState
{
    public override void EnterState(EnemyController enemy)
    {
       
        enemy.agent.speed=enemy.InitSpeed;
        enemy.isAttack = false;

        enemy.isWalk = true;
        enemy.agent.SetDestination(enemy.InitPoint);
        enemy.agent.isStopped = false;//用于不是攻击状态下突然进入，player死亡
    }

    public override void OnUpdate(EnemyController enemy)
    {
        Debug.Log("进入站桩状态");
        if (Vector3.Distance(enemy.InitPoint, enemy.transform.position) <= enemy.agent.stoppingDistance)
        {

            enemy.isWalk = false;

            enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation,enemy.InitRotation,0.01f);

        }
    }
}
