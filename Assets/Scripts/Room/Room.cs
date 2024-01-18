using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EStatusformat
{
    Bat,
    Trap,
    Count
}

public class Room : MonoBehaviour
{
    protected int[] thisRoomNum = new int[2]; //tile에서 2중배열의 위치를 각각 룸이 가지고 있음 ex) tile[0,1] == tishRoomNum{0,1}
    protected EStatusformat roomStatus;
    
    public event Action<GameObject> OnEnemyEnterRoom; //임시로 GameObject를 넣어둠
     
    private bool _initialized;
    public virtual void Awake()
    {
        Initialize();
    }

    public virtual bool Initialize()
    {
        if (_initialized) return false;
        
        _initialized = true;
        return true;
    }

    public virtual void EnemyEnterRoom(GameObject g) //매개변수로 Enemy스크립트가 들어갈듯? 혹은 움직임 로직 관련 (이벤트에도)
    {
        // thisRoomNum을 리턴시켜줌(어떤 형식으로 보내줄지는 미정)
        // Enemy가 방을 기억하고 다시 올 확률을 낮추기 위해서.
    }

}
