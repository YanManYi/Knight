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
        enemy.agent.isStopped = false;//���ڲ��ǹ���״̬��ͻȻ���룬player����
    }

    public override void OnUpdate(EnemyController enemy)
    {
        Debug.Log("����վ׮״̬");
        if (Vector3.Distance(enemy.InitPoint, enemy.transform.position) <= enemy.agent.stoppingDistance)
        {

            enemy.isWalk = false;

            enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation,enemy.InitRotation,0.01f);

        }
    }
}
