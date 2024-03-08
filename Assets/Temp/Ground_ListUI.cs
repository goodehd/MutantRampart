using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Ground_ListUI : BaseUI
{
    private TMP_Text _listGroundName;
    private Image _listGroundImg;
    private TMP_Text _listGroundPrice;
    private Button _infoButton;
    private Button _buyButton;

    public ItemData ShopGroundItemData { get; set; }

    protected override void Init()
    {
        SetUI<Image>();
        SetUI<TMP_Text>();
        SetUI<Button>();

        _listGroundName = GetUI<TMP_Text>("Ground_ItemName");
        _listGroundImg = GetUI<Image>("Ground_ItemImg");
        _listGroundPrice = GetUI<TMP_Text>("Ground_ItemPrice");
        _infoButton = GetUI<Button>("Ground_InfoBtn");
        _buyButton = GetUI<Button>("Ground_BuyBtn");

        SetInfo();

        SetUICallback(_buyButton.gameObject, EUIEventState.Click, ClickBuyBtn);

    }

    private void SetInfo()
    {
        _listGroundName.text = ShopGroundItemData.Key;
        _listGroundPrice.text = ShopGroundItemData.Price.ToString();
    }

    private void ClickBuyBtn(PointerEventData EventData)
    {
        YesNo_PopupUI ui = Main.Get<UIManager>().OpenPopup<YesNo_PopupUI>("YesNo_PopupUI");
        ui.curAskingText = "구매하시겠습니까 ?";
        ui.ShopGroundItemData = ShopGroundItemData;
    }
}
