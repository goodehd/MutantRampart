using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopInfo_PopupUI : BaseUI
{
    private Button _closeButton;
    private Button _unitButton;
    private Button _roomButton;
    private Button _groundButton;
    private Button _itemButton;

    private Image _unitInfoImg;
    private Image _roomInfoImg;
    private Image _groundInfoImg;
    private Image _itemInfoImg;

    protected override void Init()
    {
        SetUI<Button>();
        SetUI<Image>();

        _closeButton = GetUI<Button>("ShopInfoCloseBtn");
        _unitButton = GetUI<Button>("InfoUnitBtn");
        _roomButton = GetUI<Button>("InfoRoomBtn");
        _groundButton = GetUI<Button>("InfoGroundBtn");
        _itemButton = GetUI<Button>("InfoItemBtn");

        SetUICallback(_closeButton.gameObject, EUIEventState.Click, ClickCloseBtn);
        SetUICallback(_unitButton.gameObject, EUIEventState.Click, ClickUnitBtn);
        SetUICallback(_roomButton.gameObject, EUIEventState.Click, ClickRoomBtn);
        SetUICallback(_groundButton.gameObject, EUIEventState.Click, ClickGroundBtn);
        SetUICallback(_itemButton.gameObject, EUIEventState.Click, ClickItemBtn);

        _unitInfoImg = GetUI<Image>("UnitInfoImg");
        _roomInfoImg = GetUI<Image>("RoomInfoImg");
        _groundInfoImg = GetUI<Image>("GroundInfoImg");
        _itemInfoImg = GetUI<Image>("ItemInfoImg");
    }

    private void ClickCloseBtn(PointerEventData data)
    {
        Main.Get<UIManager>().ClosePopup();
    }

    private void ClickUnitBtn(PointerEventData data)
    {
        _unitInfoImg.gameObject.SetActive(true);
        _roomInfoImg.gameObject.SetActive(false);
        _groundInfoImg.gameObject.SetActive(false);
        _itemInfoImg.gameObject.SetActive(false);
    }

    private void ClickRoomBtn(PointerEventData data)
    {
        _unitInfoImg.gameObject.SetActive(false);
        _roomInfoImg.gameObject.SetActive(true);
        _groundInfoImg.gameObject.SetActive(false);
        _itemInfoImg.gameObject.SetActive(false);
    }

    private void ClickGroundBtn(PointerEventData data)
    {
        _unitInfoImg.gameObject.SetActive(false);
        _roomInfoImg.gameObject.SetActive(false);
        _groundInfoImg.gameObject.SetActive(true);
        _itemInfoImg.gameObject.SetActive(false);
    }

    private void ClickItemBtn(PointerEventData data)
    {
        _unitInfoImg.gameObject.SetActive(false);
        _roomInfoImg.gameObject.SetActive(false);
        _groundInfoImg.gameObject.SetActive(false);
        _itemInfoImg.gameObject.SetActive(true);
    }
}
