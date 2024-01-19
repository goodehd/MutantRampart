using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    protected Character Owner { get; private set; }

    public BaseState(Character owner)
    {
        Owner = owner;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}
