using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThisRoom 
{
    public event Action OnEquipedEvenet;
    public event Action OnUnEquipedEvenet;

    public int IndexX { get; set; }
    public int IndexY { get; set; }
    public bool IsEquiped { get; set; }
    public RoomBehavior Owner { get; set; }
    public RoomData Data { get; private set; }
    public LinkedList<CharacterBehaviour> Enemys { get; set; } = new LinkedList<CharacterBehaviour>();

    public ThisRoom() { }
    public ThisRoom(RoomData data) 
    {
        Data = data;

        IsEquiped = false;

        IndexX = 0; IndexY = 0;
    }

    public void Init(RoomData data)
    {
        Data = data;

        IsEquiped = false;

        IndexX = 0; IndexY = 0;
    }

    public void EquipedRoom()
    {
        IsEquiped = true;
        OnEquipedEvenet?.Invoke();
    }

    public void UnEquipedRoom()
    {
        IsEquiped = false;
        OnUnEquipedEvenet?.Invoke();
    }
}
