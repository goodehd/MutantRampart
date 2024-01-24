using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackState : BaseState
{
    private Coroutine _coroutine;

    public UnitAttackState(Character owner) : base(owner)
    {

    }

    public override void EnterState()
    {
        AttackStart();
    }

    public override void ExitState()
    {
        CoroutineManagement.Instance.StopCoroutine(_coroutine);
    }

    public override void UpdateState()
    {

    }

    private void AttackStart()
    {
        _coroutine = CoroutineManagement.Instance.StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        Owner.Animator.SetTrigger(Literals.Attack);

        yield return new WaitForSeconds(1 / Owner.Status[EstatType.AttackSpeed].Value);
        AttackStart();
    }
}
