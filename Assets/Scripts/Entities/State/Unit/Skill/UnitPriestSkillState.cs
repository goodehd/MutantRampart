using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPriestSkillState : BaseState
{
    private CharacterBehaviour[] _units;

    public UnitPriestSkillState(CharacterBehaviour owner) : base(owner)
    {
    }

    public override void Init()
    {

    }

    public override void EnterState()
    {
        Owner.Animator.SetTrigger(Literals.Skill);
        Main.Get<SoundManager>().SoundPlay($"{Owner.CharacterInfo.Data.PrefabName}Skill", ESoundType.Effect);

        Owner.Status.GetStat<Vital>(EstatType.Mp).CurValue = 0;
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
            float value = Owner.CharacterInfo.CalculateSkillValue();
            CharacterBehaviour target = null;
            float ratio = float.MaxValue;

            for (int i = 0; i < _units.Length; i++)
            {
                if (_units[i] == null || _units[i].CharacterInfo.IsDead)
                    continue;

                float targetRatio = _units[i].Status.GetStat<Vital>(EstatType.Hp).Normalized();

                if(targetRatio < ratio)
                {
                    ratio = targetRatio;
                    target = _units[i];
                }
            }

            GameObject go = Main.Get<ResourceManager>().Instantiate($"{Literals.FX_PATH}PriestFx1");
            Vector3 pos = target.GetWorldPos();
            pos.y += 0.5f;
            go.transform.position = pos;
            target.Status.GetStat<Vital>(EstatType.Hp).CurValue += value;
            Owner.StateMachine.ChangeState(EState.Attack);
        }
    }
}
