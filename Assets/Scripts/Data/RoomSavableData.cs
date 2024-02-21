using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoomSavableData
{
    public int IndexX;
    public int IndexY;
    public bool isEquiped;
    public string RoomName;
    public Vector2 Pos;
    

    public RoomSavableData(Room room) 
    {
        IndexX = room.IndexX;
        IndexY = room.IndexY;
        isEquiped = room.IsEquiped;
        RoomName = room.Data.Key;
        Pos = room.Pos;
    }
}

[Serializable]
public class RoomDirSavableData
{
    public int IndexX;
    public int IndexY;
    public ERoomDir RoomDir;

    public RoomDirSavableData(RoomBehavior room)
    {
        IndexX = room.IndexX;
        IndexY = room.IndexY;
        RoomDir = room.RoomDir;
    }
}