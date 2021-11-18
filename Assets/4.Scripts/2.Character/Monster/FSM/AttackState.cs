using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState :EnemyBaseState
{
    public override void EnterState(EnemyController enemy)
    {
        Debug.Log("攻击状态");
        enemy.agent.speed *= 1.5f;
        enemy.isWalk = true;
        enemy.isAttack = true;
        enemy.remainLookAt = enemy.durationLookAt;
    }

    public override void OnUpdate(EnemyController enemy)
    {

        if (enemy.attackTarget&& enemy.agent)
        {
            enemy.isFollow = true;
            //播放动画不可以移动
            if(enemy.anim.GetCurrentAnimatorStateInfo(1).IsName("Run"))
            enemy.agent.isStopped = false;
            enemy.agent.SetDestination(enemy.attackTarget.transform.position);

            //在攻击或者远程攻击范围内
            if (enemy.TargetInAttackRange()||enemy.TargetInSkillRange())
            {

                enemy.isFollow = false;
                enemy.agent.isStopped = true;

                if (enemy.lastAttackTime <= 0)
                {

                    enemy.lastAttackTime = enemy.characterStats.CoolDown;
                  
                    //暴击判断

                    enemy.characterStats.isCritical = Random.value <= enemy.characterStats.CriticalChance;

                    EventAttack(enemy);

                }
           


            }

        }
        else
        {
            enemy.isFollow = false;
           
            if (enemy.remainLookAt >= 0)
            {
                enemy.remainLookAt -= Time.deltaTime;
                enemy.agent.SetDestination(enemy.transform.position);

            }
            else 

            //拉脱回到上一个初始状态Guard或者Patrol
            if (enemy.isPatrol)
                enemy.TransitionToState(enemy.patrolState);
            else
                enemy.TransitionToState(enemy.guardState);

        }
    }

    /// <summary>
    /// 每次只一帧执行
    /// </summary>
    /// <param name="enemy"></param>
    public void EventAttack(EnemyController enemy)
    {
        enemy.transform.LookAt(enemy.attackTarget.transform);

        if (enemy.TargetInAttackRange())
        {
            //近距离攻击
            enemy.anim.SetTrigger("Skill01");
        }

        if (enemy.TargetInSkillRange())
        {

            //远距离攻击
            enemy.anim.SetTrigger("Skill02");

        }


        //注释：本来分远近两种敌人，但有的monster没有远程攻击怎么办，那就暴击率来控制，播放第二种动画
        //注意一二技能同时满足的情况哦，一技能多一个非暴击判断。
        //敌人哟=有远近的攻击技能，则要注意同时满足的距离的情况，点那个状态机里的Solo,优先走一技能

    }



}
