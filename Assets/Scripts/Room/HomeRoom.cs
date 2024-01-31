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
       
       Debug.Log("생김");
     }
    
    public override void EnterRoom(Enemy enemy)
    {
        Debug.Log("집침입");
        base.EnterRoom(enemy);
        enemy.Die();
        Main.Get<GameManager>().PlayerHP.CurValue--;
    }
}
