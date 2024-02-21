using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Character
{
    public event Action Ondeploy;
    public event Action OnRecall;

    private RoomBehavior _curRoom;
    public RoomBehavior CurRoom 
    {
        get 
        {
            return _curRoom;
        }
        set
        {
            if(value != null)
            {
                Ondeploy?.Invoke();
            }
            else
            {
                OnRecall?.Invoke();
            }
            _curRoom = value;
        } 
    }

    public int CurPosX { get; set; }
    public int CurPosY { get; set; }
    public int CurIndex { get; set; }
    public bool IsDead { get; set; }
    public bool isEquiped { get; set; }

    public CharacterBehaviour Owner { get; set; }
    public CharacterData Data { get; private set; }
    public SkillData SkillData { get; private set; }
    public CharacterStatus Status { get; private set; }

    public Item[] Item { get; set; } = new Item[3];
    public int[] itemnumbers = new int[3];

    public event Action<CharacterBehaviour> OnAttackState;

    public Character(CharacterData data)
    {
        Data = data;
        SkillData = Main.Get<DataManager>().GetSkill(data.Key);

        Status = new CharacterStatus(data);

        IsDead = false;
        isEquiped = false;

        CurPosX = -1;
        CurPosY = -1;
        CurIndex = -1;
    }

    public void InvokeAttackAction(CharacterBehaviour target)
    {
        OnAttackState?.Invoke(target);
    }

    public float CalculateSkillValue()
    {
        return SkillData.BaseValue + Status[EstatType.Damage].Value * SkillData.AttakcCoefficient +
            Status[EstatType.Defense].Value * SkillData.DefenseCoefficient + Status[EstatType.AttackSpeed].Value * SkillData.AttackSpeedCoefficient;
    }

    public CharacterSavableData CreateSavableUnitData()
    {
        return new CharacterSavableData(this);
    }
}
