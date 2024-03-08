using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenCondition : BaseCondition
{
    private Coroutine _durationCoroutine;
    private float _upgradeValue_1 { get; set; }

    public FrozenCondition(CharacterBehaviour owner, RoomData data) : base(owner, data)
    {
        OwnerData = data;
        Duration = data.Duration;
        _upgradeValue_1 = data.UpgradeValue_1;
        ConditionName = ECondition.Frozen;
        Conditionpersonality = EConditionpersonality.debuff;
    }

    public FrozenCondition(CharacterBehaviour owner, float duration) : base(owner, duration)
    {
        Duration = duration;
        ConditionName = ECondition.Frozen;
        Conditionpersonality = EConditionpersonality.debuff;
    }

    public override void EnterCondition()
    {
        if (Owner.CharacterInfo.IsDead) return;
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
        StatModifier mod = new StatModifier(0f, EStatModType.Multip, 1, this);
        Owner.Status.GetStat<Stat>(EstatType.MoveSpeed).AddModifier(mod);
        Owner.Status.GetStat<Stat>(EstatType.AttackSpeed).AddModifier(mod);
    }

    public override void ExitCondition()
    {
        if (_durationCoroutine != null)
        {
            Owner.StopCoroutine(_durationCoroutine);
        }
        Owner.TakeDmageNoneDefense(Owner.Status.GetStat<Vital>(EstatType.Hp).Value * (5 * 0.01f));
        Owner.Status.GetStat<Stat>(EstatType.MoveSpeed).RemoveAllModifier(this);
        Owner.Status.GetStat<Stat>(EstatType.AttackSpeed).RemoveAllModifier(this);
    }
}
