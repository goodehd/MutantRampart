using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingCondition : BaseCondition
{
    public override event Action<BaseCondition> OnEndCondition;
    
    private Coroutine _coroutine;
    private float UpgradeValue_1 { get; set; }
    private float UpgradeValue_2 { get; set; }
    private float UpgradeValue_3 { get; set; }
    public HealingCondition(CharacterBehaviour owner, RoomData data) : base(owner, data)
    {
        OwnerData = data;
        Duration = data.Duration;
        UpgradeValue_1 = data.UpgradeValue_1;
        UpgradeValue_2 = data.UpgradeValue_2;
        UpgradeValue_3 = data.UpgradeValue_3;

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
        //_coroutine = CoroutineManagement.Instance.StartCoroutine(ConditionEffect(UpgradeValue_1));
    }

    public override void UpdateCondition()
    {
        
    }

    public override void ExitCondition()
    {
        StopCoroutine();
        OnEndCondition?.Invoke(this);
    }

    public override void StopCoroutine()
    {
        if (_coroutine != null)
        {
            //CoroutineManagement.Instance.StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    public override IEnumerator ConditionDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        StopCoroutine();
    }

    public IEnumerator ConditionEffect(float a)
    {
        while (true)
        {
            Owner.Status.GetStat<Vital>(EstatType.Hp).CurValue += a;
            yield return new WaitForSeconds(1f);
        }
    }
}
