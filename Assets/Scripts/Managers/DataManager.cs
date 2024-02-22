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

        stage = new StageMonsterInfo(1000);
        stage.AddMonster("Slime", 10);
        stage.AddMonster("Slime2", 1);
        stage.AddMonster("Snail", 3);
        stage.AddMonster("PlantBuger", 5);
        stage.AddMonster("BigBull", 3);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(1000);
        stage.AddMonster("BigBull", 10);
        stage.AddMonster("BigBull2", 1);
        stage.AddMonster("PlantBuger", 3);
        stage.AddMonster("Snail", 3);
        stage.AddMonster("Slime", 5);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(1000);
        stage.AddMonster("PlantBuger", 5);
        stage.AddMonster("BigBull", 10);
        stage.AddMonster("PlantBuger2", 1);
        stage.AddMonster("BigBull2", 1);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(3000);
        stage.AddMonster("PlantBuger2", 2);
        stage.AddMonster("BigBull", 10);
        stage.AddMonster("Snail", 10);
        stage.AddMonster("Slime", 10);
        stage.AddMonster("BigBull", 10);
        stageMonsterInfoList.Add(stage);
        // 5stage----------------------------------

        stage = new StageMonsterInfo(1000);
        stage.AddMonster("Slime", 10);
        stage.AddMonster("Slime2", 1);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(1000);
        stage.AddMonster("Slime", 10);
        stage.AddMonster("Slime2", 1);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(1000);
        stage.AddMonster("Slime", 10);
        stage.AddMonster("Slime2", 1);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(1000);
        stage.AddMonster("Slime", 10);
        stage.AddMonster("Slime2", 1);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(5000);
        stage.AddMonster("Slime", 10);
        stage.AddMonster("Slime2", 1);
        stageMonsterInfoList.Add(stage);
        //10stage~------------------------------



    }
}
