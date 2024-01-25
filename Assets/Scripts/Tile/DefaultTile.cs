using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DefaultTile : Room
{
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        _roomStatus = EStatusformat.DefaultTile;
        ThisRoomData = Main.Get<DataManager>().roomDatas["Default"];
        ThisRoomData.isEquiped = false;
        return true;
    }
}
