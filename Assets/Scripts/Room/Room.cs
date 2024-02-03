using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room 
{
    public event Action OnEquipedEvent;
    public event Action OnUnEquipedEvent;

    public int IndexX { get; set; }
    public int IndexY { get; set; }
    public bool IsEquiped { get; set; }
    public RoomBehavior Owner { get; set; }
    public RoomData Data { get; private set; }
    public LinkedList<CharacterBehaviour> Enemys { get; set; } = new LinkedList<CharacterBehaviour>();
    public Room() { }
    public Room(RoomData data) 
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
        if(Data.Type == EStatusformat.Home)
        {
            Main.Get<GameManager>().isHomeSet = true;
        }
        OnEquipedEvent?.Invoke();
    }

    public void UnEquipedRoom()
    {
        IsEquiped = false;
        if (Data.Type == EStatusformat.Home)
        {
            Main.Get<GameManager>().isHomeSet = false;
        }
        OnUnEquipedEvent?.Invoke();
    }
}
