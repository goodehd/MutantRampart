using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class FrostbiteCondition : BaseCondition
{
    private float _upgradeValue_1 { get; set; }
    public FrostbiteCondition(CharacterBehaviour owner, RoomData data) : base(owner, data)
    {
        OwnerData = data;
        Duration = data.Duration;
        _upgradeValue_1 = data.UpgradeValue_1;
        ConditionName = ECondition.Frostbite;
        Owner.Status.GetStat<Vital>(EstatType.Hp).OnValueZero += ExitCondition;
    }
    public FrostbiteCondition(CharacterBehaviour owner, ItemData data) : base(owner, data)
    {
        OwnerData = data;
        Duration = data.UpgradeValue_1;
        _upgradeValue_1 = data.UpgradeValue_1;
        ConditionName = ECondition.Frostbite;
        Owner.Status.GetStat<Vital>(EstatType.Hp).OnValueZero += ExitCondition;
    }

    public override void EnterCondition()
    {
        Owner.StartCoroutine(ConditionDuration(Duration));
        ConditionEffect(_upgradeValue_1);
        
    }

    public override IEnumerator ConditionDuration(float duration)
    {
        if (Duration >= 999) yield break;
        yield return new WaitForSeconds(duration);
        ExitCondition();
    }

    public void ConditionEffect(float DataValue)
    {
        StatModifier mod = new StatModifier(DataValue/100f, EStatModType.Multip, 1, this);
        Owner.Status.GetStat<Stat>(EstatType.MoveSpeed).AddModifier(mod);
    }

    public override void ExitCondition()
    {
        StopCoroutine();
        InvokeEndCondition();
    }

    public override void StopCoroutine()
    {
        Owner.Status.GetStat<Stat>(EstatType.MoveSpeed).RemoveAllModifier(this);
    }

    public override void UpdateCondition()
    {

    }
}
