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
    }
    public FrozenCondition(CharacterBehaviour owner, float duration) : base(owner, duration)
    {
        Duration = duration;
    }

    public override void EnterCondition()
    {
        Owner.StartCoroutine(ConditionEffect(_upgradeValue_1));
        Owner.StartCoroutine(ConditionDuration(Duration));
    }

    public override IEnumerator ConditionDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        StopCoroutine();
    }

    public IEnumerator ConditionEffect(float DataValue)
    {
        StatModifier mod = new StatModifier(0f, EStatModType.Multip, 1, this);
        Owner.Status.GetStat<Stat>(EstatType.MoveSpeed).AddModifier(mod);
        Owner.Status.GetStat<Stat>(EstatType.AttackSpeed).AddModifier(mod);
        yield return null;
    }

    public override void ExitCondition()
    {
        StopCoroutine();
        InvokeEndCondition();
    }

    public override void StopCoroutine()
    {
        Owner.Status.GetStat<Stat>(EstatType.MoveSpeed).RemoveAllModifier(this);
        Owner.StopCoroutine(ConditionEffect(_upgradeValue_1));
    }

    public override void UpdateCondition()
    {

    }

}
