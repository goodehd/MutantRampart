using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleStatus : ISingleStatus
{
    private float _value;

    public float Value { get { return _value; } }

    public event Action<float> OnValueChanged;

    public SingleStatus(float value)
    {
        _value = value;
    }

    public void AddValue(float value)
    {
        if (CheckNegativeNumber(value))
        {
            return;
        }

        float newValue = Mathf.Min(_value + value, 10000f);

        ValueChangedHandle(newValue);
    }

    public void SubValue(float value)
    {
        if (CheckNegativeNumber(value))
        {
            return;
        }

        float newValue = Mathf.Max(_value - value, 0f);

        ValueChangedHandle(newValue);
    }

    public bool CheckNegativeNumber(float newValue)
    {
        return newValue < 0f;
    }

    private void ValueChangedHandle(float newValue)
    {
        if (Mathf.Approximately(_value, newValue))
        {
            return;
        }

        _value = newValue;
        OnValueChanged?.Invoke(newValue);
    }
}
