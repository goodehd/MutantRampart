using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

public class DataManager : IManagers
{ 
    //public Dictionary<string, Data를 상속받는 Data클래스> --Data = new();
    public Dictionary<string, EnemyData> enemy = new Dictionary<string, EnemyData>();

    public CSVReader reader = new();
    
    public bool Init()
    {
        
        return true;
        
    }
    public void Initialize() {
        //--Data = LoadJson<Data를 상속받는 Data클래스>();
        enemy = reader.LoadToCSVData<EnemyData>();

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
