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
        stage.AddMonster("BigBull", 1);
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
        _curStage = Main.Get<GameManager>().CurStage;
        if (_curStage >= stageMonsterInfoList.Count)
            return;

        if (_isStageStart)
            return;

        StageEnemyCount = stageMonsterInfoList[_curStage].Count;
        _tileManager.SpawnTile.StartStage(stageMonsterInfoList[_curStage]);
        _isStageStart = true;

        OnStageStartEvent?.Invoke(_curStage);
        Main.Get<SoundManager>().SoundPlay($"NightBGM", ESoundType.BGM);
    }

    public void StageClear()
    {
        StageClear_PopupUI ui = Main.Get<UIManager>().OpenPopup<StageClear_PopupUI>("StageClear_PopupUI");

        if (Main.Get<GameManager>().isTutorial) // 튜토리얼 중이라면
        {
            TutorialMsg_PopupUI tutorialUI = Main.Get<UIManager>().OpenPopup<TutorialMsg_PopupUI>(); // 마지막 튜토리얼 팝업
            tutorialUI.curTutorialText = "아주 좋아요!\n이런 방식으로 침입하는 적으로부터 Home 을 지켜내세요!\n무운을 빌어요!";
            tutorialUI.isBackgroundActive = true;
            tutorialUI.isCloseBtnActive = true;
            Main.Get<GameManager>().isTutorial = false; // 튜토리얼 이제 끝 !
            PlayerPrefs.SetInt("Tutorial", 1);
            PlayerPrefs.Save();
        }

        ui._curStage = _curStage + 1;
        Main.Get<GameManager>().CurStage = ui._curStage;
        ui._rewardsGold = stageMonsterInfoList[_curStage].RewardsGold;

        Main.Get<GameManager>().ChangeMoney(stageMonsterInfoList[_curStage].RewardsGold);
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
}
