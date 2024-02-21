using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster
{
    public string Name { get; set; }
    public int Count { get; set; }

    public Monster(string name, int count)
    {
        Name = name;
        Count = count;
    }
}

public class StageMonsterInfo
{
    public List<Monster> Monsters { get; set; }
    public int Count { get; set; }
    public int RewardsGold { get; set; }

    public StageMonsterInfo(int rewardsGold)
    {
        Count = 0;
        Monsters = new List<Monster>();
        RewardsGold = rewardsGold;
    }

    public void AddMonster(string name, int count)
    {
        Monsters.Add(new Monster(name, count));
        Count += count;
    }
}

public class StageManager : IManagers
{
    public event Action<int> OnStageStartEvent;
    public event Action<int> OnStageClearEvent;
    public int StageEnemyCount { get; set; }

    private TileManager _tileManager;
    private List<StageMonsterInfo> stageMonsterInfoList = new List<StageMonsterInfo>();

    private bool _isStageStart = false;
    private int _curStage = 0;
    
    private int _addPlusClearGold = 0;

    public bool Init()
    {
        _tileManager = Main.Get<TileManager>();

        StageMonsterInfo stage = new StageMonsterInfo(1000);
        stage.AddMonster("BigBull", 2);
        stage.AddMonster("Slime", 3);
        stage.AddMonster("Snail", 1);
        stage.AddMonster("PlantBuger", 1);
        stageMonsterInfoList.Add(stage);

        stage = new StageMonsterInfo(2000);
        stage.AddMonster("Slime", 3);
        stage.AddMonster("Snail", 5);
        stage.AddMonster("BigBull", 1);
        stage.AddMonster("PlantBuger", 2);
        stageMonsterInfoList.Add(stage);
        stage = new StageMonsterInfo(2000);
        stage.AddMonster("Slime", 3);
        stage.AddMonster("Snail", 5);
        stage.AddMonster("BigBull", 1);
        stage.AddMonster("PlantBuger", 2);
        stageMonsterInfoList.Add(stage);
        stage = new StageMonsterInfo(2000);
        stage.AddMonster("Slime", 3);
        stage.AddMonster("Snail", 5);
        stage.AddMonster("BigBull", 1);
        stage.AddMonster("PlantBuger", 2);
        stageMonsterInfoList.Add(stage);
        stage = new StageMonsterInfo(2000);
        stage.AddMonster("Slime", 3);
        stage.AddMonster("Snail", 5);
        stage.AddMonster("BigBull", 1);
        stage.AddMonster("PlantBuger", 2);
        stageMonsterInfoList.Add(stage);
        stage = new StageMonsterInfo(2000);
        stage.AddMonster("Slime", 3);
        stage.AddMonster("Snail", 5);
        stage.AddMonster("BigBull", 1);
        stage.AddMonster("PlantBuger", 2);
        stageMonsterInfoList.Add(stage);
        stage = new StageMonsterInfo(2000);
        stage.AddMonster("Slime", 3);
        stage.AddMonster("Snail", 5);
        stage.AddMonster("BigBull", 1);
        stage.AddMonster("PlantBuger", 2);
        stageMonsterInfoList.Add(stage);
        stage = new StageMonsterInfo(2000);
        stage.AddMonster("Slime", 3);
        stage.AddMonster("Snail", 5);
        stage.AddMonster("BigBull", 1);
        stage.AddMonster("PlantBuger", 2);
        stageMonsterInfoList.Add(stage);
        stage = new StageMonsterInfo(2000);
        stage.AddMonster("Slime", 3);
        stage.AddMonster("Snail", 5);
        stage.AddMonster("BigBull", 1);
        stage.AddMonster("PlantBuger", 2);
        stageMonsterInfoList.Add(stage);

        return true;
    }

    public void StartStage()
    {
        if (_curStage >= stageMonsterInfoList.Count)
            return;

        if (_isStageStart)
            return;

        StageEnemyCount = stageMonsterInfoList[_curStage].Count;
        _tileManager.SpawnTile.StartStage(stageMonsterInfoList[_curStage]);
        _isStageStart = true;

        OnStageStartEvent?.Invoke(_curStage);
    }

    public void StageClear()
    {
        StageClear_PopupUI ui = Main.Get<UIManager>().OpenPopup<StageClear_PopupUI>("StageClear_PopupUI");
        ui._curStage = _curStage + 1;
        Main.Get<GameManager>().CurStage = ui._curStage;
        ui._rewardsGold = stageMonsterInfoList[_curStage].RewardsGold;

        Main.Get<GameManager>().ChangeMoney(stageMonsterInfoList[_curStage].RewardsGold);
        _isStageStart = false;
        OnStageClearEvent?.Invoke(++_curStage);
        Time.timeScale = 1.0f;
    }

    public void CheckClear()
    {
        StageEnemyCount--;
        if (StageEnemyCount <= 0 && Main.Get<GameManager>().PlayerHP.CurValue > 0)
        {
            StageClear();
        }
    }

    public void AddClearGold(int value)
    {
        _addPlusClearGold += value;
    }
}
