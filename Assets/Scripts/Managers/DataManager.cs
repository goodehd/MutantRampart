using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

public class DataManager : IManagers
{ 
    //public Dictionary<string, Data를 상속받는 Data클래스> --Data = new();
    public Dictionary<string, CharacterData> enemy = new Dictionary<string, CharacterData>();

    public CSVReader reader = new();
    
    public bool Init()
    {
        enemy = reader.LoadToCSVData<CharacterData>();
        return true;
    }

    public void Initialize() {
        //--Data = LoadJson<Data를 상속받는 Data클래스>();
        //enemy = reader.LoadToCSVData<CharacterData>();
    }

    private Dictionary<string, T> LoadJson<T>() where T : Data
    {
        
        return null;
        
        /*
        TextAsset textAsset = Main.Resource.LoadJsonData(typeof(T).Name);
        //클래스의 이름을 키값으로(파일명) 제이슨을 받아오는

        Dictionary<string, T> dic = JsonConvert.DeserializeObject<List<T>>(textAsset.text).ToDictionary(data => data.Key);

        return dic;
        */
    }
    
}
