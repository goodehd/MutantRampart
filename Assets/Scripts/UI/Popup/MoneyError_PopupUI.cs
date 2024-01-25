using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoneyError_PopupUI : BaseUI
{
    private Button _closeButton;

    protected override void Init()
    {
        SetUI<Button>();

        _closeButton = GetUI<Button>("MoneyErrorCloseBtn");

        SetUICallback(_closeButton.gameObject, EUIEventState.Click, ClickCloseBtn);

    }

    private void ClickCloseBtn(PointerEventData eventData)
    {
        Main.Get<UIManager>().ClosePopup();
    }
}
