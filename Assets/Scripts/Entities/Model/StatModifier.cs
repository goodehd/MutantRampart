public enum EStatModType
{
    Add,
    Multip,
    Override
}

public class StatModifier
{
    public readonly EStatModType StatModType;
    public readonly float Value;
    public readonly int Order;
    public readonly object Source;

    public StatModifier(float value, EStatModType type, int order, object source)
    {
        Value = value;
        StatModType = type;
        Order = order;
        Source = source;
    }

    public StatModifier(float value, EStatModType type) : this(value, type, (int)type, null) { }
    public StatModifier(float value, EStatModType type, int order) : this(value, type, order, null) { }
    public StatModifier(float value, EStatModType type, object source) : this(value, type, (int)type, source) { }
}
