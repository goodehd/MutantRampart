using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : BaseState
{
    public DeadState(Character owner) : base(owner)
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
        ////Owner.Animator.GetCurrentAnimatorStateInfo(0).IsName("Right_Die") &&
        //if (
        //    Owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        //{
        //    Main.Get<ResourceManager>().Destroy(Owner.gameObject);
        //}
    }

    private IEnumerator DieObject()
    {
        yield return new WaitForSeconds(1);
        Main.Get<ResourceManager>().Destroy(Owner.gameObject);
    }
}
