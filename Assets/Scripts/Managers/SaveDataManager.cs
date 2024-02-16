using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveDataManager : IManagers
{
    public string path;

    public PlayerData Player = new PlayerData();

    public bool Init()
    {
        
        path = Application.persistentDataPath + "/save";
      
        return true;
    }

    public void SaveData()
    {
        string data = JsonUtility.ToJson(Player);
        File.WriteAllText(path, data);
    }

    public void LoadData()
    {
        string data = File.ReadAllText(path);
        Player = JsonUtility.FromJson<PlayerData>(data);
        Debug.Log(Player.Name);
    }

    public void ClearData()
    {
        Player = new PlayerData();
    }

    public void DeleteData()
    {
        File.Delete(path); 
    }

}
