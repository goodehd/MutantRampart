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

        _stageText = GetUI<TextMeshProUGUI>("StageFailLevelText");
        _rewardsText = GetUI<TextMeshProUGUI>("FailRewardMoneyText");
        _upgradePointTxt = GetUI<TextMeshProUGUI>("UpgradePointTxt");
        _mainMenuBtn = GetUI<Button>("StageFailMainMenuBtn");
        _retryBtn = GetUI<Button>("StageFailRetryBtn");

        SetUICallback(_mainMenuBtn.gameObject, EUIEventState.Click, ClickMainMenuBtn);
        //SetUICallback(_retryBtn.gameObject, EUIEventState.Click, ClickRetryBtn);

        _stageText.text = $"Day {_curStage}";
        UpgradePoint = 1;
        _upgradePointTxt.text = $"+ {UpgradePoint}";
        /*if (_curStage > 5)
        {
            UpgradePoint += 1;
            _upgradePointTxt.text = $"+ {UpgradePoint}";
        }*/
        //_rewardsText.text = ;
    }

    private void ClickMainMenuBtn(PointerEventData data)
    {
        Main.Get<UpgradeManager>().UpgradePoint += UpgradePoint;
        Main.ManagerInit();
        Main.Get<SceneManager>().ChangeScene<SelectScene>();
    }
}
