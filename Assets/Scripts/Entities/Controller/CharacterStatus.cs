using System;

public class CharacterStatus
{
    public event Action OnStatusChange;
    private Stat[] _stats;

    public CharacterStatus(CharacterData data)
    {
        Init(data);
    }

    public void Init(CharacterData data)
    {
        _stats = new Stat[(int)EstatType.Max];
       //{
       //     new Vital(EstatType.Hp, data.Hp),
       //     new Vital(EstatType.Mp, 0, data.Mp),
       //     new Stat(EstatType.Damage, data.Damage),
       //     new Stat(EstatType.Defense, data.Defense),
       //     new Stat(EstatType.AttackSpeed, data.AttackSpeed),
       //     new Stat(EstatType.MoveSpeed, data.MoveSpeed)
       //};

        CreateVital(EstatType.Hp, data.Hp, data.Hp);
        CreateVital(EstatType.Mp, 0, data.Mp);
        CreateStat(EstatType.Damage, data.Damage);
        CreateStat(EstatType.Defense, data.Defense);
        CreateStat(EstatType.AttackSpeed, data.AttackSpeed);
        CreateStat(EstatType.MoveSpeed, data.MoveSpeed);
    }

    public Stat this[EstatType type]
    {
        get
        {
            return _stats[(int)type];
        }
    }

    public T GetStat<T>(EstatType type) where T : Stat
    {
        return _stats[(int)type] as T;
    }

    private void CreateStat(EstatType type, float value)
    {
        Stat newStat = new Stat(type, value);
        _stats[(int)type] = newStat;
        newStat.OnValueChanged += StatusChange;
    }

    private void CreateVital(EstatType type, float value, float maxValue)
    {
        Vital newVital = new Vital(type, value, maxValue);
        _stats[(int)type] = newVital;
        newVital.OnValueChanged += StatusChange;
    }

    private void StatusChange(float value)
    {
        OnStatusChange?.Invoke();
    }
}
