using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : IManagers
{
    public Dictionary<string, CharacterData> Character = new Dictionary<string, CharacterData>();
    public Dictionary<string, RoomData> Room = new Dictionary<string, RoomData>();
    public Dictionary<string, ItemData> Item = new Dictionary<string, ItemData>();
    
    public CSVReader reader = new();
    
    public bool Init()
    {
        Character = reader.LoadToCSVData<CharacterData>();
        Room = reader.LoadToCSVData<RoomData>();
        Item = reader.LoadToCSVData<ItemData>();
        return true;
    }
}
