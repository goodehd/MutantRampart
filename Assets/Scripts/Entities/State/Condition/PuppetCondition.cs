using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuppetCondition : BaseCondition //TODO : 아군으로 바뀌는 로직과, 사망시 폭발하여 해당 방의 적들에게 피해를 주는것 어디에 들어가야할까?
{
    public PuppetCondition(CharacterBehaviour owner, RoomData data) : base(owner, data)
    {
        ConditionName = ECondition.Puppet;
    }

    public override IEnumerator ConditionDuration(float duration)
    {
        if (Duration >= 999) yield break;
        yield return new WaitForSeconds(duration);
        StopCoroutine();
    }

    public override void EnterCondition()
    {
        
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

    public IEnumerator ConditionEffect(float DataValue)
    {
        yield return null;
    }
}
