using UnityEngine;

public class Unit : Character
{
    public override void Init(CharacterData data) 
    {
        base.Init(data); 
        StateMachine.AddState(EState.Attack, new UnitAttackState(this));
    }
}
