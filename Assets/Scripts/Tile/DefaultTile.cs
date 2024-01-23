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
        //_trapType = Enum.Parse<ETrapType>(this.gameObject.name) ;
        return true;
    }
    /* public bool TryBuildRoomHere(EStatusformat status, out Room room)
     {
         room = null;

         if (Main.Get<TileManager>().CanBuildRoom == false)
         {
             Debug.Log("현재 배치할 수 있는 방의 Count 초과입니다.");
         }


     }*/

}
