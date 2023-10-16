using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackState : BaseState
{
    public override void EnterState(EnemyBase enemy)
    {
        enemy.animState= 2; //进入状态后警戒一次
    }

    public override void StateUpdate(EnemyBase enemy)
    {
        //就近寻敌
        foreach (var item in enemy.targetList)
        {
            if(enemy.LastSkill + enemy.SkillRate > Time.time)
            {
                if (item.CompareTag("Player"))
                {
                    enemy.target = item;
                    break;
                }
                if (Math.Abs(item.transform.position.x - enemy.transform.position.x) < Math.Abs(enemy.target.position.x - enemy.transform.position.x))
                {
                    enemy.target = item;
                }
            }else if (Math.Abs(item.transform.position.x - enemy.transform.position.x) < Math.Abs(enemy.target.position.x - enemy.transform.position.x))
            {
                enemy.target = item;
            }
        }
        //无敌人退出状态
        if (enemy.targetList.Count < 1)
        {
            enemy.animState = 0;
            enemy.StateTransforTo(enemy.patrol);
        }
        //追击完成时
        if (Vector2.Distance(enemy.transform.position,enemy.target.position) <= enemy.distenceToTarget)
        {
            if (enemy.target.CompareTag("Player") && Time.time > enemy.LastAttack + enemy.AttackRate)
            {
                enemy.Attack();
            }
            if (enemy.target.CompareTag("Bomb") && Time.time > enemy.LastSkill + enemy.SkillRate)
            {
                enemy.Skill();
            }
            enemy.animState = 0; //CD状态不追击
        }
        //追击状态
        if (!enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("warn") && enemy.targetList.Count > 0 && Vector2.Distance(enemy.transform.position, enemy.target.position) > enemy.distenceToTarget)
        {
            enemy.target = enemy.targetList[0];
            enemy.animState = 1;
            enemy.MoveToTarget();
        }
    }
}
