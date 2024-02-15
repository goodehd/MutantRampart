using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EConditionpersonality
{
    Buff,
    debuff
}

public abstract class BaseCondition
{
    protected CharacterBehaviour Owner { get; private set; }
    protected Data OwnerData { get; set; }
    protected float Duration { get; set; }

    protected EConditionpersonality Conditionpersonality;

    public event Action<BaseCondition> OnEndCondition;
    
    public BaseCondition(CharacterBehaviour owner, Data data)
    {
        Owner = owner;
        OwnerData = data;
    }
    public BaseCondition(CharacterBehaviour owner, float duration)
    {
        Owner = owner;
        Duration = duration;
    }

    public abstract void EnterCondition();
    public abstract void UpdateCondition();
    public abstract void ExitCondition();
    public abstract void StopCoroutine();

    public abstract IEnumerator ConditionDuration(float duration);

    public void InvokeEndCondition()
    {
        OnEndCondition?.Invoke(this);
    }

}