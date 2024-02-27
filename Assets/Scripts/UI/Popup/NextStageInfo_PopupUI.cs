using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NextStageInfo_PopupUI : BaseUI
{
    private DataManager _dataManager;
    private int _curStage;
    private int _maxStage;
    private ScrollRect _nextInfoScrollView;
    private Transform _nextInfoContent;
    protected override void Init()
    {
        base.Init();
        _dataManager = Main.Get<DataManager>();
        SetUI<Transform>();
        SetUI<ScrollRect>();
        _nextInfoScrollView = GetUI<ScrollRect>("NextStageEnemy_Scroll View");
        _nextInfoContent = GetUI<Transform>("NextStageEnemy_Content");

        _curStage = _gameManager.CurStage;
        _maxStage = _dataManager.stageMonsterInfoList.Count;
        SetNextInfo();
    }
    public void SetNextInfo()
    {
        StageMonsterInfo nextStageInfo;
        if (_curStage < _maxStage)
        {
            nextStageInfo = _dataManager.stageMonsterInfoList[_curStage + 1];

            foreach (Transform item in _nextInfoContent.transform)
            {
                Destroy(item.gameObject);
            }
            for (int i = 0; i < nextStageInfo.Monsters.Count; i++)
            {
                NextEnemy_ContentsUI inventUnitItems = Main.Get<UIManager>().CreateSubitem<NextEnemy_ContentsUI>("InventUnit_ContentsBtnUI", _nextInfoContent);
                inventUnitItems.Monsterinfo = nextStageInfo.Monsters[i];
            }
        }
    }
}
