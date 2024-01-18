using System;

public interface ISingleStatus
{
    float Value { get; }
    event Action<float> OnValueChanged;

    void AddValue(float amount);
    void SubValue(float amount);

    bool CheckNegativeNumber(float value);
}
