using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyBaseState
{
    public override void EnterState(EnemyController enemy)
    {
        Debug.Log("����Ѳ��״̬");
        enemy.agent.speed = enemy.InitSpeed;
    }

    public override void OnUpdate(EnemyController enemy)
    {
        
    }
}
