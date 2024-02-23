using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatRoom : RoomBehavior
{
    private enum EBatType
    {
        Forest,
        Igloo,
        Livingroom,
        Temple,
        Count
    }
    
    private EBatType _batType;
    public CharacterBehaviour[] Units { get; private set; } = new CharacterBehaviour[3];
    public int UnitCount { get; set; }
    private int _enemyCount = 0;

    public override void Init(RoomData data)
    {
        base.Init(data);

        _batType = Enum.Parse<EBatType>(this.gameObject.name);
        Main.Get<StageManager>().OnStageClearEvent -= StageClear;
        Main.Get<StageManager>().OnStageClearEvent += StageClear;
        UnitCount = 0;
    }

    public override void EnterRoom(Enemy enemy)
    {
        base.EnterRoom(enemy);

        if (UnitCount > 0)
        {
            enemy.Renderer.flipX = false;
            enemy.StateMachine.ChangeState(EState.Attack);
            enemy.transform.position = Literals.EnemyPos[_enemyCount % 6] + transform.position;
            _enemyCount++;

            foreach (CharacterBehaviour unit in Units)
            {
                if(unit == null || unit.CharacterInfo.IsDead)
                {
                    continue;
                }

                if(unit.StateMachine.CurrentStateName == EState.Attack ||
                    unit.StateMachine.CurrentStateName == EState.Skill)
                {
                    continue;
                }

                unit.StateMachine.ChangeState(EState.Attack);
            }
        }
    }

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        SortCharacter();
    }

    public override void OnDestroyRoom()
    {
        base.OnDestroyRoom();
        DeleteAllUnit();
    }

    public void SortCharacter()
    {
        if(Units != null)
        {
            foreach (CharacterBehaviour unit in Units)
            {
                if (unit != null)
                {
                    // 현재 Unit의 CharacterInfo 가져오기
                    Character characterInfo = unit.CharacterInfo;

                    // CharacterInfo가 playerUnits에 있는지 확인
                    int indexInPlayerUnits = Main.Get<GameManager>().PlayerUnits.IndexOf(characterInfo);
                    int index = Array.IndexOf(Units, unit);
                    if (indexInPlayerUnits != -1)
                    {
                        // 해당 CharacterInfo가 playerUnits에 존재하면 해당 데이터를 인덱스 위치로 옮김
                        Character playerUnit = Main.Get<GameManager>().PlayerUnits[indexInPlayerUnits];
                        Main.Get<GameManager>().PlayerUnits.RemoveAt(indexInPlayerUnits);
                        Main.Get<GameManager>().PlayerUnits.Insert(index, playerUnit);
                    }
                }
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
        Units[index].StateMachine.InitState();
        Units[index].CurRoom = this;
        Units[index].CurPosX = IndexX;
        Units[index].CurPosY = IndexY;
        Units[index].transform.position = Literals.BatPos[index] + transform.position + Units[index].transform.position;
        Units[index].CurIndex = index;
        Units[index].CharacterInfo.isEquiped = true;
        //유닛 체력이 회복되는가 체크 해봤슴 나중에 버프타일 함수에다가 넣으면 된다~~
        ConditionAdd(Units[index]);
        //Units[index].ConditionMachine.AddCondition(new HealingCondition(Units[index],RoomInfo.Data)); //TODO : Condition Test
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
                DestroyUnit(Units[i]);
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
                DestroyUnit(Units[i]);
                Main.Get<ResourceManager>().Destroy(Units[i].gameObject);
            }
        }
        UnitCount = 0;
    }

    private void DestroyUnit(CharacterBehaviour unit)
    {
        unit.CharacterInfo.CurPosX = -1;
        unit.CharacterInfo.CurPosY = -1;
        unit.CharacterInfo.CurRoom = null;
        unit.CharacterInfo.isEquiped = false;
        unit.DestroyUnit();
    }

    private void StageClear(int stage)
    {
        int count = 0;
        for (int i = 0; i < Units.Length; i++)
        {
            if (Units[i] != null)
            {
                if (!Units[i].gameObject.activeSelf)
                {
                    Units[i].gameObject.SetActive(true);
                    ConditionAdd(Units[i]);
                }
                Units[i].ResetCharacter();
                count++;
            }
        }
        UnitCount = count;
        _enemyCount = 0;
    }

    private void ConditionAdd(CharacterBehaviour character)
    {
        switch (_batType)
        {
            case EBatType.Forest:
                character.ConditionMachine.AddCondition(new HealingCondition(character, RoomInfo.Data));
                break;
            case EBatType.Igloo:
                character.ConditionMachine.AddCondition(new FreezingAttackCondition(character, RoomInfo.Data));
                break;
            case EBatType.Livingroom:

                break;
            case EBatType.Temple:
                character.ConditionMachine.AddCondition(new TempleBuffCondition(character, RoomInfo.Data));
                break;
            default:
                break;
        }
    }

    
}
