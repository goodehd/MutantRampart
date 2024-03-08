using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : BaseState
{
    private TileManager tileManager;
    private Coroutine _coroutine;
    private CharacterBehaviour[] _targets;

    public EnemyAttackState(CharacterBehaviour owner) : base(owner)
    {
        tileManager = Main.Get<TileManager>();
    }

    public override void Init()
    {

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

    public void StopCoroutine()
    {
        if (_coroutine != null)
        {
            Owner.StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    private void AttackStart()
    {
        _coroutine = Owner.StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            float attackSpeed = Owner.Status[EstatType.AttackSpeed].Value;

            if (attackSpeed > 0) // attackSpeed가 0보다 클 때만 루프 실행
            {
                yield return new WaitForSeconds(0.2f);

                CharacterBehaviour target = SetAttackTartget();

                if (target == null)
                {
                    if (tileManager.GetRoom(Owner.CurPosX, Owner.CurPosY).isEndPoint)
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
                target.TakeDamage(Owner.Status[EstatType.Damage].Value);
                yield return new WaitForSeconds(1 / attackSpeed);
            }
            else
            {
                yield return null;
            }
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
