using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : BaseState
{
    public DeadState(CharacterBehaviour owner) : base(owner)
    {
    }

    public override void EnterState()
    {
        CoroutineManagement.Instance.StartCoroutine(DieObject());
        Owner.Animator.SetTrigger(Literals.Dead);
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {

    }

    private IEnumerator DieObject()
    {
        yield return new WaitForSeconds(1);
        Main.Get<ResourceManager>().Destroy(Owner.gameObject);
    }
}
