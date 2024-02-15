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

        Owner.Status.GetStat<Vital>(EstatType.Hp).OnValueZero += ExitCondition;
    }

    public override void EnterCondition()
    {
        ConditionEffect(_upgradeValue_1);
        Owner.StartCoroutine(ConditionDuration(Duration));
    }

    public override IEnumerator ConditionDuration(float duration)
    {
        if (Duration >= 999) yield break;
        UnityEngine.Debug.Log("빙결 쿨 시작");
        yield return new WaitForSeconds(duration);
        UnityEngine.Debug.Log("빙결 쿨 종료");
        StopCoroutine();
    }

    public void ConditionEffect(float DataValue)
    {
        StatModifier mod = new StatModifier(DataValue/100f, EStatModType.Multip, 1, this);
        Owner.Status.GetStat<Stat>(EstatType.MoveSpeed).AddModifier(mod);
        UnityEngine.Debug.Log("빙결효과 적용");
    }

    public override void ExitCondition()
    {
        StopCoroutine();
        InvokeEndCondition();
    }

    public override void StopCoroutine()
    {
        UnityEngine.Debug.Log("빙결 디버프 종료");
        Owner.Status.GetStat<Stat>(EstatType.MoveSpeed).RemoveAllModifier(this);

    }

    public override void UpdateCondition()
    {

    }
}
