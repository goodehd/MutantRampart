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
    private Image _listItemImg;
    private TMP_Text _listItemPrice;
    private Button _buyButton;
    //public bool isUnitItem { get; private set; } = true;

    // Data
    public CharacterData ShopUnitData { get; set; }

    protected override void Init()
    {
        SetUI<Image>();
        SetUI<TMP_Text>();
        SetUI<Button>();

        _listBG = GetUI<Image>("Unit_List");
        _listItemName = GetUI<TMP_Text>("Unit_ItemName");
        _listItemImg = GetUI<Image>("Unit_ItemImg");
        _listItemPrice = GetUI<TMP_Text>("Unit_ItemPrice");
        _buyButton = GetUI<Button>("Unit_BuyBtn");

        SetInfo();

        SetUICallback(_buyButton.gameObject, EUIEventState.Click, ClickBuyBtn);

    }

    private void SetInfo()
    {
        _listItemName.text = ShopUnitData.Key;
        _listItemImg.sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.UNIT_SPRITE_PATH}{ShopUnitData.Key}");
        _listItemPrice.text = ShopUnitData.Price.ToString();
        //Main.Get<UIManager>().OpenPopup<BuyConfirmUI>("BuyConfirm_PopupUI").isUnitItem = true;

    }

    private void ClickBuyBtn(PointerEventData EventData)
    {

        Main.Get<UIManager>().OpenPopup<BuyConfirm_PopupUI>("BuyConfirm_PopupUI").ShopUnitData = ShopUnitData;
        //Main.Get<UIManager>().OpenPopup<BuyConfirmUI>("BuyConfirm_PopupUI").isUnitItem = true;
        //Main.Get<UIManager>().OpenPopup<BuyConfirmUI>("BuyConfirm_PopupUI").isRoomItem = false;

        //BuyConfirmUI buyConfirmUI = Main.Get<UIManager>().OpenPopup<BuyConfirmUI>("BuyConfirm_PopupUI");
        //buyConfirmUI.itemName = this.gameObject.name; // 이러면 Unit_List 의 이름을 들고와버리네..
    }
}

// 1. BaseUI 상속받기
// 2. 필요한 컴포넌트들 변수 선언
// 3. 