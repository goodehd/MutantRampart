public enum EUIEventState
{
    Click,
    Hovered,
    Exit,
    Max
}

public enum ESoundType
{
    BGM,
    Effect,
    UI
}

public enum EstatType
{
    Hp,
    Damage,
    Defense,
    AttackSpeed,
    MoveSpeed,
    Max
}

public enum EState
{
    Idle,
    Move,
    Attack,
    Dead
}

public enum EUIstate
{
    Main,
    ChangeTileSelect,
    ChangeUnitAndRoom
}

public enum EStatusformat
{
    Bat,
    Trap,
    Home,
    DefaultTile,
    Count
}

[System.Flags]
public enum Condition
{
    None = 0,
    Healing = 1 << 0,
    Frostbite = 1 << 1,
    Frozen = 1 << 2,
    AttackUp = 1 << 3,
    HpUp = 1 << 4,
    AtkSpeedUp = 1 << 5,
    DefUp = 1 << 6,
    Fool = 1 << 7,
}
