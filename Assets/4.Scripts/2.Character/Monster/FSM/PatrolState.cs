using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyBaseState
{
    public override void EnterState(EnemyController enemy)
    {
        Debug.Log("½øÈëÑ²Âß×´Ì¬");
        enemy.agent.speed = enemy.InitSpeed;
    }

    public override void OnUpdate(EnemyController enemy)
    {
        
    }
}
