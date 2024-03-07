using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageFail_PopupUI : BaseUI
{
    private TextMeshProUGUI _stageText;
    private TextMeshProUGUI _rewardsText;
    private TextMeshProUGUI _upgradePointTxt;
    private Button _mainMenuBtn;
    private Button _retryBtn;

    public int _curStage { get; set; }
    public int UpgradePoint { get; set; }

    private SaveDataManager _saveDataManager;

    //public int _rewardsGold { get; set; }

    protected override void Init()
    {
        _saveDataManager = Main.Get<SaveDataManager>();
        SetUI<TextMeshProUGUI>();
        SetUI<Button>();

        UpgradePoint = Main.Get<UpgradeManager>().UpgradePoint;

        _stageText = GetUI<TextMeshProUGUI>("StageFailLevelText");
        _rewardsText = GetUI<TextMeshProUGUI>("FailRewardMoneyText");
        _upgradePointTxt = GetUI<TextMeshProUGUI>("UpgradePointTxt");
        _mainMenuBtn = GetUI<Button>("StageFailMainMenuBtn");
        _retryBtn = GetUI<Button>("StageFailRetryBtn");

        SetUICallback(_mainMenuBtn.gameObject, EUIEventState.Click, ClickMainMenuBtn);
        //SetUICallback(_retryBtn.gameObject, EUIEventState.Click, ClickRetryBtn);

        _stageText.text = $"Day {_curStage}";
        
        if (_curStage > 0)
        {
            UpgradePoint = 1;
            _upgradePointTxt.text = $"+ {UpgradePoint}";
        }
        else if(_curStage > 15)
        {
            UpgradePoint = 2;
            _upgradePointTxt.text = $"+ {UpgradePoint}";
        }
        else if(_curStage > 30)
        {
            UpgradePoint = 3;
            _upgradePointTxt.text = $"+ {UpgradePoint}";
        }
        else if(_curStage > 45)
        {
            UpgradePoint = 4;
            _upgradePointTxt.text = $"+ {UpgradePoint}";
        }
        //_rewardsText.text = ;
    }

    private void ClickMainMenuBtn(PointerEventData data)
    {
        Main.Get<UpgradeManager>().UpgradePoint += UpgradePoint;
        Main.ManagerInit();
        Main.Get<SceneManager>().ChangeScene<SelectScene>();
    }
}
