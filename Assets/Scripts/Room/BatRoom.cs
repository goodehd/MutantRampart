using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BatRoom : RoomBehavior
{
    private enum EBatType
    {
        Forest,
        Igloo,
        LivingRoom,
        Temple,
        Home_2,
        Count
    }
    
    private EBatType _batType;
    public CharacterBehaviour[] Units { get; private set; } = new CharacterBehaviour[3];
    public int UnitCount { get; set; } 

    public override void Init(RoomData data)
    {
        base.Init(data);

        _batType = Enum.Parse<EBatType>(this.gameObject.name);
        Main.Get<StageManager>().OnStageClear += StageClear;
        UnitCount = 0;
    }

    public override void EnterRoom(Enemy enemy)
    {
        base.EnterRoom(enemy);

        if (UnitCount > 0)
        {
            enemy.Renderer.flipX = false;
            enemy.StateMachine.ChangeState(EState.Attack);
            enemy.transform.position = Literals.EnemyPos[Enemys.Count % 6] + transform.position;
            Enemys.AddLast(enemy);

            foreach (CharacterBehaviour unit in Units)
            {
                if(unit == null)
                {
                    continue;
                }
                unit.StateMachine.ChangeState(EState.Attack);
            }
        }
    }

    public bool CreateUnit(Character data)
    {
        if (UnitCount >= RoomInfo.Data.MaxUnitCount)
            return false;

        int index = 0;
        for( ; index < Units.Length; index++)
        {
            if (Units[index] == null)
                break;
        }

        Units[index] = Main.Get<SceneManager>().Scene.CreateCharacter(data.Data.Key);
        Units[index].SetData(data);
        Units[index].CurRoom = this;
        Units[index].transform.position = Literals.BatPos[index] + transform.position;
        UnitCount++;

        return true;
    }

    public void DeleteUnit(Character data)
    {
        for(int i = 0; i < Units.Length; i++)
        {
            if (Units[i] == null)
                continue;

            if (Units[i].CharacterInfo == data)
            {
                Units[i].CharacterInfo.CurRoom = null;
                Main.Get<ResourceManager>().Destroy(Units[i].gameObject);
                UnitCount--;
            }
        }
    }

    public void DeleteAllUnit()
    {
        for (int i = 0; i < Units.Length; i++)
        {
            if (Units[i] != null)
            {
                Units[i].CharacterInfo.CurRoom = null;
                Main.Get<ResourceManager>().Destroy(Units[i].gameObject);
            }
        }
        UnitCount = 0;
    }

    private void StageClear()
    {
        int count = 0;
        for (int i = 0; i < Units.Length; i++)
        {
            if (Units[i] != null)
            {
                Units[i].gameObject.SetActive(true);
                Units[i].Status.GetStat<Vital>(EstatType.Hp).CurValue = Units[i].Status.GetStat<Vital>(EstatType.Hp).Value;
                count++;
            }
        }
        UnitCount = count;
    }

    private void BuffUnitRoomEBat(Character data)
    {
        switch (_batType)
        {
            case EBatType.Forest:
                StartCoroutine(ForestBuff(data));
                break;
            case EBatType.Igloo:
                IglooBuff(data);
                break;
            case EBatType.LivingRoom:
                LivingRoomBuff(data);
                break;
            case EBatType.Temple:
                TempleBuff(data);
                break;
            case EBatType.Home_2:
                
                break;
            default:
                break;
        }
    }
    

    private IEnumerator ForestBuff(Character data)
    {
        while (!data.IsDead)
        {
            data.Status.GetStat<Vital>(EstatType.Hp).CurValue += 1f;
            yield return new WaitForSeconds(1f);
        }
    }

    private void IglooBuff(Character data)
    {
        StatModifier mod = new StatModifier(10f, EStatModType.Add, 1);
        data.Status.GetStat<Stat>(EstatType.Defense).AddModifier(mod);
    }

    private void LivingRoomBuff(Character data)
    {
        StatModifier mod = new StatModifier(10f, EStatModType.Add, 1);
        data.Status.GetStat<Stat>(EstatType.Damage).AddModifier(mod);
    }

    private void TempleBuff(Character data)
    {
        StatModifier mod = new StatModifier(10f, EStatModType.Add, 1);
        data.Status.GetStat<Stat>(EstatType.AttackSpeed).AddModifier(mod);
    }

    private void Home_2Buff(Character data)
    {
        if (data.Status.GetStat<Vital>(EstatType.Hp).CurValue <= 1f) //아무리봐도 사망처리랑 충돌할 것 같음...
        {
            //data.Status.GetStat<Vital>(EstatType.Hp).AddModifier();
        }
        
        //StatModifier mod = new StatModifier(10f, EStatModType.Add, 1);
        //data.Status.GetStat<Stat>(EstatType.Damage).AddModifier(mod);        1강화
        //data.Status.GetStat<Stat>(EstatType.AttackSpeed).AddModifier(mod);   1강화
        //data.Status.GetStat<Stat>(EstatType.Defense).AddModifier(mod);       2강화
    }

    private void OnDestroy()
    {
        Main.Get<StageManager>().OnStageClear -= StageClear;
        DeleteAllUnit();
    }
}
