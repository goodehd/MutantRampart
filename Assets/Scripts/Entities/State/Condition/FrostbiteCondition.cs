using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class FrostbiteCondition : BaseCondition
{
    private Coroutine _durationCoroutine;
    private float _upgradeValue_1 { get; set; }

    public FrostbiteCondition(CharacterBehaviour owner, RoomData data) : base(owner, data)
    {
        OwnerData = data;
        Duration = data.Duration;
        _upgradeValue_1 = data.UpgradeValue_1;
        ConditionName = ECondition.Frostbite;
    }

    public FrostbiteCondition(CharacterBehaviour owner, ItemData data) : base(owner, data)
    {
        OwnerData = data;
        Duration = data.UpgradeValue_1;
        _upgradeValue_1 = data.UpgradeValue_1;
        ConditionName = ECondition.Frostbite;
    }

    public override void EnterCondition()
    {
        _durationCoroutine = Owner.StartCoroutine(ConditionDuration(Duration));
        ConditionEffect(_upgradeValue_1);
    }

    public IEnumerator ConditionDuration(float duration)
    {
        var seconds = new WaitForSeconds(duration);
        yield return seconds;
        _durationCoroutine = null;
        Owner.ConditionMachine.RemoveCondition(this);
    }

    public void ConditionEffect(float DataValue)
    {
        StatModifier mod = new StatModifier(DataValue/100f, EStatModType.Multip, 1, this);
        Owner.Status.GetStat<Stat>(EstatType.MoveSpeed).AddModifier(mod);
    }

    public override void ExitCondition()
    {
        if (_durationCoroutine != null)
        {
            Owner.StopCoroutine(_durationCoroutine);
        }
        Owner.Status.GetStat<Stat>(EstatType.MoveSpeed).RemoveAllModifier(this);
    }
}
