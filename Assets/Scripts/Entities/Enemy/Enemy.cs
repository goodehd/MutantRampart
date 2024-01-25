using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public override void Init(CharacterData data)
    {
        base.Init(data);
        StateMachine.AddState(EState.Attack, new EnemyAttackState(this));
    }

    protected override void Die()
    {
        CurRoom.RemoveEnemy(this);
        base.Die();
    }
}
