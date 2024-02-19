using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : BaseState
{
    public EnemyDeadState(CharacterBehaviour owner) : base(owner)
    {
    }

    public override void Init()
    {

    }

    public override void EnterState()
    {
        Owner.StartCoroutine(DieObject());
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {

    }

    private IEnumerator DieObject()
    {
        Owner.Animator.SetTrigger(Literals.Dead);
        Owner.CharacterInfo.CurRoom.RemoveEnemy(this.Owner);
        yield return new WaitForSeconds(1);
        Owner.ResetCharacter();
        Owner.StateMachine.InitState();
        Main.Get<ResourceManager>().Destroy(Owner.gameObject);
    }
}
