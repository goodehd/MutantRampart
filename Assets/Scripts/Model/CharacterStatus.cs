public class CharacterStatus
{
    public Status Hp { get; private set; }
    public SingleStatus Atk { get; private set; }
    public SingleStatus Def { get; private set; }

    public CharacterStatus(float hp, float maxHp, float atk, float def)
    {
        Hp = new Status(hp, maxHp);
        Atk = new SingleStatus(atk);
        Def = new SingleStatus(def);
    }

    public CharacterStatus(CharacterStatus status)
    {
        Hp = new Status(status.Hp.CurValue, status.Hp.MaxValue);
        Atk = new SingleStatus(status.Atk.Value);
        Def = new SingleStatus(status.Def.Value);
    }
}
