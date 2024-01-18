using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : IStatus
{
    [SerializeField] private float _curValue;
    [SerializeField] private float _maxValue;

    public float CurValue { get { return _curValue; } }
    public float MaxValue { get { return _maxValue; } } 

    public event Action<float, float> OnValueChanged;

    public Status(float value, float maxValue)
    {
        _curValue = value;
        _maxValue = maxValue;
    }

    public void AddValue(float value)
    {
        if(CheckNegativeNumber(value))
        {
            return;
        }

        float newValue = Mathf.Min(_curValue + value, _maxValue);

        ValueChangedHandle(newValue, _maxValue);
    }

    public void SubValue(float value)
    {
        if (CheckNegativeNumber(value))
        {
            return;
        }

        float newValue = Mathf.Max(_curValue - value, 0f);

        ValueChangedHandle(newValue, _maxValue);
    }

    public void AddMaxValue(float value)
    {
        if (CheckNegativeNumber(value))
        {
            return;
        }

        float newValue = Mathf.Min(_maxValue + value, 100000f);

        ValueChangedHandle(_curValue, newValue);
    }

    public void SubMaxValue(float value)
    {
        if (CheckNegativeNumber(value))
        {
            return;
        }

        float newValue = Mathf.Max(_maxValue - value, 0f);

        ValueChangedHandle(_curValue, newValue);
    }

    public bool CheckNegativeNumber(float newValue)
    {
        return newValue < 0f;
    }

    public float GetPercentage()
    {
        return _curValue / _maxValue;
    }

    private void ValueChangedHandle(float newCurValue, float newMaxValue)
    {
        bool curValueChanged = !Mathf.Approximately(_curValue, newCurValue);
        bool maxValueChanged = !Mathf.Approximately(_maxValue, newMaxValue);

        if (!curValueChanged && !maxValueChanged)
        {
            return;
        }

        _curValue = newCurValue;
        _maxValue = newMaxValue;
        OnValueChanged?.Invoke(_curValue, _maxValue);
    }
}
