using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EItemType
{
    Weapon,
    Armor,
    Consume,
    Count,
    ExpandMap
}

public class Item
{
    public ItemData EquipItemData;
    public bool IsEquiped { get; set; }

    public int ItemIndex;

    public Character Owner;

    public Item() { }
    public Item(Item item)
    {
        EquipItemData = item.EquipItemData;
        Owner = item.Owner;
        IsEquiped = item.IsEquiped;
    }

    public void Init(ItemData data)
    {
        EquipItemData = data;
        IsEquiped = false;
    }

    public virtual Item Clone()
    {
        return new Item(this);
    }

    public virtual void EquipItem(Character data)
    {
        if (EquipItemData == null) return;
        Owner = data;
        StatModifier HPADD = new StatModifier(EquipItemData.HpAdd, EStatModType.Add, 1, this);
        StatModifier DFADD = new StatModifier(EquipItemData.DefenseAdd, EStatModType.Add, 1, this);
        StatModifier ATADD = new StatModifier(EquipItemData.AttackAdd, EStatModType.Add, 1, this);
        StatModifier SPADD = new StatModifier(EquipItemData.SpeedAdd, EStatModType.Add, 1, this);

        data.Status.GetStat<Vital>(EstatType.Hp).AddModifier(HPADD);
        data.Status.GetStat<Stat>(EstatType.Defense).AddModifier(DFADD);
        data.Status.GetStat<Stat>(EstatType.Damage).AddModifier(ATADD);
        data.Status.GetStat<Stat>(EstatType.AttackSpeed).AddModifier(SPADD);
        data.Status.GetStat<Vital>(EstatType.Hp).CurValue = data.Status.GetStat<Vital>(EstatType.Hp).Value;
        IsEquiped = true;
        data.OnAttackState += AttackEffect;

    }

    public virtual void UnEquipItem(Character data)
    {
        data.Status.GetStat<Vital>(EstatType.Hp).RemoveAllModifier(this);
        data.Status.GetStat<Stat>(EstatType.Defense).RemoveAllModifier(this);
        data.Status.GetStat<Stat>(EstatType.Damage).RemoveAllModifier(this);
        data.Status.GetStat<Stat>(EstatType.AttackSpeed).RemoveAllModifier(this);
        IsEquiped = false;
        data.OnAttackState -= AttackEffect;
    }

    public virtual void AttackEffect(CharacterBehaviour target)
    {
        Debug.Log("엄마아아아악");
    }

    public virtual void IdentityEffect(int stage)
    {
        Debug.Log("엄마");
    }

}

public class SilverCoin : Item
{
    public SilverCoin() { }
    public SilverCoin(Item item) : base(item)
    {

    }

    public override Item Clone()
    {
        return new SilverCoin(this);
    }

    public override void EquipItem(Character data)
    {
        base.EquipItem(data);
    }

    public override void UnEquipItem(Character data)
    {
        base.UnEquipItem(data);

    }

    public override void AttackEffect(CharacterBehaviour target)
    {
        Main.Get<GameManager>().ChangeMoney(-1);
    }

}

public class GoldenCoin : Item
{
    public GoldenCoin() { }
    public GoldenCoin(Item item) : base(item)
    {

    }
    public override Item Clone()
    {
        return new GoldenCoin(this);
    }

    public override void EquipItem(Character data)
    {
        base.EquipItem(data);
        Main.Get<StageManager>().OnStageClearEvent += IdentityEffect;
    }

    public override void UnEquipItem(Character data)
    {
        base.UnEquipItem(data);
        Main.Get<StageManager>().OnStageClearEvent -= IdentityEffect;
    }

    public override void IdentityEffect(int stage)
    {
        Main.Get<GameManager>().ChangeMoney(+1000);
    }

}

public class SilverBar : Item //피격은 아직 구현이;;
{
    public SilverBar() { }
    public SilverBar(Item item) : base(item)
    {

    }
    public override Item Clone()
    {
        return new SilverBar(this);
    }

    public override void EquipItem(Character data)
    {
        base.EquipItem(data);
    }

    public override void UnEquipItem(Character data)
    {
        base.UnEquipItem(data);

    }
}

public class GoldBar : Item
{
    private int _attackCount = 5;
    StatModifier DamageAdd = new StatModifier(2f, EStatModType.Multip, 2);

    public GoldBar() { }
    public GoldBar(Item item) : base(item)
    {

    }
    public override Item Clone()
    {
        return new GoldBar(this);
    }
    public override void EquipItem(Character data)
    {
        base.EquipItem(data);
        _attackCount = 5;
        Owner.Status.GetStat<Stat>(EstatType.Damage).AddModifier(DamageAdd);
        Main.Get<StageManager>().OnStageClearEvent += IdentityEffect;
    }

    public override void UnEquipItem(Character data)
    {
        base.UnEquipItem(data);
        Owner.Status.GetStat<Stat>(EstatType.Damage).RemoveModifier(DamageAdd);
        Main.Get<StageManager>().OnStageClearEvent -= IdentityEffect;
    }

    public override void AttackEffect(CharacterBehaviour target)
    {
        if (_attackCount <= 0)
        {
            Owner.Status.GetStat<Stat>(EstatType.Damage).RemoveModifier(DamageAdd);
            return;
        }
        _attackCount--;
    }
    public override void IdentityEffect(int stage)
    {
        _attackCount = 5;
        Owner.Status.GetStat<Stat>(EstatType.Damage).RemoveModifier(DamageAdd);
        Owner.Status.GetStat<Stat>(EstatType.Damage).AddModifier(DamageAdd);
    }

}

public class FrozenTuna : Item
{
    public FrozenTuna() { }
    public FrozenTuna(Item item) : base(item)
    {

    }
    public override Item Clone()
    {
        return new FrozenTuna(this);
    }

    public override void EquipItem(Character data)
    {
        base.EquipItem(data);
    }

    public override void UnEquipItem(Character data)
    {
        base.UnEquipItem(data);
    }

    public override void AttackEffect(CharacterBehaviour target)
    {
        if (!target.ConditionMachine.CheckCondition(ECondition.Frostbite))
        {
            target.ConditionMachine.AddCondition(new FrostbiteCondition(target, this.EquipItemData));
        }
    }

}

public class Feather : Item
{
    public Feather() { }
    public Feather(Item item) : base(item)
    {

    }
    public override Item Clone()
    {
        return new Feather(this);
    }

    public override void EquipItem(Character data)
    {
        base.EquipItem(data);
    }

    public override void UnEquipItem(Character data)
    {
        base.UnEquipItem(data);
    }

    public override void AttackEffect(CharacterBehaviour target)
    {
        float a = Random.Range(0, 100);
        if (a < 30)
        {
            target.Status.GetStat<Vital>(EstatType.Hp).CurValue -= Owner.Status[EstatType.Damage].Value;
        }
    }
}



