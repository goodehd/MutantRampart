using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Room_ListUI : BaseUI
{
    private Image _listBG;
    private TMP_Text _listItemName;
    private Image _listItemImg;
    private TMP_Text _listItemPrice;
    private Button _infoButton;
    private Button _buyButton;

    // Data
    public RoomData ShopRoomData { get; set; }

    public string itemName { get; set; }
    public string itemDescript { get; set; }

    protected override void Init()
    {
        SetUI<Image>();
        SetUI<TMP_Text>();
        SetUI<Button>();

        _listBG = GetUI<Image>("Room_ListUI");
        _listItemName = GetUI<TMP_Text>("Room_ItemName");
        _listItemImg = GetUI<Image>("Room_ItemImg");
        _listItemPrice = GetUI<TMP_Text>("Room_ItemPrice");
        _infoButton = GetUI<Button>("Room_InfoBtn");
        _buyButton = GetUI<Button>("Room_BuyBtn");

        SetInfo();

        SetUICallback(_infoButton.gameObject, EUIEventState.Click, ClickInfoBtn);
        SetUICallback(_buyButton.gameObject, EUIEventState.Click, ClickBuyBtn);

    }

    private void SetInfo()
    {
        _listItemName.text = ShopRoomData.Key;
        _listItemImg.sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.ROOM_SPRITES_PATH}{ShopRoomData.Key}");
        _listItemPrice.text = ShopRoomData.Price.ToString();

    }

    private void ClickInfoBtn(PointerEventData EventData)
    {
        ItemDescript_PopupUI ui = Main.Get<UIManager>().OpenPopup<ItemDescript_PopupUI>("ItemDescript_PopupUI");
        ui.ShopRoomData = ShopRoomData;
    }

    private void ClickBuyBtn(PointerEventData EventData)
    {
        YesNo_PopupUI ui = Main.Get<UIManager>().OpenPopup<YesNo_PopupUI>("YesNo_PopupUI");
        ui.curAskingText = "구매하시겠습니까 ?";
        ui.ShopRoomData = ShopRoomData;
    }
}

// 1. BaseUI 상속받기
// 2. 필요한 컴포넌트들 변수 선언
// 3. 