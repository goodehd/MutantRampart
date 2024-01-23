using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Unit_List : BaseUI
{
    private Image _listBG;
    private TMP_Text _listItemName;
    private Sprite _listItemImg;
    private TMP_Text _listItemPrice;
    private Button _buyButton;

    // Data ?

    protected override void Init()
    {
        SetUI<Image>();
        SetUI<TMP_Text>();
        SetUI<Button>();

        _listBG = GetUI<Image>("Unit_List(Clone)");
        _listItemName = GetUI<TMP_Text>("Unit_ItemName(Clone)");
        //_listItemImg = 
        _listItemPrice = GetUI<TMP_Text>("Unit_ItemPrice(Clone)");
        _buyButton = GetUI<Button>("Unit_BuyBtn(Clone)");

        SetUICallback(_buyButton.gameObject, EUIEventState.Click, ClickBuyBtn);

    }

    private void SetInfo()
    {
        //_listItemName.text = 
    }

    private void ClickBuyBtn(PointerEventData EventData)
    {
        Main.Get<UIManager>().OpenPopup<BuyConfirmUI>("BuyConfirm_PopupUI");
    }
}

// 1. BaseUI 상속받기
// 2. 필요한 컴포넌트들 변수 선언
// 3. 