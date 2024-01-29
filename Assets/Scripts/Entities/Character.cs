using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public int CurPosX { get; set; }
    public int CurPosY { get; set; }
    public Room CurRoom { get; set; }
    public bool IsDead { get; set; }

    public CharacterBehaviour Owner { get; set; }
    public CharacterData Data { get; private set; }
    public CharacterStatus Status { get; private set; }

    public Character(CharacterData data)
    {
        Data = data;
        Status = new CharacterStatus(data);

        IsDead = false;

        CurPosX = -1;
        CurPosY = -1;
    }

    public void Init(CharacterData data)
    {
        Data = data;
        Status = new CharacterStatus(data);

        IsDead = false;

        CurPosX = -1;
        CurPosY = -1;
    }
}
