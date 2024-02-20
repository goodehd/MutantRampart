using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(CharacterBehaviour owner) : base(owner)
    {
    }

    public override void Init()
    {

    }

    public override void EnterState()
    {
        Owner.Animator.SetBool(Literals.Idle, true);
    }

    public override void ExitState()
    {
        Owner.Animator.SetBool(Literals.Idle, false);
    }

    public override void UpdateState()
    {

    }
}
