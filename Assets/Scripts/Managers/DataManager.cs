using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class DataManager : IManagers
{
    public Dictionary<string, CharacterData> Character = new Dictionary<string, CharacterData>();
    public Dictionary<string, RoomData> Room = new Dictionary<string, RoomData>();
    public Dictionary<string, ItemData> Item = new Dictionary<string, ItemData>();
    public Dictionary<string, Item> ItemCDO = new Dictionary<string, Item>();
    private Dictionary<string, SkillData> _skill = new Dictionary<string, SkillData>();
    public List<StageMonsterInfo> stageMonsterInfoList = new List<StageMonsterInfo>();




    public CSVReader reader = new();

    public bool Init()
    {
        Character = reader.LoadToCSVData<CharacterData>();
        Room = reader.LoadToCSVData<RoomData>();
        Item = reader.LoadToCSVData<ItemData>();
        _skill = reader.LoadToCSVData<SkillData>();
        CreateItemCOD();
        CreateStageInfo();
        return true;
    }

    private void CreateItemCOD()
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

    public SkillData GetSkill(string key)
    {
        if(_skill.TryGetValue(key, out SkillData skillData))
        {
            return skillData;
        }
        return null;
    }

    private void CreateStageInfo()
    {
        StageMonsterInfo stage = new StageMonsterInfo(1000);
        stage.AddMonster("Slime2", 3);
        stageMonsterInfoList.Add(stage);
        
        stage = new StageMonsterInfo(3000);  //여기를 좀 올리고
        stage.AddMonster("Slime", 3);
        stage.AddMonster("Snail", 3);
        stage.AddMonster("PlantBuger", 3);
        stage.AddMonster("BigBull", 2);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(3000);  //여기를 좀 올리고
        stage.AddMonster("BigBull", 2);
        stage.AddMonster("PlantBuger", 3);
        stage.AddMonster("Snail", 3);
        stage.AddMonster("Slime2", 2);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(3000);
        stage.AddMonster("PlantBuger", 3);
        stage.AddMonster("BigBull", 3);
        stage.AddMonster("PlantBuger2", 1);
        stage.AddMonster("BigBull2", 1);
        stageMonsterInfoList.Add(stage);
        
        stage = new StageMonsterInfo(5000);    //여기가 개어렵다   
        stage.AddMonster("PlantBuger2", 2);
        stage.AddMonster("BigBull", 5);
        stage.AddMonster("BigBull2", 1);
       stageMonsterInfoList.Add(stage);
        // 5stage----------------------------------

        stage = new StageMonsterInfo(3000);
        stage.AddMonster("PlantBuger2", 3);
        stage.AddMonster("Snail2", 2);
        stage.AddMonster("Slime2", 3);
        stage.AddMonster("Slime", 5);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(3000);
        stage.AddMonster("Snail2", 3);
        stage.AddMonster("PlantBuger2", 3);
        stage.AddMonster("BigBull2", 2);
        stage.AddMonster("BigBull", 3);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(3000);
        stage.AddMonster("Snail2", 3);
        stage.AddMonster("Slime2", 5);
        stage.AddMonster("Slime", 5);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(3000);
        stage.AddMonster("Snail2", 2);
        stage.AddMonster("Slime", 10);
        stage.AddMonster("Snail", 5);
        stage.AddMonster("BigBull2", 3);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(10000);
        stage.AddMonster("PlantBuger3", 1);
        stage.AddMonster("Snail3", 1);
        stage.AddMonster("Slime3", 1);
        stage.AddMonster("BigBull3", 1);
        stage.AddMonster("Slime2", 5);
        stageMonsterInfoList.Add(stage);
        //10stage~------------------------------3성 하나, 둘 정도

        stage = new StageMonsterInfo(5000); 
        stage.AddMonster("PlantBuger3", 1);
        stage.AddMonster("PlantBuger2", 5);
        stage.AddMonster("Slime3", 1);
        stage.AddMonster("Slime2", 5);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(5000); 
        stage.AddMonster("Snail3", 1);
        stage.AddMonster("Snail2", 5);
        stage.AddMonster("PlantBuger2", 5);
        stage.AddMonster("BigBull2", 5);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(5000);
        stage.AddMonster("Slime3", 3);
        stage.AddMonster("Snail2", 10);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(5000);
        stage.AddMonster("PlantBuger3", 2);
        stage.AddMonster("PlantBuger2", 5);
        stage.AddMonster("BigBull2", 5);
        stage.AddMonster("BigBull3", 1);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(10000);
        stage.AddMonster("Snail3", 3);
        stage.AddMonster("Slime3", 2);
        stage.AddMonster("PlantBuger3", 2);
        stage.AddMonster("BigBull3", 5);
        stageMonsterInfoList.Add(stage);
        // 15stage-----------------------------------3성이 1,2 거의 2성작(이상적인 배치가 가능해지는 시점)

        stage = new StageMonsterInfo(5000);
        stage.AddMonster("PlantBuger3", 5);
        stage.AddMonster("PlantBuger2", 3);
        stage.AddMonster("Slime3", 3);
        stage.AddMonster("Slime2", 2);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(5000);
        stage.AddMonster("Snail3", 5);
        stage.AddMonster("Slime3", 5);
        stage.AddMonster("BigBull3", 5);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(5000);
        stage.AddMonster("PlantBuger3", 5);
        stage.AddMonster("BigBull3", 5);
        stage.AddMonster("BigBull2", 5);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(5000);
        stage.AddMonster("PlantBuger3", 3);
        stage.AddMonster("PlantBuger2", 5);
        stage.AddMonster("BigBull3", 3);
        stage.AddMonster("Slime2", 5);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(20000);
        stage.AddMonster("PlantBuger3", 5);
        stage.AddMonster("Slime3", 5);
        stage.AddMonster("BigBull3", 5);
        stage.AddMonster("BigBull2", 10);
        stageMonsterInfoList.Add(stage);
        // 20stage----------------------------------- 여기 이후는 개힘들게 가자

        stage = new StageMonsterInfo(10000);
        stage.AddMonster("PlantBuger3", 10);
        stage.AddMonster("PlantBuger2", 5);
        stage.AddMonster("Slime3", 10);
        stage.AddMonster("Slime2", 5);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(10000);
        stage.AddMonster("Snail3", 10);
        stage.AddMonster("Slime3", 10);
        stage.AddMonster("BigBull3", 10);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(10000);
        stage.AddMonster("PlantBuger3", 10);
        stage.AddMonster("BigBull3", 10);
        stage.AddMonster("BigBull2", 10);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(10000);
        stage.AddMonster("PlantBuger3", 5);
        stage.AddMonster("PlantBuger2", 15);
        stage.AddMonster("BigBull3", 5);
        stage.AddMonster("Slime2", 15);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(30000);
        stage.AddMonster("PlantBuger3", 10);
        stage.AddMonster("Slime3", 10);
        stage.AddMonster("BigBull3", 10);
        stage.AddMonster("BigBull2", 20);
        stageMonsterInfoList.Add(stage);

        // 25stage----------------------------------- 여기까지 한계선 최적화 끝판왕

        stage = new StageMonsterInfo(10000);
        stage.AddMonster("Slime3", 30);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(10000);
        stage.AddMonster("Snail3", 30);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(10000);
        stage.AddMonster("PlantBuger3", 30);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(10000);
        stage.AddMonster("BigBull3", 30);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(30000);
        stage.AddMonster("PlantBuger3", 10);
        stage.AddMonster("Snail3", 10);
        stage.AddMonster("Slime3", 10);
        stage.AddMonster("BigBull3", 10);
        stageMonsterInfoList.Add(stage);
    }
}
