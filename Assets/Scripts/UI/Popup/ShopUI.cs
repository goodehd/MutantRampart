using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopUI : BaseUI
{

    private Button _unitButton;
    private Button _roomButton;
    private Button _groundButton;
    private Button _closeButton;
    private ScrollRect _unitScrollView;
    private ScrollRect _roomScrollView;
    private ScrollRect _groundScrollView;
    private Transform _unitContent;
    private Transform _roomContent;


    protected override void Init()
    {
        SetUI<Button>();
        SetUI<ScrollRect>();
        SetUI<Transform>();

        _unitButton = GetUI<Button>("UnitBtn");
        _roomButton = GetUI<Button>("RoomBtn");
        _groundButton = GetUI<Button>("GroundBtn");
        _closeButton = GetUI<Button>("ShopCloseBtn");

        _unitScrollView = GetUI<ScrollRect>("Unit_Scroll View");
        _roomScrollView = GetUI<ScrollRect>("Room_Scroll View");
        _groundScrollView = GetUI<ScrollRect>("Ground_Scroll View");

        _unitContent = GetUI<Transform>("Unit_Content");
        _roomContent = GetUI<Transform>("Room_Content");

        SetUICallback(_unitButton.gameObject, EUIEventState.Click, ClickUnitBtn);
        SetUICallback(_roomButton.gameObject, EUIEventState.Click, ClickRoomBtn);
        SetUICallback(_groundButton.gameObject, EUIEventState.Click, ClickGroundBtn);
        SetUICallback(_closeButton.gameObject, EUIEventState.Click, ClickCloseBtn);

        // Shop - Unit Items
        List<ShopItemData> shopUnitItems = Main.Get<GameManager>().ShopUnitItems;
        for (int i = 0; i < shopUnitItems.Count; i++)
        {
            Unit_List unitItemsList = Main.Get<UIManager>().CreateSubitem<Unit_List>("Unit_List", _unitContent);
            unitItemsList.ShopUnitData = shopUnitItems[i];
        }

        // Shop - Room Items
        List<ShopItemData> shopRoomItems = Main.Get<GameManager>().ShopRoomItems;
        for (int i = 0; i < shopRoomItems.Count; i++)
        {
            Room_List roomItemsList = Main.Get<UIManager>().CreateSubitem<Room_List>("Room_List", _roomContent);
            roomItemsList.ShopRoomData = shopRoomItems[i];
        }

        // todo : Shop - Ground Item


    }

    private void ClickUnitBtn(PointerEventData eventData)
    {
        // Unit_Scroll View 활성화
        _unitScrollView.gameObject.SetActive(true);
        _roomScrollView.gameObject.SetActive(false);
        _groundScrollView.gameObject.SetActive(false);
    }

    private void ClickRoomBtn(PointerEventData eventData)
    {
        // Room_Scroll View 활성화
        _unitScrollView.gameObject.SetActive(false);
        _roomScrollView.gameObject.SetActive(true);
        _groundScrollView.gameObject.SetActive(false);
    }

    private void ClickGroundBtn(PointerEventData eventData)
    {
        // Ground_Scroll View 활성화
        _unitScrollView.gameObject.SetActive(false);
        _roomScrollView.gameObject.SetActive(false);
        _groundScrollView.gameObject.SetActive(true);
    }

    private void ClickCloseBtn(PointerEventData eventData)
    {
        Main.Get<UIManager>().ClosePopup();
    }
}
