using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyAttackState : BaseState
{
    private Coroutine _coroutine;
    private Character[] _targets;

    public EnemyAttackState(Character owner) : base(owner)
    {
        
    }

    public override void EnterState()
    {
        _targets = ((BatRoom)Owner.CurRoom).Units;
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
        Character target = SetAttackTartget();

        if( target == null)
        {
            Owner.StateMachine.ChangeState(EState.Move);
            yield break;
        }

        Owner.Animator.SetTrigger(Literals.Attack);
        target.Status.GetStat<Vital>(EstatType.Hp).CurValue -= Owner.Status[EstatType.Damage].Value;

        yield return new WaitForSeconds(1 / Owner.Status[EstatType.AttackSpeed].Value);
        AttackStart();
    }

    private Character SetAttackTartget()
    {
        for(int i = 0; i < _targets.Length; i++)
        {
            if (_targets[i] != null)
            {
                return _targets[i];
            }
        }
        return null;
    }
}
