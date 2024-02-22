using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingCondition : BaseCondition
{
    private Coroutine _co;

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
        ConditionName = ECondition.Healing;
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
        _co = Owner.StartCoroutine(ConditionEffect(_upgradeValue_1));
    }

    public override void ExitCondition()
    {
        if(_co != null)
        {
            Owner.StopCoroutine(_co);
            _co = null;
        }
    }

    public IEnumerator ConditionEffect(float DataValue)
    {
        while (true)
        {
            Owner.Status.GetStat<Vital>(EstatType.Hp).CurValue += Owner.Status.GetStat<Vital>(EstatType.Hp).Value * (DataValue*0.01f);
            yield return new WaitForSeconds(1f);
        }
    }
}
