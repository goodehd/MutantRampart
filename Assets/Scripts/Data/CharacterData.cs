public class CharacterData : Data, INextKey
{
    public float Hp { get; set; }
    public float Mp { get; set; }
    public float HitRecoveryMp { get; set; }
    public float Damage { get; set; }
    public float Defense { get; set; }
    public float AttackSpeed { get; set; }
    public float MoveSpeed { get; set; }
    public EAttackType AttackType { get; set; }
    public int TargetCount { get; set; }
    public int Price { get; set; }
    public string NextKey { get; set; }
    public string PrefabName { get; set; }
}