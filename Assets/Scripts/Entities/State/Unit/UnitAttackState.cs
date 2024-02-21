using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackState : BaseState
{
    private Coroutine _coroutine;
    private LinkedList<CharacterBehaviour> _targets;

    public UnitAttackState(CharacterBehaviour owner) : base(owner)
    {
        Init();
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
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

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

            Owner.CharacterInfo.InvokeAttackAction(target);
            Owner.Status.GetStat<Vital>(EstatType.Mp).CurValue += Owner.CharacterInfo.Data.HitRecoveryMp;
            if (Owner.Status.GetStat<Vital>(EstatType.Mp).CurValue >= Owner.Status.GetStat<Vital>(EstatType.Mp).Value)
            {
                Owner.StateMachine.ChangeState(EState.Skill);
                yield break;
            }
            Owner.Animator.SetTrigger(Literals.Attack);
            Main.Get<SoundManager>().SoundPlay($"{Owner.CharacterInfo.Data.PrefabName}Attack", ESoundType.Effect);
            target.TakeDamage(Owner.Status[EstatType.Damage].Value);
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
