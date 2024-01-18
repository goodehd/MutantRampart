public class CharacterStatus
{
    private Stat[] _stats;

    public CharacterStatus(CharacterData data)
    {
        Init(data);
    }

    public void Init(CharacterData data)
    {
        _stats = new Stat[(int)EstatType.Max]
       {
            new Vital(EstatType.Hp, data.Hp),
            new Stat(EstatType.Damage, data.Damage),
            new Stat(EstatType.Defense, data.Defense),
            new Stat(EstatType.AttackSpeed, data.AttackSpeed),
            new Stat(EstatType.MoveSpeed, data.MoveSpeed)
       };
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
}
