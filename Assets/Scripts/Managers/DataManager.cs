using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class DataManager : IManagers
{
    public Dictionary<string, CharacterData> Character = new Dictionary<string, CharacterData>();
    public Dictionary<string, RoomData> Room = new Dictionary<string, RoomData>();
    public Dictionary<string, ItemData> Item = new Dictionary<string, ItemData>();
    public Dictionary<string, Item> ItemCDO = new Dictionary<string, Item>();

    public CSVReader reader = new();

    public bool Init()
    {
        Character = reader.LoadToCSVData<CharacterData>();
        Room = reader.LoadToCSVData<RoomData>();
        Item = reader.LoadToCSVData<ItemData>();
        CreateItemCOD();
        return true;
    }

    public void CreateItemCOD()
    {
        foreach (string key in Item.Keys)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type type = assembly.GetType(key);
            Item newItem = null;

            if (type != null)
            {
                newItem = Activator.CreateInstance(type) as Item; 
            }
            else
            {
                newItem = new Item();
            }
            ItemCDO.Add(key, newItem);

        }
    }
}
