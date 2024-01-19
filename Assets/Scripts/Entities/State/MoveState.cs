using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : BaseState
{
    public MoveState(Character owner) : base(owner)
    {

    }

    public override void EnterState()
    {
        Owner.Animator.SetBool(Literals.Move, true);
    }

    public override void ExitState()
    {
        Owner.Animator.SetBool(Literals.Move, false);
    }

    public override void UpdateState()
    {

    }
}
