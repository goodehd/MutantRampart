using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UnitAreaAttackState : BaseState
{
    private Coroutine _coroutine;
    private LinkedList<CharacterBehaviour> _targets;

    public UnitAreaAttackState(CharacterBehaviour owner) : base(owner)
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
        if (_coroutine != null)
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

            List<CharacterBehaviour> tartgetList = SetTartget();

            if (tartgetList.Count == 0)
            {
                Owner.StateMachine.ChangeState(EState.Idle);
                yield break;
            }

            if (Owner == null)
            {
                yield break;
            }

            Owner.Status.GetStat<Vital>(EstatType.Mp).CurValue += Owner.CharacterInfo.Data.HitRecoveryMp;
            if (Owner.Status.GetStat<Vital>(EstatType.Mp).CurValue >= Owner.Status.GetStat<Vital>(EstatType.Mp).Value)
            {
                Owner.StateMachine.ChangeState(EState.Skill);
                yield break;
            }

            for(int i = 0; i < tartgetList.Count; i++)
            {
                Owner.CharacterInfo.InvokeAttackAction(tartgetList[i]);

                float damage = i == 0 ? Owner.Status[EstatType.Damage].Value : Owner.Status[EstatType.Damage].Value / 2;
                tartgetList[i].TakeDamage(damage);
            }

            Owner.Animator.SetTrigger(Literals.Attack);
            Main.Get<SoundManager>().SoundPlay($"{Owner.CharacterInfo.Data.PrefabName}Attack", ESoundType.Effect);
            yield return new WaitForSeconds(1 / Owner.Status[EstatType.AttackSpeed].Value);
        }
    }

    private List<CharacterBehaviour> SetTartget()
    {
        List<CharacterBehaviour> tartgetList = new List<CharacterBehaviour>();
        foreach (CharacterBehaviour t in _targets)
        {
            if (!t.CharacterInfo.IsDead)
            {
                tartgetList.Add(t);
            }

            if(tartgetList.Count >= Owner.CharacterInfo.Data.TargetCount)
            {
                break;
            }
        }
        return tartgetList;
    }
}
