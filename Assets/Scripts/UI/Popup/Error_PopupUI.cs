using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Error_PopupUI : BaseUI
{

    private TMP_Text _errorText;
    private Button _closeButton;

    public string curErrorText { get; set; }

    protected override void Init()
    {
        SetUI<TMP_Text>();
        SetUI<Button>();

        _errorText = GetUI<TMP_Text>("ErrorText");
        _closeButton = GetUI<Button>("ErrorCloseBtn");

        _errorText.text = curErrorText;

        SetUICallback(_closeButton.gameObject, EUIEventState.Click, ClickCloseBtn);

    }

    private void ClickCloseBtn(PointerEventData EventData)
    {
        Main.Get<UIManager>().ClosePopup();
    }
    
}
