using System.Collections;
using System.Collections.Generic;
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
    //private Transform _content;
    private Button _buyButton;

    // Data
    public ShopItemData ShopRoomData { get; set; }

    protected override void Init()
    {
        SetUI<Image>();
        SetUI<TMP_Text>();
        //SetUI<Transform>();
        SetUI<Button>();

        _listBG = GetUI<Image>("Room_List(Clone)");
        _listItemName = GetUI<TMP_Text>("Room_ItemName");
        _listItemImg = GetUI<Image>("Room_ItemImg");
        _listItemPrice = GetUI<TMP_Text>("Room_ItemPrice");
        //_content = GetUI<Transform>("Room_Content");
        _buyButton = GetUI<Button>("Room_BuyBtn");

        SetInfo();

        //List<Shop_RoomData> shopRoomItems = Main.Get<GameManager>().ShopRoomItems;
        //for (int i = 0; i < shopRoomItems.Count; i++)
        //{
        //    Room_List roomItemsList = Main.Get<UIManager>().CreateSubitem<Room_List>("Room_List", _content);
        //    roomItemsList.ShopRoomData = shopRoomItems[i];
        //}

        SetUICallback(_buyButton.gameObject, EUIEventState.Click, ClickBuyBtn);

    }

    private void SetInfo()
    {
        _listItemName.text = ShopRoomData.Key;
        _listItemImg.sprite = Main.Get<ResourceManager>().Load<Sprite>(ShopRoomData.SpritePath);
        _listItemPrice.text = ShopRoomData.Price.ToString();
    }

    private void ClickBuyBtn(PointerEventData EventData)
    {
        Main.Get<UIManager>().OpenPopup<BuyConfirmUI>("BuyConfirm_PopupUI");
    }
}

// 1. BaseUI 상속받기
// 2. 필요한 컴포넌트들 변수 선언
// 3. 