using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    public AttackState(Character owner) : base(owner)
    {
    }

    public override void EnterState()
    {
        Owner.Animator.SetBool(Literals.Attack, true);
    }

    public override void ExitState()
    {
        Owner.Animator.SetBool(Literals.Attack, false);
    }

    public override void UpdateState()
    {

    }
}
