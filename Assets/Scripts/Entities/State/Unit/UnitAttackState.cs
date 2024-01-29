using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UnitAttackState : BaseState
{
    private Coroutine _coroutine;
    private LinkedList<CharacterBehaviour> _targets;
    public event Action OnAttackState; 

    public UnitAttackState(CharacterBehaviour owner) : base(owner)
    {
        OnAttackState += owner.CharacterInfo.OnAttackInvoke;
    }

    public override void EnterState()
    {
        _targets = ((BatRoom)Owner.CharacterInfo.CurRoom).Enemys;
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
        OnAttackState?.Invoke();
    }

    private IEnumerator Attack()
    {
        if(_targets.Count == 0)
        {
            Owner.StateMachine.ChangeState(EState.Idle);
            yield break;
        }

        if(Owner == null)
        {
            yield break;
        }

        Owner.Animator.SetTrigger(Literals.Attack);
        _targets.First.Value.Status.GetStat<Vital>(EstatType.Hp).CurValue -= Owner.Status[EstatType.Damage].Value;

        yield return new WaitForSeconds(1 / Owner.Status[EstatType.AttackSpeed].Value);
        AttackStart();
    }
}
