using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState :EnemyBaseState
{
    public override void EnterState(EnemyController enemy)
    {
        Debug.Log("����״̬");
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
            //���Ŷ����������ƶ�
            if(enemy.anim.GetCurrentAnimatorStateInfo(1).IsName("Run"))
            enemy.agent.isStopped = false;
            enemy.agent.SetDestination(enemy.attackTarget.transform.position);

            //�ڹ�������Զ�̹�����Χ��
            if (enemy.TargetInAttackRange()||enemy.TargetInSkillRange())
            {

                enemy.isFollow = false;
                enemy.agent.isStopped = true;

                if (enemy.lastAttackTime <= 0)
                {

                    enemy.lastAttackTime = enemy.characterStats.CoolDown;
                  
                    //�����ж�

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

            //���ѻص���һ����ʼ״̬Guard����Patrol
            if (enemy.isPatrol)
                enemy.TransitionToState(enemy.patrolState);
            else
                enemy.TransitionToState(enemy.guardState);

        }
    }

    /// <summary>
    /// ÿ��ֻһִ֡��
    /// </summary>
    /// <param name="enemy"></param>
    public void EventAttack(EnemyController enemy)
    {
        enemy.transform.LookAt(enemy.attackTarget.transform);

        if (enemy.TargetInAttackRange())
        {
            //�����빥��
            enemy.anim.SetTrigger("Skill01");
        }

        if (enemy.TargetInSkillRange())
        {

            //Զ���빥��
            enemy.anim.SetTrigger("Skill02");

        }


        //ע�ͣ�������Զ�����ֵ��ˣ����е�monsterû��Զ�̹�����ô�죬�Ǿͱ����������ƣ����ŵڶ��ֶ���
        //ע��һ������ͬʱ��������Ŷ��һ���ܶ�һ���Ǳ����жϡ�
        //����Ӵ=��Զ���Ĺ������ܣ���Ҫע��ͬʱ����ľ������������Ǹ�״̬�����Solo,������һ����

    }



}
