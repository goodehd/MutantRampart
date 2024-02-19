using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialMsg_PopupUI : BaseUI
{
    private TMP_Text _tutorialMsgTxt;

    public Button _closeBtn;

    public DayMain_SceneUI Owner { get; set; }

    public string curTutorialText { get; set; }
    public bool isCloseBtnActive { get; set; } = false;

    protected override void Init()
    {
        SetUI<Button>();
        SetUI<TMP_Text>();

        //Owner.tutorialMsg_PopupUI = this;

        _closeBtn = GetUI<Button>("TutorialCloseBtn");
        SetUICallback(_closeBtn.gameObject, EUIEventState.Click, ClickCloseBtn);

        _tutorialMsgTxt = GetUI<TMP_Text>("TutorialMsgTxt");

        _tutorialMsgTxt.text = curTutorialText;

        if (isCloseBtnActive)
        {
            _closeBtn.gameObject.SetActive(true);
        }

    }

    private void ClickCloseBtn(PointerEventData data)
    {
        Main.Get<UIManager>().ClosePopup();
    }
}
