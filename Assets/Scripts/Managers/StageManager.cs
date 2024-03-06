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
        RewardsGold = rewardsGold + (int)(rewardsGold * Main.Get<UpgradeManager>().UpgradeGoldPercent);
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
    private DataManager _dataManager;

    private bool _isStageStart = false;
    private int _curStage = 0;
    
    private int _addPlusClearGold = 0;

    public bool Init()
    {
        _tileManager = Main.Get<TileManager>();
        _dataManager = Main.Get<DataManager>();

        _isStageStart = false;
        OnStageStartEvent = null;
        OnStageClearEvent = null;
        return true;
    }

    public void StartStage()
    {
        _curStage = Main.Get<GameManager>().CurStage;
        if (_curStage >= _dataManager.stageMonsterInfoList.Count)
            return;

        if (_isStageStart)
            return;

        StageEnemyCount = _dataManager.stageMonsterInfoList[_curStage].Count;
        _tileManager.SpawnTile.StartStage(_dataManager.stageMonsterInfoList[_curStage]);
        _isStageStart = true;

        Main.Get<SoundManager>().SoundPlay($"NightBGM", ESoundType.BGM);
        OnStageStartEvent?.Invoke(_curStage);
    }

    public void StageClear()
    {
        StageClear_PopupUI ui = Main.Get<UIManager>().OpenPopup<StageClear_PopupUI>("StageClear_PopupUI");

        if (Main.Get<TutorialManager>().isTutorial) // 튜토리얼 중이라면
        {
            Main.Get<TutorialManager>().CreateTutorialPopup("T18", true, true); // 마지막 튜토리얼 팝업

            Main.Get<TutorialManager>().isTutorial = false; // 튜토리얼 이제 끝 !
            PlayerPrefs.SetInt("Tutorial", 1);
            PlayerPrefs.Save();
        }

        ui._curStage = _curStage + 1;
        Main.Get<GameManager>().CurStage = ui._curStage;
        ui._rewardsGold = _dataManager.stageMonsterInfoList[_curStage].RewardsGold;

        Main.Get<GameManager>().ChangeMoney(_dataManager.stageMonsterInfoList[_curStage].RewardsGold);
        _isStageStart = false;
        OnStageClearEvent?.Invoke(++_curStage);
        Main.Get<SoundManager>().SoundPlay($"DayBGM", ESoundType.BGM);
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

    public bool GetIsStageStart()
    {
        return _isStageStart;
    }
}
