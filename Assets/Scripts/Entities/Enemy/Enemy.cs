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

    public override void Die()
    {
        CurRoom.RemoveEnemy(this);
        Main.Get<StageManager>().CheckClear();
        base.Die();
    }
}
