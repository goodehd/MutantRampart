using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterBehaviour
{
    public override void Init(CharacterData data)
    {
        base.Init(data);
        StateMachine.AddState(EState.Attack, new EnemyAttackState(this));
    }

    public override void Die()
    {
        CharacterInfo.CurRoom.RemoveEnemy(this);
        Main.Get<StageManager>().CheckClear();
        base.Die();
    }
}
