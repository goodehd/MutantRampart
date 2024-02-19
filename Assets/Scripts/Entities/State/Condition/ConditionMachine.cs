using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionMachine
{
    private List<List<BaseCondition>> _curConditionList = new List<List<BaseCondition>>();

    public ConditionMachine()
    {
        for (int i = 0; i < (int)ECondition.Max; i++)
        {
            _curConditionList.Add(new List<BaseCondition>());
        }
    }


    public void AddCondition(BaseCondition basecondition)
    {
        _curConditionList[(int)basecondition.ConditionName].Add(basecondition);
        basecondition.EnterCondition();
        basecondition.OnEndCondition += RemoveCondition;
    }
    
    public void RemoveCondition(BaseCondition basecondition)
    {
        _curConditionList[(int)basecondition.ConditionName].Remove(basecondition);
        Debug.Log($"{basecondition.ConditionName}");
    }
    
    public void ClearConditions()
    {
        for (int i = 0; i < (int)ECondition.Max; i++)
        {
            _curConditionList[i].Clear();
        }
    }

    public bool CheckCondition(ECondition eCondition)
    {
        return _curConditionList[(int)eCondition].Count > 0;
    }


}
