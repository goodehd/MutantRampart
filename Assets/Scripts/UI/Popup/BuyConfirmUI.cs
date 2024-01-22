using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class BuyConfirmUI : BaseUI
{
    private Button _yesButton;
    private Button _noButton;


    protected override void Init()
    {
        SetUI<Button>();

        _yesButton = GetUI<Button>("YesBtn");
        _noButton = GetUI<Button>("NoBtn");

        SetUICallback(_yesButton.gameObject, EUIEventState.Click, ClickYesBtn);
        SetUICallback(_noButton.gameObject, EUIEventState.Click, ClickNoBtn);

    }

    private void ClickYesBtn(PointerEventData eventData)
    {
        // todo : 구매 관련 로직 작성
        // e.g. 돈이 아이템 금액보다 많으면 -> 돈 차감, 아이템을 인벤토리에 추가
        Main.Get<UIManager>().ClosePopup();

        // 돈이 아이템 금액보다 적으면 돈부족 경고창 띄우기
        Main.Get<UIManager>().OpenPopup<MoneyErrorUI>("MoneyError_PopupUI");

    }

    private void ClickNoBtn(PointerEventData eventData)
    {
        Main.Get<UIManager>().ClosePopup();
    }

}
