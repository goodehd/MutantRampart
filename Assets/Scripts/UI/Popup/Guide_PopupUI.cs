using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Guide_PopupUI : BaseUI
{
    private Button _closeButton;

    protected override void Init()
    {
        SetUI<Button>();
        SetUI<Image>();

        _closeButton = GetUI<Button>("GuideCloseBtn");
        SetUICallback(_closeButton.gameObject, EUIEventState.Click, ClickCloseBtn);
    }

    private void ClickCloseBtn(PointerEventData data)
    {
        Main.Get<UIManager>().ClosePopup();
    }
}
