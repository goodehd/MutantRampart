using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public int CurPosX { get; set; }
    public int CurPosY { get; set; }
    public RoomBehavior CurRoom { get; set; }
    public bool IsDead { get; set; }

    public CharacterBehaviour Owner { get; set; }
    public CharacterData Data { get; private set; }
    public CharacterStatus Status { get; private set; }


    public Item[] Item { get; set; } = new Item[3];
    public int[] itemnumbers = new int[3];
    public event Action OnAttack;
    

    public Character(CharacterData data)
    {
        Data = data;
        Status = new CharacterStatus(data);

        IsDead = false;

        CurPosX = -1;
        CurPosY = -1;
    }


    public void Init(CharacterData data)
    {
        Data = data;
        Status = new CharacterStatus(data);
        
        IsDead = false;

        CurPosX = -1;
        CurPosY = -1;
        
    }

    public T EquipItem<T>() where T : Item //아이템 클래스가 다양해지면?
    {
        T empty = Activator.CreateInstance<T>();

        return empty;
    }

    public void OnAttackInvoke()
    {
        OnAttack?.Invoke();
    }

    
}
