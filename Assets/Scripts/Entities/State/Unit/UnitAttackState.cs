using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackState : BaseState
{
    private Coroutine _coroutine;
    private LinkedList<CharacterBehaviour> _targets;
    public event Action OnAttackState; 

    public UnitAttackState(CharacterBehaviour owner) : base(owner)
    {
        OnAttackState += owner.CharacterInfo.OnAttackInvoke;
    }

    public override void Init()
    {

    }

    public override void EnterState()
    {
        _targets = ((BatRoom)Owner.CharacterInfo.CurRoom).Enemys;
        AttackStart();
    }

    public override void ExitState()
    {
        StopCoroutine();
    }

    public void StopCoroutine()
    {
        if(_coroutine != null)
        {
            Owner.StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    public override void UpdateState()
    {

    }

    private void AttackStart()
    {
        _coroutine = Owner.StartCoroutine(Attack());
        OnAttackState?.Invoke();
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            CharacterBehaviour target = SetTartget();

            if (target == null)
            {
                Owner.StateMachine.ChangeState(EState.Idle);
                yield break;
            }

            if (Owner == null)
            {
                yield break;
            }

            Owner.Animator.SetTrigger(Literals.Attack);
            target.Status.GetStat<Vital>(EstatType.Hp).CurValue -= Owner.Status[EstatType.Damage].Value;

            yield return new WaitForSeconds(1 / Owner.Status[EstatType.AttackSpeed].Value);
        }
    }

    private CharacterBehaviour SetTartget()
    {
        foreach (CharacterBehaviour t in _targets)
        {
            if(!t.CharacterInfo.IsDead)
            {
                return t;
            }
        }
        return null;
    }
}
