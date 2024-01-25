using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Room_List : BaseUI
{
    private Image _listBG;
    private TMP_Text _listItemName;
    private Image _listItemImg;
    private TMP_Text _listItemPrice;
    private Button _buyButton;

    // Data
    public RoomData ShopRoomData { get; set; }

    protected override void Init()
    {
        SetUI<Image>();
        SetUI<TMP_Text>();
        SetUI<Button>();

        _listBG = GetUI<Image>("Room_List");
        _listItemName = GetUI<TMP_Text>("Room_ItemName");
        _listItemImg = GetUI<Image>("Room_ItemImg");
        _listItemPrice = GetUI<TMP_Text>("Room_ItemPrice");
        _buyButton = GetUI<Button>("Room_BuyBtn");

        SetInfo();

        SetUICallback(_buyButton.gameObject, EUIEventState.Click, ClickBuyBtn);

    }

    private void SetInfo()
    {
        _listItemName.text = ShopRoomData.Key;
        _listItemImg.sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.ROOM_SPRITES_PATH}{ShopRoomData.Key}");
        _listItemPrice.text = ShopRoomData.Price.ToString();

    }

    private void ClickBuyBtn(PointerEventData EventData)
    {
        Main.Get<UIManager>().OpenPopup<BuyConfirm_PopupUI>("BuyConfirm_PopupUI").ShopRoomData = ShopRoomData;
    }
}

// 1. BaseUI 상속받기
// 2. 필요한 컴포넌트들 변수 선언
// 3. 