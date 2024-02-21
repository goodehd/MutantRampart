using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo
{
    public string SpwanEnemyName;
    public int SpwanCount;
}

public class StageManager : IManagers
{
    public event Action<int> OnStageStartEvent;
    public event Action<int> OnStageClearEvent;
    public int StageEnemyCount { get; set; }

    private TileManager _tileManager;
    private List<StageInfo> _stages = new List<StageInfo>();

    private bool _isStageStart = false;
    private int _curStage = 0;
    
    private int _addPlusClearGold = 0;

    public bool Init()
    {
        _tileManager = Main.Get<TileManager>();
        //_stages.Add(new StageInfo { SpwanEnemyName = "BigBull", SpwanCount = 5 });
        _stages.Add(new StageInfo { SpwanEnemyName = "BigBull", SpwanCount = 5 });
        _stages.Add(new StageInfo { SpwanEnemyName = "Snail", SpwanCount = 5 });
        _stages.Add(new StageInfo { SpwanEnemyName = "PlantBuger", SpwanCount = 5 });
        _stages.Add(new StageInfo { SpwanEnemyName = "Slime", SpwanCount = 10 });
        _stages.Add(new StageInfo { SpwanEnemyName = "Slime", SpwanCount = 15 });
        return true;
    }

    public void StartStage()
    {
        if (_curStage >= _stages.Count)
            return;

        if (_isStageStart)
            return;

        StageEnemyCount = _stages[_curStage].SpwanCount;
        _tileManager.SpawnTile.StartStage(_stages[_curStage].SpwanEnemyName, _stages[_curStage].SpwanCount);
        _isStageStart = true;

        OnStageStartEvent?.Invoke(_curStage);
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
        }

        ui._curStage = _curStage + 1;
        Main.Get<GameManager>().CurStage = ui._curStage;
        ui._rewardsGold = _stages[_curStage].SpwanCount * 1000 + _addPlusClearGold;

        Main.Get<GameManager>().ChangeMoney(_stages[_curStage].SpwanCount * 1000 + _addPlusClearGold);
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
