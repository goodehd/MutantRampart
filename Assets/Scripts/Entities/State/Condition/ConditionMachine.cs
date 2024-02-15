using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionMachine
{

    private List<BaseCondition> _curConditionList = new List<BaseCondition>();
    
    public void AddCondition(BaseCondition basecondition)
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
