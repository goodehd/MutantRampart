using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThisRoom 
{
    public int CurPosX { get; set; }
    public int CurPosY { get; set; }
    public bool IsEquiped { get; set; }
    public RoomData Data { get; set; }
    public LinkedList<CharacterBehaviour> Enemys { get; set; } = new LinkedList<CharacterBehaviour>();

    public void Init(RoomData data)
    {
        Data = data;

        IsEquiped = false;

        CurPosX = 0; CurPosY = 0;
    }



}
