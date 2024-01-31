using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyAttackState : BaseState
{
    private TileManager tileManager;
    private Coroutine _coroutine;
    private CharacterBehaviour[] _targets;

    public EnemyAttackState(CharacterBehaviour owner) : base(owner)
    {
        tileManager = Main.Get<TileManager>();
    }

    public override void EnterState()
    {
        _targets = ((BatRoom)Owner.CharacterInfo.CurRoom).Units;
        AttackStart();
    }

    public override void ExitState()
    {
        StopCoroutine();
    }

    public override void UpdateState()
    {

    }

    public override void StopCoroutine()
    {
        if (_coroutine != null)
        {
            CoroutineManagement.Instance.StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    private void AttackStart()
    {
        _coroutine = CoroutineManagement.Instance.StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            CharacterBehaviour target = SetAttackTartget();

            if (target == null)
            {
                if (tileManager.GetRoom(Owner.CurPosX, Owner.CurPosY).GetComponent<RoomBehavior>().isEndPoint)
                {
                    Owner.StateMachine.ChangeState(EState.Dead);
                    Main.Get<GameManager>().PlayerHP.CurValue--;
                }
                else
                {
                    Owner.StateMachine.ChangeState(EState.Move);
                }
                yield break;
            }

            Owner.Animator.SetTrigger(Literals.Attack);
            target.Status.GetStat<Vital>(EstatType.Hp).CurValue -= Owner.Status[EstatType.Damage].Value;

            yield return new WaitForSeconds(1 / Owner.Status[EstatType.AttackSpeed].Value);
        }
    }

    private CharacterBehaviour SetAttackTartget()
    {
        for(int i = 0; i < _targets.Length; i++)
        {
            if (_targets[i] != null && !_targets[i].CharacterInfo.IsDead)
            {
                return _targets[i];
            }
        }
        return null;
    }
}
