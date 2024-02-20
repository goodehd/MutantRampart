using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitShamanSkillState : BaseState
{
    private LinkedList<CharacterBehaviour> _targets;

    public UnitShamanSkillState(CharacterBehaviour owner) : base(owner)
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

        GameObject go = Main.Get<ResourceManager>().Instantiate($"{Literals.FX_PATH}ShamanFx1");
        Vector3 pos = Owner.GetWorldPos();
        pos.x += 0.1f;
        go.transform.position = pos;
        go.GetComponent<ShamanFx1>().Pos = pos;
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        if (Owner.Animator.GetCurrentAnimatorStateInfo(0).IsName("Unit_Gun_Skill") &&
                Owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            float damage = Owner.CharacterInfo.CalculateSkillValue();
            int count = 0;
            foreach (CharacterBehaviour target in _targets.ToList())
            {
                if (count >= 3)
                    break;

                count++;
                target.Status.GetStat<Vital>(EstatType.Hp).CurValue -= damage;
            }
            Owner.StateMachine.ChangeState(EState.Attack);
        }
    }
}
