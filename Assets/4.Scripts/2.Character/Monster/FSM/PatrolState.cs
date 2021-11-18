using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyBaseState
{
    public override void EnterState(EnemyController enemy)
    {
      
        enemy.agent.speed = enemy.InitSpeed;
        enemy.isAttack = false;
        enemy.agent.isStopped = false;//用于不是攻击状态下突然进入，player死亡
    }

    public override void OnUpdate(EnemyController enemy)
    {
     

        if (Vector3.Distance(enemy.wayPoint, enemy.transform.position) <= enemy.agent.stoppingDistance)
        {



            enemy.isWalk = false;
            if (enemy.remainLookAt>=0)
            {
            enemy.remainLookAt -= Time.deltaTime;

            }
            else   enemy.GetNewWayPoint();
           
          
        }

        else {
         
            
                enemy.isWalk = true;
            enemy.agent.SetDestination(enemy.wayPoint);
           
        }
    }
}
