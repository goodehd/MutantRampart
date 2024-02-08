using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionMachine
{
    //private Condition _currentCondition = Condition.None;
    private List<BaseCondition> _curConditionList = new List<BaseCondition>();

    #region BitMask
    /*
    public bool HasCondition(Condition condition)
    {
        return (_currentCondition & condition) != 0;
    }
    
    public void AddCondition(Condition condition,BaseCondition basecondition, Data data)
    {
        _currentCondition |= condition;
    }
    
    public void RemoveCondition(Condition condition)
    {
        _currentCondition &= ~condition;
    }
    
    public void ClearConditions()
    {
        _currentCondition = Condition.None;
    }
    */
    #endregion
    
    public void AddCondition(Condition condition,BaseCondition basecondition)
    {
        _curConditionList.Add(basecondition);
        basecondition.EnterCondition();
        basecondition.OnEndCondition += RemoveCondition;
    }
    
    public void RemoveCondition(BaseCondition basecondition)
    {
        _curConditionList.Remove(basecondition);
    }
    public void ClearConditions()
    {
        _curConditionList.Clear();
    }
    
}
