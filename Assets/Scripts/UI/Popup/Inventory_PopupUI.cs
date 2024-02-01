using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory_PopupUI : BaseUI
{
    private Button _backButton;
    private Button _roomButton;
    private Button _unitButton;
    private Button _closeBtn;

    private ScrollRect _inventRoomScrollView;
    private ScrollRect _inventUnitScrollView;

    private Transform _inventRoomContent;
    private Transform _inventUnitContent;

    //private TMP_Text _inventItemNameTxt;
    //private TMP_Text _inventItemDescriTxt;

    //private Image _inventUnitImg;

    // InventItemDetailBox 관련
    //private Image _inventItemDetailBox;
    //private Transform _myItemsContent;

    //public string _selectItemNameTxt { get; set; }
    //public string _selectItemDescriTxt { get; set; }

    public DayMain_SceneUI Owner { get; set; }

    public InventRoomDescri_PopupUI inventRoomDescri_PopupUI;

    public InventUnitDescri_PopupUI inventUnitDescri_PopupUI;

    protected override void Init()
    {
        SetUI<Button>();
        SetUI<Image>();
        SetUI<TMP_Text>();
        SetUI<Transform>();
        SetUI<ScrollRect>();

        _backButton = GetUI<Button>("InventBackBtn");
        _roomButton = GetUI<Button>("InventRoomBtn");
        _unitButton = GetUI<Button>("InventUnitBtn");
        _closeBtn = GetUI<Button>("InventoryCloseBtn");

        SetUICallback(_backButton.gameObject, EUIEventState.Click, ClickBackBtn);
        SetUICallback(_roomButton.gameObject, EUIEventState.Click, ClickRoomBtn);
        SetUICallback(_unitButton.gameObject, EUIEventState.Click, ClickUnitBtn);
        SetUICallback(_closeBtn.gameObject, EUIEventState.Click, ClickCloseBtn);

        _inventRoomScrollView = GetUI<ScrollRect>("InventRoom_Scroll View");
        _inventUnitScrollView = GetUI<ScrollRect>("InventUnit_Scroll View");

        _inventRoomContent = GetUI<Transform>("InventRoom_Content");
        _inventUnitContent = GetUI<Transform>("InventUnit_Content");


        SetRoomInventory();
        SetUnitInventory();
    }

    public void SetRoomInventory()
    {
        // room
        List<ThisRoom> playerRooms = Main.Get<GameManager>().PlayerRooms;
        foreach (Transform item in _inventRoomContent.transform) // todo : 초기화 관련 ?
        {
            Destroy(item.gameObject);
        }
        for (int i = 0; i < playerRooms.Count; i++)
        {
            InventRoom_ContentsBtnUI inventRoomItems = Main.Get<UIManager>().CreateSubitem<InventRoom_ContentsBtnUI>("InventRoom_ContentsBtnUI", _inventRoomContent);
            inventRoomItems.RoomData = playerRooms[i];
            inventRoomItems.Owner = this;
        }
    }

    public void SetUnitInventory()
    {
        // unit
        List<Character> playerUnits = Main.Get<GameManager>().playerUnits;
        foreach (Transform item in _inventUnitContent.transform)
        {
            Destroy(item.gameObject);
        }
        for (int i = 0; i < playerUnits.Count; i++)
        {
            InventUnit_ContentsBtnUI inventUnitItems = Main.Get<UIManager>().CreateSubitem<InventUnit_ContentsBtnUI>("InventUnit_ContentsBtnUI", _inventUnitContent);
            inventUnitItems.UnitData = playerUnits[i];
            inventUnitItems.Owner = this;
        }
    }


    private void ClickBackBtn(PointerEventData EventData)
    {
        Main.Get<UIManager>().CloseAllPopup();
        Owner.isInventOpen = false;

    }

    private void ClickRoomBtn(PointerEventData EventData)
    {
        _inventRoomScrollView.gameObject.SetActive(true);
        _inventUnitScrollView.gameObject.SetActive(false);

        if (inventUnitDescri_PopupUI != null) // unit description 창이 켜져있다면
        {
            inventUnitDescri_PopupUI.Owner._selectCheckImg.gameObject.SetActive(false);
            Main.Get<UIManager>().ClosePopup(); // stack 으로 popup 이 쌓임. -> 후입선출 되는 흐름 ! 그래서 ClosePopup 하게 되면 제일 최근에 생성된 UnitDescri_PopupUI 가 닫히는 것이다 !
            inventUnitDescri_PopupUI = null;
        }
    }

    private void ClickUnitBtn(PointerEventData EventData)
    {
        _inventRoomScrollView.gameObject.SetActive(false);
        _inventUnitScrollView.gameObject.SetActive(true);

        if (inventRoomDescri_PopupUI != null) // room description 설명창이 열려 있다면
        {
            inventRoomDescri_PopupUI.Owner._selectCheckImg.gameObject.SetActive(false);
            Main.Get<UIManager>().ClosePopup();
            inventRoomDescri_PopupUI = null;
        }
    }

    private void ClickCloseBtn(PointerEventData EventData)
    {
        Main.Get<UIManager>().CloseAllPopup();
        Owner.isInventOpen = false;
    }

}


