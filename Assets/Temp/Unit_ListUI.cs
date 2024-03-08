using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Unit_ListUI : BaseUI
{
    private Image _listBG;
    private TMP_Text _listItemName;
    private Image _listItemImg;
    private TMP_Text _listItemPrice;
    private Button _infoButton;
    private Button _buyButton;

    // Data
    public CharacterData ShopUnitData { get; set; }
    public string itemName { get; set; }
    public string itemDescript { get; set; }

    protected override void Init()
    {
        SetUI<Image>();
        SetUI<TMP_Text>();
        SetUI<Button>();

        _listBG = GetUI<Image>("Unit_ListUI");
        _listItemName = GetUI<TMP_Text>("Unit_ItemName");
        _listItemImg = GetUI<Image>("Unit_ItemImg");
        _listItemPrice = GetUI<TMP_Text>("Unit_ItemPrice");
        _infoButton = GetUI<Button>("Unit_InfoBtn");
        _buyButton = GetUI<Button>("Unit_BuyBtn");

        SetInfo();

        SetUICallback(_infoButton.gameObject, EUIEventState.Click, ClickInfoBtn);
        SetUICallback(_buyButton.gameObject, EUIEventState.Click, ClickBuyBtn);
    }

    private void SetInfo()
    {
        _listItemName.text = ShopUnitData.Key;
        _listItemImg.sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.UNIT_SPRITE_PATH}{ShopUnitData.Key}");
        _listItemPrice.text = ShopUnitData.Price.ToString();
    }

    private void ClickInfoBtn(PointerEventData EventData)
    {
        ItemDescript_PopupUI ui = Main.Get<UIManager>().OpenPopup<ItemDescript_PopupUI>("ItemDescript_PopupUI");
        ui.ShopUnitData = ShopUnitData;
    }

    private void ClickBuyBtn(PointerEventData EventData)
    {
        YesNo_PopupUI ui = Main.Get<UIManager>().OpenPopup<YesNo_PopupUI>("YesNo_PopupUI");
        ui.curAskingText = "구매하시겠습니까 ?";
        ui.ShopUnitData = ShopUnitData;

    }
}

// 1. BaseUI 상속받기
// 2. 필요한 컴포넌트들 변수 선언
// 3. 