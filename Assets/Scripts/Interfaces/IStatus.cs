using System;

public interface IStatus
{
    float CurValue { get; }
    float MaxValue { get; }

    event Action<float, float> OnValueChanged;

    void AddValue(float value);
    void SubValue(float value);

    void AddMaxValue(float value);
    void SubMaxValue(float value);

    bool CheckNegativeNumber(float value);
    float GetPercentage();
}