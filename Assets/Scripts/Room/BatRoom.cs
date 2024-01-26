using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BatRoom : Room
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
    public int UnitCount {  get; set; } 

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        _roomStatus = EStatusformat.Bat;
        OnEnemyEnterRoom += EnemyEnterRoom;
        _batType = Enum.Parse<EBatType>(this.gameObject.name);
        UnitCount = 0;

        return true;
    }

    public override void EnemyEnterRoom(GameObject g)
    {
        base.EnemyEnterRoom(g);

        if (UnitCount > 0)
        {
            CharacterBehaviour enemy = g.GetComponent<CharacterBehaviour>();
            enemy.Renderer.flipX = false;
            enemy.StateMachine.ChangeState(EState.Attack);
            enemy.transform.position = new Vector3(transform.position.x - Enemys.Count * 0.2f, 
                transform.position.y + 1.5f + Enemys.Count * 0.2f, g.transform.position.z);
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

    public void CreateUnit(int slotIndex, Character data)
    {
        if (Units[slotIndex] != null)
        {
            DeleteUnit(slotIndex);
        }

        Units[slotIndex] = Main.Get<SceneManager>().Scene.CreateCharacter(data.Data.Key);
        Units[slotIndex].SetData(data);
        
        Units[slotIndex].transform.position = new Vector3(transform.position.x + 1f, transform.position.y + 2f, 3.0f);
        Units[slotIndex].CurRoom = this;
        UnitCount++;
    }

    public void DeleteUnit(int slotIndex)
    {
        Units[slotIndex].CurRoom = null;

        Main.Get<ResourceManager>().Destroy(Units[slotIndex].gameObject);
        Units[slotIndex] = null;
        UnitCount--;
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
}
