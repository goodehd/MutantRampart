using System.Collections;
using System.Collections.Generic;
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
    public int SlotIndex;

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
        data.Status.GetStat<Vital>(EstatType.Hp).CurValue = data.Status.GetStat<Vital>(EstatType.Hp).Value;
        IsEquiped = false;
        Owner = null;
        data.OnAttackState -= AttackEffect;
    }

    public virtual void AttackEffect(List<CharacterBehaviour> target)
    {

    }

    public virtual void IdentityEffect(int stage)
    {
    }

    public ItemSavableData CreateSavableItemData()
    {
        return new ItemSavableData(this);
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

    public override void AttackEffect(List<CharacterBehaviour> target)
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
        Main.Get<GameManager>().ChangeMoney(+250);
    }

}

public class SilverBar : Item
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
        Main.Get<StageManager>().OnStageClearEvent += IdentityEffect;
    }

    public override void UnEquipItem(Character data)
    {
        Main.Get<StageManager>().OnStageClearEvent -= IdentityEffect;
        base.UnEquipItem(data);
    }

    public override void AttackEffect(List<CharacterBehaviour> target)
    {
        if (_attackCount <= 0) return;
        foreach(CharacterBehaviour behaviour in target)
        {
            behaviour.TakeDamage(Owner.Status[EstatType.Damage].Value*2);
        }
        _attackCount--;
    }
    public override void IdentityEffect(int stage)
    {
        _attackCount = 5;
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

    public override void AttackEffect(List<CharacterBehaviour> target)
    {
        foreach(CharacterBehaviour behaviour in target)
        {
            if (!behaviour.ConditionMachine.CheckCondition(ECondition.Frostbite))
            {
                behaviour.ConditionMachine.AddCondition(new FrostbiteCondition(behaviour, this.EquipItemData));
            }
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

    public override void AttackEffect(List<CharacterBehaviour> target)
    {
        float a = Random.Range(0, 100);
        if (a < 30)
        {
            foreach (CharacterBehaviour behaviour in target)
            {
                behaviour.TakeDamage(Owner.Status[EstatType.Damage].Value);
            }
            Owner.Status.GetStat<Vital>(EstatType.Mp).CurValue += Owner.Data.HitRecoveryMp;
        }
    }
}



