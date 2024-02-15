using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeRoom : RoomBehavior
{
    public int damageAmount = 10;

    public override void Init(RoomData data)
    {
        base.Init(data);
    
       isEndPoint = true;
       
     }
    
    public override void EnterRoom(Enemy enemy)
    {
        base.EnterRoom(enemy);
        Main.Get<GameManager>().PlayerHP.CurValue--;
        enemy.Die();
    }
}
