using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitClericSkillState : BaseState
{
    private LinkedList<CharacterBehaviour> _targets;
    private CharacterBehaviour[] _units;

    public UnitClericSkillState(CharacterBehaviour owner) : base(owner)
    {
    }

    public override void Init()
    {

    }

    public override void EnterState()
    {
        Owner.Animator.SetTrigger(Literals.Skill);
        Owner.Status.GetStat<Vital>(EstatType.Mp).CurValue = 0;
        _targets = ((BatRoom)Owner.CharacterInfo.CurRoom).Enemys;
        _units = ((BatRoom)Owner.CharacterInfo.CurRoom).Units;
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        if (Owner.Animator.GetCurrentAnimatorStateInfo(0).IsName("Unit_Gun_Skill") &&
                Owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            GameObject go = Main.Get<ResourceManager>().Instantiate($"{Literals.FX_PATH}ClericFx1");
            Vector3 pos = Owner.GetWorldPos();
            pos.y -= 0.3f;
            go.transform.position = pos;

            float damage = Owner.CharacterInfo.CalculateSkillValue();
            foreach (CharacterBehaviour target in _targets.ToList())
            {
                target.Status.GetStat<Vital>(EstatType.Hp).CurValue -= damage;
            }

            for (int i = 0; i < _units.Length; i++)
            {
                if (_units[i] == null)
                    continue;

                _units[i].Status.GetStat<Vital>(EstatType.Hp).CurValue += damage;
            }
            Owner.StateMachine.ChangeState(EState.Attack);
        }
    }
}
