using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : EnemyBaseState
{
    public override void EnterState(EnemyController enemy)
    {
        enemy.agent.enabled = false;
        enemy.GetComponent<Collider>().enabled = false;

        MonoBehaviour.Destroy(enemy.gameObject,2f);
    }

    public override void OnUpdate(EnemyController enemy)
    {
        
    }
}
