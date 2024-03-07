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
    public Dictionary<string, TutorialData> Tutorial = new Dictionary<string, TutorialData>();
    public List<StageData> stageData = new List<StageData>();
    public List<StageListData> stageListData = new List<StageListData>();
    public CSVReader reader = new();

    public bool Init()
    {
        Character = reader.LoadToCSVData<CharacterData>();
        Room = reader.LoadToCSVData<RoomData>();
        Item = reader.LoadToCSVData<ItemData>();
        _skill = reader.LoadToCSVData<SkillData>();
        Tutorial = reader.LoadToCSVData<TutorialData>();
        stageData = reader.LoadToCSVDataToList<StageData>();
        stageListData = reader.LoadToCSVDataToList<StageListData>();
        CreateItemCDO();
        CreateStageInfo();
        return true;
    }

    private void CreateItemCDO()
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

    public void CreateStageInfo()
    {
        StageMonsterInfo stagetest;
        int initCount = 0;
        for(int i =0; stageListData.Count > i; i++)
        {
            stagetest = new StageMonsterInfo(stageListData[i].RewardsGold);
            for(int j =0; j < stageListData[i].MonsterListCount; j++)
            {
                stagetest.AddMonster(stageData[initCount].Monster, stageData[initCount].Value);
                initCount++;
            }
            stageMonsterInfoList.Add(stagetest);
        }    
    }
}
