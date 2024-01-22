using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopUI : BaseUI
{

    private Button _unitButton;
    private Button _tileButton;
    private Button _groundButton;
    private Button _closeButton;
    private ScrollRect _unitScrollView;
    private ScrollRect _tiletScrollView;
    private ScrollRect _groundScrollView;

    protected override void Init()
    {
        SetUI<Button>();
        SetUI<ScrollRect>();


        _unitButton = GetUI<Button>("UnitBtn");
        _tileButton = GetUI<Button>("TileBtn");
        _groundButton = GetUI<Button>("GroundBtn");
        _closeButton = GetUI<Button>("ShopCloseBtn");

        _unitScrollView = GetUI<ScrollRect>("Unit_Scroll View");
        _tiletScrollView = GetUI<ScrollRect>("Tile_Scroll View");
        _groundScrollView = GetUI<ScrollRect>("Ground_Scroll View");

        SetUICallback(_unitButton.gameObject, EUIEventState.Click, ClickUnitBtn);
        SetUICallback(_tileButton.gameObject, EUIEventState.Click, ClickTileBtn);
        SetUICallback(_groundButton.gameObject, EUIEventState.Click, ClickGroundBtn);
        SetUICallback(_closeButton.gameObject, EUIEventState.Click, ClickCloseBtn);


    }

    private void ClickUnitBtn(PointerEventData eventData)
    {
        // Unit_Scroll View 활성화
        _unitScrollView.gameObject.SetActive(true);
        _tiletScrollView.gameObject.SetActive(false);
        _groundScrollView.gameObject.SetActive(false);
    }

    private void ClickTileBtn(PointerEventData eventData)
    {
        // Tile_Scroll View 활성화
        _unitScrollView.gameObject.SetActive(false);
        _tiletScrollView.gameObject.SetActive(true);
        _groundScrollView.gameObject.SetActive(false);
    }

    private void ClickGroundBtn(PointerEventData eventData)
    {
        // Ground_Scroll View 활성화
        _unitScrollView.gameObject.SetActive(false);
        _tiletScrollView.gameObject.SetActive(false);
        _groundScrollView.gameObject.SetActive(true);
    }

    private void ClickCloseBtn(PointerEventData eventData)
    {
        Main.Get<UIManager>().ClosePopup();
    }
}
