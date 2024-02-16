using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDeadState : BaseState
{
    public UnitDeadState(CharacterBehaviour owner) : base(owner)
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
        yield return new WaitForSeconds(1);
        Owner.gameObject.SetActive(false);
    }
}