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
        Owner.Renderer.color = Color.white;
    }

    public override void UpdateState()
    {

    }

    private IEnumerator DieObject()
    {
        Owner.Animator.SetTrigger(Literals.Dead);
        yield return new WaitForSeconds(1);
        Owner.Renderer.color = Color.white;
        Owner.gameObject.SetActive(false);
    }
}