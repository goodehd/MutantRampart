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
    public int StageEnemyCount { get; set; }

    private TileManager _tileManager;
    private List<StageInfo> _stages = new List<StageInfo>();

    private bool _isStageStart = false;
    private int _curStage = 0;
    public event Action OnStageClear; 
    
    public bool Init()
    {
        _tileManager = Main.Get<TileManager>();
        _stages.Add(new StageInfo { SpwanEnemyName = "Slime", SpwanCount = 5 });
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
    }

    public void StageClear()
    {
        StageClear_PopupUI ui = Main.Get<UIManager>().OpenPopup<StageClear_PopupUI>("StageClear_PopupUI");
        ui._curStage = _curStage + 1;
        ui._rewardsGold = _stages[_curStage].SpwanCount * 1000;

        Main.Get<GameManager>().ChangeMoney(_stages[_curStage].SpwanCount * 1000);
        _curStage++;
        _isStageStart = false;
        OnStageClear?.Invoke();
    }

    public void CheckClear()
    {
        StageEnemyCount--;
        if(StageEnemyCount <= 0)
        {
            StageClear();
        }
    }
}
