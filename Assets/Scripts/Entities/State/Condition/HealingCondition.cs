using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingCondition : BaseCondition
{
    private float _upgradeValue_1 { get; set; }
    private float _upgradeValue_2 { get; set; }
    private float _upgradeValue_3 { get; set; }
    public HealingCondition(CharacterBehaviour owner, RoomData data) : base(owner, data)
    {
        OwnerData = data;
        Duration = data.Duration;
        _upgradeValue_1 = data.UpgradeValue_1;
        _upgradeValue_2 = data.UpgradeValue_2;
        _upgradeValue_3 = data.UpgradeValue_3;

        Owner.Status.GetStat<Vital>(EstatType.Hp).OnValueZero += ExitCondition;
    }

    public HealingCondition(CharacterBehaviour owner, CharacterData data) : base(owner, data)
    {
        OwnerData = data;
    }

    public HealingCondition(CharacterBehaviour owner, ItemData data) : base(owner, data)
    {
        OwnerData = data;
    }
    
    public override void EnterCondition()
    {
        Owner.StartCoroutine(ConditionEffect(_upgradeValue_1));
        Owner.StartCoroutine(ConditionDuration(Duration));
    }

    public override void UpdateCondition()
    {
        
    }

    public override void ExitCondition()
    {
        StopCoroutine();
        InvokeEndCondition();
    }

    public override void StopCoroutine()
    {
        Owner.StopCoroutine(ConditionEffect(_upgradeValue_1));
    }

    public override IEnumerator ConditionDuration(float duration)
    {
        if (Duration >= 999) yield break;
        yield return new WaitForSeconds(duration);
        StopCoroutine();
    }

    public IEnumerator ConditionEffect(float DataValue)
    {
        while (true)
        {
            Owner.Status.GetStat<Vital>(EstatType.Hp).CurValue += DataValue;
            yield return new WaitForSeconds(1f);
        }
    }
}