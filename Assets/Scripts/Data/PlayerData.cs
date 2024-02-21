using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData 
{
    public string Name;
    public int Curstage;
    public int PlayerMoney;
    public float PlayerHP;
    public int MapSizeX;
    public int MapSizeY;
    public List<CharacterSavableData> PlayerUnitsSaveData = new List<CharacterSavableData>();
    public List<RoomSavableData> PlayerRoomsSaveData = new List<RoomSavableData>();
    public List<ItemSavableData> PlayerItemsSaveData = new List<ItemSavableData>();
    public List<RoomDirSavableData> PlayerRoomsDirSaveData = new List<RoomDirSavableData>();
}


