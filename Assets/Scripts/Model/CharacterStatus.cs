using UnityEngine.PlayerLoop;

public class CharacterStatus
{
    private Stat[] _stats;
    private HealthPoint _healthPoints;

    public CharacterStatus(CharacterData data)
    {
        Init(data);
    }

    public void Init(CharacterData data)
    {
        _stats = new Stat[(int)EstatType.Max]
       {
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


}
