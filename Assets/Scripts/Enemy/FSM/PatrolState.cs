
using System;

public class PatrolState : BaseState
{
    public override void EnterState(EnemyBase enemy)
    {
        enemy.SwitchPatrolPoint();
    }

    public override void StateUpdate(EnemyBase enemy)
    {
        enemy.animState = 1;
        enemy.MoveToTarget();
        if(Math.Abs(enemy.transform.position.x - enemy.target.position.x) < enemy.distenceToTarget)
        {
            enemy.animState = 0;
            if (!enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
            {
                enemy.SwitchPatrolPoint();
            }
        }
        if(enemy.targetList.Count > 0)
        {
            enemy.StateTransforTo(enemy.attack);
        }
    }
}
