using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezingAttackCondition : BaseCondition
{
    private float _upgradeValue_1 { get; set; }
    private float _upgradeValue_2 { get; set; }
    public FreezingAttackCondition(CharacterBehaviour owner, RoomData data) : base(owner, data)
    {
        OwnerData = data;
        Duration = data.Duration;
        _upgradeValue_1 = data.UpgradeValue_1;
        _upgradeValue_2 = data.UpgradeValue_2;
        ConditionName = ECondition.FreezingAttack;
    }

    public override void EnterCondition()
    {
        Owner.CharacterInfo.OnAttackState += ConditionEffect;
    }

    public override void ExitCondition()
    {
        Owner.CharacterInfo.OnAttackState -= ConditionEffect;
    }

    public void ConditionEffect(List<CharacterBehaviour> attackEntity)
    {
        foreach (CharacterBehaviour attack in attackEntity)
        {
            if (attack.ConditionMachine.CheckCondition(ECondition.Frozen))
            {
                return;
            }

            float a = Random.Range(0, 100);
            if (attack.ConditionMachine.CheckCondition(ECondition.Frostbite))
            {
                attack.ConditionMachine.AddCondition(new FrozenCondition(attack, _upgradeValue_1));
            }
            else if (a < _upgradeValue_2)
            {
                attack.ConditionMachine.AddCondition(new FrozenCondition(attack, _upgradeValue_1));
            }
        }
    }
}
