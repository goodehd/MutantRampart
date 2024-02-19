using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

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
    }

    public void EnemyEnter()
    {
        Main.Get<GameManager>().PlayerHP.CurValue--;
    }
}
