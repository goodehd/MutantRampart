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
    private Button _mainMenuBtn;
    private Button _retryBtn;

    public int _curStage { get; set; }

    //public int _rewardsGold { get; set; }

    protected override void Init()
    {
        SetUI<TextMeshProUGUI>();
        SetUI<Button>();

        _stageText = GetUI<TextMeshProUGUI>("StageFailLevelText");
        _rewardsText = GetUI<TextMeshProUGUI>("FailRewardMoneyText");
        _mainMenuBtn = GetUI<Button>("StageFailMainMenuBtn");
        _retryBtn = GetUI<Button>("StageFailRetryBtn");

        SetUICallback(_mainMenuBtn.gameObject, EUIEventState.Click, ClickMainMenuBtn);
        //SetUICallback(_retryBtn.gameObject, EUIEventState.Click, ClickRetryBtn);

        _stageText.text = $"Stage {_curStage}";
        //_rewardsText.text = ;
    }

    private void ClickMainMenuBtn(PointerEventData data)
    {
        // 시작 Scene 으로 이동
        Main.Get<SaveDataManager>().DeleteData();
        Main.Get<GameManager>().Init();
        Main.Get<UIManager>().Init();
        Main.Get<StageManager>().Init();
        Main.Get<SoundManager>().SoundStop(ESoundType.BGM);
        Time.timeScale = 1.0f;
        Main.Get<SceneManager>().ChangeScene<SelectScene>();
    }

    /*private void ClickRetryBtn(PointerEventData data)
    {
        //  스테이지 re 도전 ? 이 되려나 ?
        // 일단은 시작Scene 으로 이동
        Main.Get<UIManager>().ClosePopup();
        Main.Get<SceneManager>().ChangeScene<StartScene>();
    }*/
}
