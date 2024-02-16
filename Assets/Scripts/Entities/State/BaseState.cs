using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    protected CharacterBehaviour Owner { get; private set; }

    public BaseState(CharacterBehaviour owner)
    {
        Owner = owner;
    }

    public abstract void Init();
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}
