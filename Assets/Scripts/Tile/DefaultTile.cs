using System;
using System.Collections;
using UnityEngine;

public class DefaultTile : Room
{
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        _roomStatus = EStatusformat.DefaultTile;
        ThisRoomData = Main.Get<DataManager>().roomDatas["Default"];
        return true;
    }
}
