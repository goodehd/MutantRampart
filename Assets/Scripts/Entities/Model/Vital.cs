using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vital : Stat
{
    private float _curValue;

    public event Action OnValueZero;
    public event Action OnValueMax;
    public event Action<float> OnCurValueChanged;

    public float CurValue
    {
        get
        {
            return _curValue;
        }
        set 
        {
            if (Mathf.Approximately(_curValue, value)) 
            {
                return;
            }

            if(value <= 0)
            {
                _curValue = 0;
                OnValueZero?.Invoke();
            }
            else if(value >= base.Value)
            {
                _curValue = base.Value;
                OnValueMax?.Invoke();
            }
            else
            {
                _curValue = value;
            }
            OnCurValueChanged?.Invoke(_curValue);
        }
    }

    public Vital(EstatType stateType, float value, float maxValue) : base(stateType, maxValue)
    {
        _curValue = value;
    }

    public Vital(EstatType stateType, float value) : base(stateType, value)
    {
        _curValue = value;
    }

    public float Normalized()
    {
        return CurValue / base.Value;
    }
}
