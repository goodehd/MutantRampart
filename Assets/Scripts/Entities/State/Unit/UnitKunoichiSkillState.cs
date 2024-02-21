using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitKunoichiSkillState : BaseState
{
    private LinkedList<CharacterBehaviour> _targets;

    public UnitKunoichiSkillState(CharacterBehaviour owner) : base(owner)
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
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        if (Owner.Animator.GetCurrentAnimatorStateInfo(0).IsName("Unit_Gun_Skill") &&
                Owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            GameObject go = Main.Get<ResourceManager>().Instantiate($"{Literals.FX_PATH}KunoichiFx1");
            Vector3 pos = _targets.First.Value.GetWorldPos();
            go.transform.position = pos;

            float damage = Owner.CharacterInfo.CalculateSkillValue();
            _targets.First.Value.Status.GetStat<Vital>(EstatType.Hp).CurValue -= damage;
            Owner.StateMachine.ChangeState(EState.Attack);
        }
    }
}
