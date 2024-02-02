using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartScene_SceneUI : BaseUI
{
    private Button startNewGameBtn;
    private Button continueGameBtn;
    private Button settingBtn;
    private Button endGameBtn;

    protected override void Init()
    {
        SetUI<Button>();

        startNewGameBtn = GetUI<Button>("StartNewBtn");
        continueGameBtn = GetUI<Button>("ContinueBtn");
        settingBtn = GetUI<Button>("SettingBtn");
        endGameBtn = GetUI<Button>("EndGameBtn");

        SetUICallback(startNewGameBtn.gameObject, EUIEventState.Click, ClickStartNewBtn);
        SetUICallback(continueGameBtn.gameObject, EUIEventState.Click, ClickContinueGameBtn);
        SetUICallback(settingBtn.gameObject, EUIEventState.Click, ClickSettingNewBtn);
        SetUICallback(endGameBtn.gameObject, EUIEventState.Click, ClickEndGameBtn);
    }

    private void ClickStartNewBtn(PointerEventData data)
    {
        Time.timeScale = 1.0f;
        Main.Get<SceneManager>().ChangeScene<HongTestScene>();

        // 기존에 데이터가 있는데 새로하기 버튼 누르면 확인 창 띄우기
    }

    private void ClickContinueGameBtn(PointerEventData data)
    {
        Time.timeScale = 1.0f;
        Main.Get<SceneManager>().ChangeScene<HongTestScene>();
        // 저장된 데이터 없이 이어하기 누른 경우 나중에 생각해주기
    }

    private void ClickSettingNewBtn(PointerEventData data)
    {
        Main.Get<UIManager>().OpenPopup<Setting_PopupUI>();
    }

    private void ClickEndGameBtn(PointerEventData data)
    {
        Application.Quit();
    }
}
