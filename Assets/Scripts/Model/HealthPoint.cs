using System.Collections;
using System.Collections.Generic;

public class HealthPoint
{
    private Stat _value;
    private Stat _maxValue;

    public HealthPoint(float value, float maxValue)
    {
        _value = new Stat(EstatType.Max, maxValue);
        _maxValue = new Stat(EstatType.Max, maxValue);
    }

    public HealthPoint(float value)
    {
        _value = new Stat(EstatType.Max, value);
        _maxValue = new Stat(EstatType.Max, value);
    }


}
