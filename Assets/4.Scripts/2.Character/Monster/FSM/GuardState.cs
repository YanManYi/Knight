using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardState : EnemyBaseState
{
    public override void EnterState(EnemyController enemy)
    {
        Debug.Log("����վ׮״̬");
        enemy.agent.speed=enemy.InitSpeed;
    }

    public override void OnUpdate(EnemyController enemy)
    {
       
    }
}
