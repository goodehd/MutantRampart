using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCondition
{
   protected CharacterBehaviour Owner { get; private set; }
   protected Data OwnerData { get; set; }
   protected float Duration { get; set; }


   public abstract event Action<BaseCondition> OnEndCondition;
   public BaseCondition(CharacterBehaviour owner, Data data)
   {
      Owner = owner;
      OwnerData = data;
   }

   public abstract void EnterCondition();
   public abstract void UpdateCondition();
   public abstract void ExitCondition();
   public abstract void StopCoroutine();

   public abstract IEnumerator ConditionDuration(float duration);
   
}
