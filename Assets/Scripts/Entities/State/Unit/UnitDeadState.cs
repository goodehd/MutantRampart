using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDeadState : BaseState
{
    public UnitDeadState(CharacterBehaviour owner) : base(owner)
    {
    }

    public override void EnterState()
    {
        CoroutineManagement.Instance.StartCoroutine(DieObject());
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {

    }

    public override void StopCoroutine()
    {

    }

    private IEnumerator DieObject()
    {
        Owner.Animator.SetTrigger(Literals.Dead);
        yield return new WaitForSeconds(1);
        Owner.gameObject.SetActive(false);
    }
}