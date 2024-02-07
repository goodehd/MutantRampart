using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item_ListUI : BaseUI
{
    private Image _listBG;
    private TMP_Text _listItemName;
    private Image _listItemImg;
    private TMP_Text _listItemPrice;
    private Button _infoButton;
    private Button _buyButton;

    // Data
    public ItemData ShopItemData { get; set; }
    public string itemName { get; set; }
    public string itemDescript { get; set; }

    protected override void Init()
    {
        SetUI<Image>();
        SetUI<TMP_Text>();
        SetUI<Button>();

        _listBG = GetUI<Image>("Item_ListUI");
        _listItemName = GetUI<TMP_Text>("Item_ItemName");
        _listItemImg = GetUI<Image>("Item_ItemImg");
        _listItemPrice = GetUI<TMP_Text>("Item_ItemPrice");
        _infoButton = GetUI<Button>("Item_InfoBtn");
        _buyButton = GetUI<Button>("Item_BuyBtn");

        SetInfo();

        SetUICallback(_infoButton.gameObject, EUIEventState.Click, ClickInfoBtn);
        SetUICallback(_buyButton.gameObject, EUIEventState.Click, ClickBuyBtn);
    }

    private void SetInfo()
    {
        _listItemName.text = ShopItemData.Key;
        _listItemImg.sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.ITEM_SPRITE_PATH}{ShopItemData.Key}");
        _listItemPrice.text = ShopItemData.Price.ToString();
    }

    private void ClickInfoBtn(PointerEventData EventData)
    {
        ItemDescript_PopupUI ui = Main.Get<UIManager>().OpenPopup<ItemDescript_PopupUI>("ItemDescript_PopupUI");
        ui.ShopItemData = ShopItemData;
    }

    private void ClickBuyBtn(PointerEventData EventData)
    {
        YesNo_PopupUI ui = Main.Get<UIManager>().OpenPopup<YesNo_PopupUI>("YesNo_PopupUI");
        ui.curAskingText = "구매하시겠습니까 ?";
        ui.ShopItemData = ShopItemData;
    }
}
