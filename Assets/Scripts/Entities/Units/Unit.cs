using UnityEngine;

public class Unit : CharacterBehaviour
{
    public override void Init(CharacterData data) 
    {
        base.Init(data); 
        StateMachine.AddState(EState.Attack, new UnitAttackState(this));
    }

    public override void Die()
    {
        ((BatRoom)CharacterInfo.CurRoom).UnitCount--;
        base.Die();
    }
}
