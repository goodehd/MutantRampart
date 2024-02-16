using System;
using System.Collections;
using System.Collections.Generic;
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

    public void OnAttackInvoke()
    {
        OnAttack?.Invoke();
    }
}
