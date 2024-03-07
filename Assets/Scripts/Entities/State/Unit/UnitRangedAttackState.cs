using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedTargetInfo
{
    public CharacterBehaviour Target;
    public RoomBehavior TargetRoom;
}

public class UnitRangedAttackState : BaseState
{
    private Coroutine _coroutine;
    private LinkedList<CharacterBehaviour> _targets;
    private LinkedList<RangedTargetInfo> _neighborTargets;

    public UnitRangedAttackState(CharacterBehaviour owner) : base(owner)
    {
        Init();
    }

    public override void Init()
    {
        _neighborTargets = new LinkedList<RangedTargetInfo>();
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

    public void AddNeighborTarget(Enemy enemy)
    {
        RangedTargetInfo newInfo = new RangedTargetInfo();
        newInfo.Target = enemy;
        newInfo.TargetRoom = enemy.CurRoom;
        _neighborTargets.AddLast(newInfo);
    }

    private void AttackStart()
    {
        _coroutine = Owner.StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        var firstDelay = new WaitForSeconds(0.2f);
        var attackSpeed = new WaitForSeconds(1 / Owner.Status[EstatType.AttackSpeed].Value);
        while (true)
        {
            yield return firstDelay;

            CharacterBehaviour target = SetTartget();
            List<CharacterBehaviour> targets = new List<CharacterBehaviour> {target};

            if (target == null)
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

            SetDir(target);

            Owner.CharacterInfo.InvokeAttackAction(targets);
            Owner.Animator.SetTrigger(Literals.Attack);
            Main.Get<SoundManager>().SoundPlay($"{Owner.CharacterInfo.Data.PrefabName}Attack", ESoundType.Effect);
            target.TakeDamage(Owner.Status[EstatType.Damage].Value);
            yield return attackSpeed;
        }
    }

    private CharacterBehaviour SetTartget()
    {
        foreach (CharacterBehaviour target in _targets)
        {
            if (!target.CharacterInfo.IsDead)
            {
                return target;
            }
        }

        var targetsToRemove = new List<RangedTargetInfo>();
        foreach (RangedTargetInfo targetInfo in _neighborTargets)
        {
            if(targetInfo.Target.CurRoom != targetInfo.TargetRoom)
            {
                targetsToRemove.Add(targetInfo);
                continue;
            }

            if (targetInfo.Target.CharacterInfo.IsDead)
            {
                targetsToRemove.Add(targetInfo);
                continue;
            }

            if (!targetInfo.Target.CharacterInfo.IsDead)
            {
                return targetInfo.Target;
            }
        }

        foreach (var targetToRemove in targetsToRemove)
        {
            _neighborTargets.Remove(targetToRemove);
        }

        return null;
    }

    private void SetDir(CharacterBehaviour target)
    {
        Vector3 direction = target.transform.position - Owner.transform.position;
        float dotProduct = Vector3.Dot(direction, Vector3.right);

        if (dotProduct > 0)
        {
            Owner.Renderer.flipX = false;
        }
        else if (dotProduct < 0)
        {
            Owner.Renderer.flipX = true;
        }
    }
}
