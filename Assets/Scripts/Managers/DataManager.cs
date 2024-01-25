using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : IManagers
{
    public Dictionary<string, CharacterData> Character = new Dictionary<string, CharacterData>();
    public Dictionary<string, RoomData> roomDatas = new Dictionary<string, RoomData>();

    public CSVReader reader = new();
    
    public bool Init()
    {
        Character = reader.LoadToCSVData<CharacterData>();
        roomDatas = reader.LoadToCSVData<RoomData>();
        return true;
    }
}
