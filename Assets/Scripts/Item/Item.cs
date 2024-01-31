using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EItemType
{
    Weapon,
    Armor,
    Consume,
    Count
}

public class Item : MonoBehaviour
{
    public ItemData EquipItemData;

    public void Init(ItemData data)
    {
        EquipItemData = data;
    }

    public virtual void EquipItem(Character data)
    {
        if(EquipItemData == null)return;
        
        StatModifier HPADD = new StatModifier(EquipItemData.HpAdd, EStatModType.Add, 1, this);
        StatModifier DFADD = new StatModifier(EquipItemData.DefenseAdd, EStatModType.Add, 1 , this);
        StatModifier ATADD = new StatModifier(EquipItemData.AttackAdd, EStatModType.Add, 1, this);
        StatModifier SPADD = new StatModifier(EquipItemData.SpeedAdd, EStatModType.Add, 1, this);
     
        data.Status.GetStat<Vital>(EstatType.Hp).AddModifier(HPADD);
        data.Status.GetStat<Stat>(EstatType.Defense).AddModifier(DFADD);
        data.Status.GetStat<Stat>(EstatType.Damage).AddModifier(ATADD);
        data.Status.GetStat<Stat>(EstatType.AttackSpeed).AddModifier(SPADD);
        
    }

    public virtual void UnEquipItem(Character data)
    {
        data.Status.GetStat<Vital>(EstatType.Hp).RemoveAllModifier(this);
        data.Status.GetStat<Stat>(EstatType.Defense).RemoveAllModifier(this);
        data.Status.GetStat<Stat>(EstatType.Damage).RemoveAllModifier(this);
        data.Status.GetStat<Stat>(EstatType.AttackSpeed).RemoveAllModifier(this);
    }

    public virtual void AttackEffect()
    {
        Debug.Log("엄마");
    }

    public virtual void EndEffect(int stage)
    {
        Debug.Log("엄마");
    }
}

public class SilverCoinItem : Item
{
    public override void EquipItem(Character data)
    {
        base.EquipItem(data);
        data.OnAttack += AttackEffect;
    }

    public override void UnEquipItem(Character data)
    {
        base.EquipItem(data);
        data.OnAttack -= AttackEffect;
    }
    
    public override void AttackEffect()
    {
        Main.Get<GameManager>().ChangeMoney(-1);
    }
    
}

public class GoldCoinItem : Item
{
    public override void EquipItem(Character data)
    {
        base.EquipItem(data);
        Main.Get<StageManager>().OnStageClearEvent += EndEffect;
    }

    public override void UnEquipItem(Character data)
    {
        base.UnEquipItem(data);
        Main.Get<StageManager>().OnStageClearEvent -= EndEffect;
    }

    public override void EndEffect(int stage)
    {
        Main.Get<GameManager>().ChangeMoney(+1000);
    }
    
}

public class SilverBarItem : Item //피격은 아직 구현이;;
{
    public override void EquipItem(Character data)
    {
        base.EquipItem(data);
        
    }

    public override void UnEquipItem(Character data)
    {
        base.UnEquipItem(data);
        
    }
}

public class GoldBarItem : Item
{
    private int _attackCount = 5;
    public override void EquipItem(Character data)
    {
        base.EquipItem(data);
        data.OnAttack += AttackEffect;
    }

    public override void UnEquipItem(Character data)
    {
        base.EquipItem(data);
        data.OnAttack -= AttackEffect;
    }
    
    public override void AttackEffect()
    {
        if (_attackCount <= 0)return;
        _attackCount--;
    }
}


