using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleBuffCondition : BaseCondition
{
    private Coroutine _mainRoutine;

    private float _upgradeValue_1 { get; set; }
    private float _upgradeValue_2 { get; set; }
    private float _upgradeValue_3 { get; set; }

    public TempleBuffCondition(CharacterBehaviour owner, RoomData data) : base(owner, data)
    {
        OwnerData = data;
        Duration = data.Duration;
        _upgradeValue_1 = data.UpgradeValue_1;
        _upgradeValue_2 = data.UpgradeValue_2;
        _upgradeValue_3 = data.UpgradeValue_3;
        ConditionName = ECondition.TempleBuff;
    }

    public override void EnterCondition()
    {
        Main.Get<StageManager>().OnStageStartEvent += StartEffect;
        Main.Get<StageManager>().OnStageClearEvent += ClearEffect;
    }

    public override void ExitCondition()
    {
        ClearEffect(0);
        Main.Get<StageManager>().OnStageStartEvent -= StartEffect;
        Main.Get<StageManager>().OnStageClearEvent -= ClearEffect;
    }

    public void StartEffect(int starge)
    {
        _mainRoutine = Owner.StartCoroutine(ConditionEffect(_upgradeValue_1, _upgradeValue_2));
    }

    public void ClearEffect(int starge)
    {
        if(_mainRoutine != null)
        {
            Owner.StopCoroutine(_mainRoutine);
            _mainRoutine = null;
        }
        Owner.Status.GetStat<Vital>(EstatType.Hp).RemoveAllModifier(this);
        Owner.Status.GetStat<Stat>(EstatType.Defense).RemoveAllModifier(this);
        Owner.Status.GetStat<Stat>(EstatType.Damage).RemoveAllModifier(this);
        Owner.Status.GetStat<Stat>(EstatType.AttackSpeed).RemoveAllModifier(this);

        if (!Main.Get<StageManager>().GetIsStageStart())
        {
            Owner.ResetCharacter();
        }
    }

    public IEnumerator ConditionEffect(float DataValue, float DataValue2)
    {
        StatModifier Reduction = new StatModifier((100 - DataValue) * 0.01f, EStatModType.Multip, 1, this);

        Owner.Status.GetStat<Vital>(EstatType.Hp).AddModifier(Reduction);
        Owner.Status.GetStat<Stat>(EstatType.Defense).AddModifier(Reduction);
        Owner.Status.GetStat<Stat>(EstatType.Damage).AddModifier(Reduction);
        Owner.Status.GetStat<Stat>(EstatType.AttackSpeed).AddModifier(Reduction);

        StatModifier Increasehp = new StatModifier(Owner.Status.GetStat<Vital>(EstatType.Hp).Value * DataValue2*0.01f, EStatModType.Add, 1, this);
        StatModifier Increasede = new StatModifier(Owner.Status.GetStat<Stat>(EstatType.Defense).Value * DataValue2*0.01f, EStatModType.Add, 1, this);
        StatModifier Increaseda = new StatModifier(Owner.Status.GetStat<Stat>(EstatType.Damage).Value * DataValue2*0.01f, EStatModType.Add, 1, this);
        StatModifier Increaseat = new StatModifier(Owner.Status.GetStat<Stat>(EstatType.AttackSpeed).Value * DataValue2*0.01f, EStatModType.Add, 1, this);

        while (true)
        {
            Owner.Status.GetStat<Vital>(EstatType.Hp).AddModifier(Increasehp);
            Owner.Status.GetStat<Stat>(EstatType.Defense).AddModifier(Increasede);
            Owner.Status.GetStat<Stat>(EstatType.Damage).AddModifier(Increaseda);
            Owner.Status.GetStat<Stat>(EstatType.AttackSpeed).AddModifier(Increaseat);
            yield return new WaitForSeconds(3f);
        }
    }
}
