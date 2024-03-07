using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitKunoichiSkillState : BaseState
{
    private LinkedList<CharacterBehaviour> _targets;
    private LinkedList<RangedTargetInfo> _neighborTargets;

    public UnitKunoichiSkillState(CharacterBehaviour owner) : base(owner)
    {

    }

    public override void Init()
    {

    }

    public override void EnterState()
    {
        Owner.Animator.SetTrigger(Literals.Skill);
        Main.Get<SoundManager>().SoundPlay($"{Owner.CharacterInfo.Data.PrefabName}SkillStart", ESoundType.Effect);

        Owner.Status.GetStat<Vital>(EstatType.Mp).CurValue = 0;
        _targets = ((BatRoom)Owner.CharacterInfo.CurRoom).Enemys;
        _neighborTargets = Owner.GetNeighborTarget();
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        if (Owner.Animator.GetCurrentAnimatorStateInfo(0).IsName("Unit_Gun_Skill") &&
                Owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            CharacterBehaviour target = SetTartget();

            if (target == null)
            {
                Owner.StateMachine.ChangeState(EState.Attack);
                return;
            }

            if (Owner == null)
            {
                return;
            }

            SetDir(target);

            GameObject go = Main.Get<ResourceManager>().Instantiate($"{Literals.FX_PATH}KunoichiFx1");
            Vector3 pos = target.GetWorldPos();
            go.transform.position = pos;

            float damage = Owner.CharacterInfo.CalculateSkillValue();
            target.TakeDamage(damage);
            Main.Get<SoundManager>().SoundPlay($"{Owner.CharacterInfo.Data.PrefabName}SkillEnd", ESoundType.Effect);

            Owner.StateMachine.ChangeState(EState.Attack);
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

        if(_neighborTargets != null)
        {
            var targetsToRemove = new List<RangedTargetInfo>();
            foreach (RangedTargetInfo targetInfo in _neighborTargets)
            {
                if (targetInfo.Target.CurRoom != targetInfo.TargetRoom)
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
