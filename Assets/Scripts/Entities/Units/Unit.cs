using UnityEngine;

public class Unit : Character
{
    private void Start()
    {
        Init(Main.Get<DataManager>().unit["Gun"]);
    }

    public override void Init(CharacterData data) 
    {
        base.Init(data); 
        StateMachine.AddState(EState.Attack, new UnitAttackState(this));
    }
}
