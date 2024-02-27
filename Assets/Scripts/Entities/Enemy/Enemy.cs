using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterBehaviour
{
    public override void Init(CharacterData data)
    {
        base.Init(data);
        StateMachine.AddState(EState.Attack, new EnemyAttackState(this));
        StateMachine.AddState(EState.Dead, new EnemyDeadState(this));
    }

    public override void Die()
    {
        if (!CharacterInfo.IsDead)
        {
            Main.Get<StageManager>().CheckClear();
            CharacterInfo.CurRoom.RemoveEnemy(this);
            base.Die();
        }
    }

    public override void ResetCharacter()
    {
        base.ResetCharacter();
        CurPosX = -1;
        CurPosY = -1;
    }

    public void ClearVisited()
    {
        BaseState moveState = StateMachine.GetState(EState.Move);
        ((MoveState)moveState).ClearVisited();
    }
}
