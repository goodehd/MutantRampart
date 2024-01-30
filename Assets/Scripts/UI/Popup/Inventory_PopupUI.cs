using System.Collections;
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


        // 인벤토리 room & unit content 안에 보유 아이템 연결해주기 !

        SetUnitInventory();
        SetRoomInventory();




    }

    private void SetInfo()
    {

    }

    public void SetUnitInventory()
    {
        // unit
        List<Character> playerUnits = Main.Get<GameManager>().playerUnits;
        foreach (Transform item in _inventUnitContent.transform) // todo : 초기화 관련 ?
        {
            Destroy(item.gameObject);
        }
        for (int i = 0; i < playerUnits.Count; i++)
        {
            InventUnit_ContentsBtnUI inventUnitItems = Main.Get<UIManager>().CreateSubitem<InventUnit_ContentsBtnUI>("InventUnit_ContentsBtnUI", _inventUnitContent);
            inventUnitItems.UnitData = playerUnits[i];
            //inventUnitItems.inventoryPopupUIOwner = this;
        }
    }

    public void SetRoomInventory()
    {
        // room
        List<ThisRoom> playerRooms = Main.Get<GameManager>().PlayerRooms;
        foreach (Transform item in _inventRoomContent.transform)
        {
            Destroy(item.gameObject);
        }
        for (int i = 0; i < playerRooms.Count; i++)
        {
            InventRoom_ContentsBtnUI inventRoomItems = Main.Get<UIManager>().CreateSubitem<InventRoom_ContentsBtnUI>("InventRoom_ContentsBtnUI", _inventRoomContent);
            inventRoomItems.RoomData = playerRooms[i];
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
        

    }
    private void ClickUnitBtn(PointerEventData EventData)
    {
        _inventRoomScrollView.gameObject.SetActive(false);
        _inventUnitScrollView.gameObject.SetActive(true);
    }    

    private void ClickCloseBtn(PointerEventData EventData)
    {
        Main.Get<UIManager>().CloseAllPopup();
        Owner.isInventOpen = false;
    }
    
}


