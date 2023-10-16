using UnityEngine;

public abstract class BaseState
{

    public abstract void EnterState(EnemyBase enemy);

    public abstract void StateUpdate(EnemyBase enemy);
}
