using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyBaseState
{
    public override void EnterState(EnemyController enemy)
    {
        Debug.Log("½øÈëÑ²Âß×´Ì¬");
        enemy.agent.speed = enemy.InitSpeed;
        enemy.isAttack = false;
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
