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
    }

    public override void EnterCondition()
    {
        Owner.StartCoroutine(ConditionDuration(Duration));
        //캐릭터가 때릴 때 이벤트(때린놈 캐릭터비헤이비어) += ConditionEffect
    }

    public override IEnumerator ConditionDuration(float duration)
    {
        if (Duration >= 999) yield break;
        yield return new WaitForSeconds(duration);
        StopCoroutine();
    }

    public void ConditionEffect(CharacterBehaviour attackEntity)
    {
        float a = Random.Range(0, 100);
        if (a > _upgradeValue_2) return;
        attackEntity.ConditionMachine.AddCondition(new FrozenCondition(attackEntity,_upgradeValue_1));
    }

    public override void ExitCondition()
    {
        StopCoroutine();
        InvokeEndCondition();
    }

    public override void StopCoroutine()
    {
        
    }

    public override void UpdateCondition()
    {

    }
}
