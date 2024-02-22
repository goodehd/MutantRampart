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
    public ECondition ConditionName { get; set; }
    
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
    public abstract void ExitCondition();
}
