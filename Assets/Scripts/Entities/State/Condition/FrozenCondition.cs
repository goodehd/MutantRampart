using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenCondition : BaseCondition //TODO : 유닛한테 버프를 쥐어주고 이거는 당한 몬스터에게 적용되는 빙결디버프
{
    private float _upgradeValue_1 { get; set; }
    public FrozenCondition(CharacterBehaviour owner, RoomData data) : base(owner, data)
    {
        OwnerData = data;
        Duration = data.Duration;
        _upgradeValue_1 = data.UpgradeValue_1;
        ConditionName = ECondition.Frozen;
        Owner.Status.GetStat<Vital>(EstatType.Hp).OnValueZero += ExitCondition;
        Conditionpersonality = EConditionpersonality.debuff;
    }
    public FrozenCondition(CharacterBehaviour owner, float duration) : base(owner, duration)
    {
        Duration = duration;
        ConditionName = ECondition.Frozen;
        Conditionpersonality = EConditionpersonality.debuff;
        Owner.Status.GetStat<Vital>(EstatType.Hp).OnValueZero += ExitCondition;
    }

    public override void EnterCondition()
    {
        Owner.StartCoroutine(ConditionDuration(Duration));
        ConditionEffect(_upgradeValue_1);
    }

    public override IEnumerator ConditionDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        ExitCondition();
    }

    public void ConditionEffect(float DataValue)
    {
        StatModifier mod = new StatModifier(0f, EStatModType.Multip, 1, this);
        Owner.Status.GetStat<Stat>(EstatType.MoveSpeed).AddModifier(mod);
        Owner.Status.GetStat<Stat>(EstatType.AttackSpeed).AddModifier(mod);
    }

    public override void ExitCondition()
    {
        StopCoroutine();
        InvokeEndCondition();
    }

    public override void StopCoroutine()
    {
        Owner.Status.GetStat<Stat>(EstatType.MoveSpeed).RemoveAllModifier(this);
        Owner.Status.GetStat<Stat>(EstatType.AttackSpeed).RemoveAllModifier(this);
    }

    public override void UpdateCondition()
    {

    }

}
