using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stat 
{
    protected EstatType _statType;
    protected float _value;
    protected float _baseValue;
    protected bool _isUpdate;
    protected List<StatModifier> _Modifiers = new List<StatModifier>();

    public event Action<float> OnValueChanged;
    public float Value { get { return _value; } }

    public Stat(EstatType statType, float value)
    {
        _statType = statType;
        _value = value;
        _baseValue = value;
        _isUpdate = false;
    }

    public void AddModifier(StatModifier modifier)
    {
        _Modifiers.Add(modifier);
        CalculateValue();
    }

    public void RemoveModifier(StatModifier modifier)
    {
        _Modifiers.Remove(modifier);
        CalculateValue();
    }

    public void RemoveAllModifier(object source)
    {
        _Modifiers.RemoveAll(mod => mod.Source == source);
        CalculateValue();
    }

    protected float CalculateValue()
    {
        float value = _baseValue;

        _Modifiers.Sort((x , y) => y.Order.CompareTo(x.Order));

        for(int i = 0; i < _Modifiers.Count; ++i)
        {
            StatModifier mod = _Modifiers[i];
            if(mod.StatModType == EStatModType.Add)
            {
                value += mod.Value;
            }
            else if(mod.StatModType == EStatModType.Multip)
            {
                value *= mod.Value;
            }
            else if (mod.StatModType == EStatModType.Override)
            {
                value = mod.Value;
            }
        }

        bool curValueChanged = !Mathf.Approximately(value, _value);

        if (!curValueChanged)
            return _value;

        _value = value;
        OnValueChanged?.Invoke(_value);

        return _value;
    }
}
