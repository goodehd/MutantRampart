using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop_PopupUI : BaseUI
{

    private Button _unitButton;
    private Button _roomButton;
    private Button _groundButton;
    private Button _itemButton;
    private Button _closeButton;
    private ScrollRect _unitScrollView;
    private ScrollRect _roomScrollView;
    private ScrollRect _groundScrollView;
    private ScrollRect _itemScrollView;
    private Transform _unitContent;
    private Transform _roomContent;

    private TMP_Text _playerMoneyText;

    public List<CharacterData> ShopUnitItems { get; private set; } = new List<CharacterData>(); // 상점 - UnitItems
    public List<RoomData> ShopRoomItems { get; private set; } = new List<RoomData>(); // 상점 - RoomItems


    protected override void Init()
    {
        SetUI<Button>();
        SetUI<ScrollRect>();
        SetUI<Transform>();
        SetUI<TMP_Text>();

        _unitButton = GetUI<Button>("UnitBtn");
        _roomButton = GetUI<Button>("RoomBtn");
        _groundButton = GetUI<Button>("GroundBtn");
        _itemButton = GetUI<Button>("ItemBtn");
        _closeButton = GetUI<Button>("ShopCloseBtn");

        _unitScrollView = GetUI<ScrollRect>("Unit_Scroll View");
        _roomScrollView = GetUI<ScrollRect>("Room_Scroll View");
        _groundScrollView = GetUI<ScrollRect>("Ground_Scroll View");
        _itemScrollView = GetUI<ScrollRect>("Item_Scroll View");

        _unitContent = GetUI<Transform>("Unit_Content");
        _roomContent = GetUI<Transform>("Room_Content");

        _playerMoneyText = GetUI<TMP_Text>("MoneyText");
        _playerMoneyText.text = Main.Get<GameManager>()._playerMoney.ToString();

        Main.Get<GameManager>().OnChangeMoney += UpdateMoneyText;

        SetUICallback(_unitButton.gameObject, EUIEventState.Click, ClickUnitBtn);
        SetUICallback(_roomButton.gameObject, EUIEventState.Click, ClickRoomBtn);
        SetUICallback(_groundButton.gameObject, EUIEventState.Click, ClickGroundBtn);
        SetUICallback(_itemButton.gameObject, EUIEventState.Click, ClickItemBtn);
        SetUICallback(_closeButton.gameObject, EUIEventState.Click, ClickCloseBtn);
        
        // 상점 판매 아이템 추가
        ShopUnitItems.Add(Main.Get<DataManager>().Character["Gun"]);
        ShopUnitItems.Add(Main.Get<DataManager>().Character["Jotem"]);
        ShopUnitItems.Add(Main.Get<DataManager>().Character["Warrior"]);

        ShopRoomItems.Add(Main.Get<DataManager>().roomDatas["Forest"]);
        ShopRoomItems.Add(Main.Get<DataManager>().roomDatas["Igloo"]);
        ShopRoomItems.Add(Main.Get<DataManager>().roomDatas["Lava"]);
        ShopRoomItems.Add(Main.Get<DataManager>().roomDatas["LivingRoom"]);
        ShopRoomItems.Add(Main.Get<DataManager>().roomDatas["Molar"]);
        ShopRoomItems.Add(Main.Get<DataManager>().roomDatas["Snow"]);
        ShopRoomItems.Add(Main.Get<DataManager>().roomDatas["Temple"]);

        // Shop - Unit Items
        for (int i = 0; i < ShopUnitItems.Count; i++)
        {
            Unit_ListUI unitItemsList = Main.Get<UIManager>().CreateSubitem<Unit_ListUI>("Unit_ListUI", _unitContent);
            unitItemsList.ShopUnitData = ShopUnitItems[i];
        }

        // Shop - Room Items
        for (int i = 0; i < ShopRoomItems.Count; i++)
        {
            Room_ListUI roomItemsList = Main.Get<UIManager>().CreateSubitem<Room_ListUI>("Room_ListUI", _roomContent);
            roomItemsList.ShopRoomData = ShopRoomItems[i];
        }

        // todo : Shop - Ground Item

        // todo : Shop - Item

    }

    private void UpdateMoneyText(int amount)
    {
        _playerMoneyText.text = amount.ToString();
    }

    private void ClickUnitBtn(PointerEventData eventData)
    {
        // Unit_Scroll View 활성화
        _unitScrollView.gameObject.SetActive(true);
        _roomScrollView.gameObject.SetActive(false);
        _groundScrollView.gameObject.SetActive(false);
        _itemScrollView.gameObject.SetActive(false);
    }

    private void ClickRoomBtn(PointerEventData eventData)
    {
        // Room_Scroll View 활성화
        _unitScrollView.gameObject.SetActive(false);
        _roomScrollView.gameObject.SetActive(true);
        _groundScrollView.gameObject.SetActive(false);
        _itemScrollView.gameObject.SetActive(false);
    }

    private void ClickGroundBtn(PointerEventData eventData)
    {
        // Ground_Scroll View 활성화
        _unitScrollView.gameObject.SetActive(false);
        _roomScrollView.gameObject.SetActive(false);
        _groundScrollView.gameObject.SetActive(true);
        _itemScrollView.gameObject.SetActive(false);
    }

    private void ClickItemBtn(PointerEventData eventData)
    {
        // Item_Scroll View 활성화
        _unitScrollView.gameObject.SetActive(false);
        _roomScrollView.gameObject.SetActive(false);
        _groundScrollView.gameObject.SetActive(false);
        _itemScrollView.gameObject.SetActive(true);
    }

    private void ClickCloseBtn(PointerEventData eventData)
    {
        Main.Get<UIManager>().ClosePopup();
    }
}