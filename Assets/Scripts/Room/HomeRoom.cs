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
       
       Main.Get<GameManager>().isHomeSet = true;
       Debug.Log("생김");
       Debug.Log(Main.Get<GameManager>().isHomeSet);
    }
    
    public override void EnterRoom(Enemy enemy)
    {
        Debug.Log("집침입");
        base.EnterRoom(enemy);
         enemy.Die();
         Main.Get<GameManager>().PlayerHp--;
         
    }

    private void OnDestroy()
    {
        Main.Get<GameManager>().isHomeSet = false;
        Debug.Log("없어짐");
        Debug.Log(Main.Get<GameManager>().isHomeSet);
    }
}
