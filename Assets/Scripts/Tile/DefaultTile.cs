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

        return true;
    }
    

    public override void EnemyEnterRoom(GameObject g)
    {
        Debug.Log($"X : {this.IndexX}, Y : {this.IndexY}");
    }

}