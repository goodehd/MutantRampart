using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitKnightSkillState : BaseState
{
    private bool IsSkillActive;

    public UnitKnightSkillState(CharacterBehaviour owner) : base(owner)
    {

    }

    public override void Init()
    {
        IsSkillActive = false;
        Owner.Status[EstatType.Defense].RemoveAllModifier(this);
    }

    public override void EnterState()
    {
        Owner.Animator.SetTrigger(Literals.Skill);
        Main.Get<SoundManager>().SoundPlay($"{Owner.CharacterInfo.Data.PrefabName}Skill", ESoundType.Effect);

        //TODO : 여기쯤에서 애니메이션이 시작한다.
        Owner.Status.GetStat<Vital>(EstatType.Mp).CurValue = 0;
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        if (Owner.Animator.GetCurrentAnimatorStateInfo(0).IsName("Unit_Gun_Skill") &&
                Owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            if (!IsSkillActive)
            {
                Owner.Status[EstatType.Defense].AddModifier(new StatModifier(Owner.CharacterInfo.SkillData.BaseValue, EStatModType.Add, this));
                IsSkillActive = true;
            }
            Owner.Status.GetStat<Vital>(EstatType.Hp).CurValue += Owner.CharacterInfo.SkillData.DefenseCoefficient * Owner.Status[EstatType.Defense].Value;
            //TODO : 여기쯤에서 애니메이션이 끝난다.
            Owner.StateMachine.ChangeState(EState.Attack);

        }
    }
}
