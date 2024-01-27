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
    private Button _inventoryCloseBtn;

    private ScrollRect _inventRoomScrollView;
    private ScrollRect _inventUnitScrollView;

    private Image _inventRoomDescriBox;
    private Image _inventUnitDescriBox;

    private Transform _inventRoomContent;
    private Transform _inventUnitContent;

    // InventRoomDescriBox
    private Button _inventRoomCloseBtn;
    private TMP_Text _inventRoomNameTxt;
    private TMP_Text _inventRoomTypeTxt;
    private TMP_Text _inventRoomDescriTxt;
    private Image _inventRoomImg;
    private Button _inventRoomUpgradeBtn;
    private Button _inventRoomDeleteBtn;
    private Image _upgradePopup;
    private TMP_Text _curTxt;
    private TMP_Text _nextTxt;
    private Button _upgradeConfirmButton;
    private Button _upgradeCancelBtn;
    private TMP_Text _priceTxt;

    // InventUnitDescriBox
    private Button _inventUnitCloseBtn;
    private TMP_Text _inventUnitNameTxt;
    private Image _inventUnitImg;
    private TMP_Text _inventUnitDescriTxt;
    private Button _inventUnitDeleteBtn;
    private Button _firstSlot;
    private Image _firstEquipCancelImg;
    private Button _secondSlot;
    private Image _secondEquipCancelImg;
    private Button _thirdSlot;
    private Image _thirdEquipCancelImg;
    private Transform _myItemsContent;
    private Image _inventItemDetailBox;
    private TMP_Text _inventItemNameTxt;
    private TMP_Text _inventItemDescriTxt;


    public string _selectRoomNameTxt { get; set; }
    public string _selectRoomTypeTxt { get; set; }
    public string _selectRoomDescriTxt { get; set; }
    public string _upgradeCurTxt { get; set; }
    public string _upgradeNextTxt { get; set; }
    public string _upgradePriceTxt { get; set; }
    public string _selectUnitNameTxt { get; set; }
    public string _selectUnitDescriTxt { get; set; }
    public string _selectItemNameTxt { get; set; }
    public string _selectItemDescriTxt { get; set; }

    //public List<Room> InventRoomItems { get; private set; } = new List<Room>(); // 인벤 - RoomItems
    //public List<Character> InventUnitItems { get; private set; } = new List<Character>(); // 인벤 - UnitItems




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
        _inventoryCloseBtn = GetUI<Button>("InventoryCloseBtn");

        SetUICallback(_backButton.gameObject, EUIEventState.Click, ClickBackBtn);
        SetUICallback(_roomButton.gameObject, EUIEventState.Click, ClickRoomBtn);
        SetUICallback(_unitButton.gameObject, EUIEventState.Click, ClickUnitBtn);
        SetUICallback(_inventoryCloseBtn.gameObject, EUIEventState.Click, ClickBackBtn);

        _inventRoomScrollView = GetUI<ScrollRect>("InventRoom_Scroll View");
        _inventUnitScrollView = GetUI<ScrollRect>("InventUnit_Scroll View");

        _inventRoomContent = GetUI<Transform>("InventRoom_Content");
        _inventUnitContent = GetUI<Transform>("InventUnit_Content");

        _inventRoomDescriBox = GetUI<Image>("InventRoomDescriBox");
        _inventUnitDescriBox = GetUI<Image>("InventUnitDescriBox");

        // todo : 인벤토리 room & unit content 안에 내용 넣어줘야 함 !

        SetInventRoomDescriBox();
        SetInventUnitDescriBox();

    }

    private void SetInventRoomDescriBox()
    {
        // Set _inventRoomDescriBox
        _inventRoomCloseBtn = GetUI<Button>("InventRoomCloseBtn");
        SetUICallback(_inventRoomCloseBtn.gameObject, EUIEventState.Click, ClickInventRoomCloseBtn);
        _inventRoomNameTxt = GetUI<TMP_Text>("InventRoomNameTxt");
        _inventRoomNameTxt.text = _selectRoomNameTxt;
        _inventRoomTypeTxt = GetUI<TMP_Text>("InventRoomTypeTxt");
        _inventRoomTypeTxt.text = _selectRoomTypeTxt;
        _inventRoomDescriTxt = GetUI<TMP_Text>("InventRoomDescriTxt");
        _inventRoomDescriTxt.text = _selectRoomDescriTxt;
        _inventRoomImg = GetUI<Image>("InventRoomImg");
        // todo : _inventRoomImg.sprite = 
        _inventRoomUpgradeBtn = GetUI<Button>("InventRoomUpgradeBtn");
        SetUICallback(_inventRoomUpgradeBtn.gameObject, EUIEventState.Click, ClickInventRoomUpgradeBtn);
        _inventRoomDeleteBtn = GetUI<Button>("InventRoomDeleteBtn");
        SetUICallback(_inventRoomDeleteBtn.gameObject, EUIEventState.Click, ClickInventRoomDeleteBtn);
        _upgradePopup = GetUI<Image>("UpgradePopup");
        _curTxt = GetUI<TMP_Text>("CurTxt");
        _curTxt.text = _upgradeCurTxt;
        _nextTxt = GetUI<TMP_Text>("NextTxt");
        _nextTxt.text = _upgradeNextTxt;
        _upgradeConfirmButton = GetUI<Button>("UpgradeConfirmBtn");
        SetUICallback(_upgradeConfirmButton.gameObject, EUIEventState.Click, ClickUpgradeConfirmButton);
        _upgradeCancelBtn = GetUI<Button>("UpgradeCancelBtn");
        SetUICallback(_upgradeCancelBtn.gameObject, EUIEventState.Click, ClickUpgradeCancelButton);
        _priceTxt = GetUI<TMP_Text>("PriceTxt");
        _priceTxt.text = _upgradePriceTxt;
    }

    private void SetInventUnitDescriBox()
    {
        // Set InventUnitDescriBox
        _inventUnitCloseBtn = GetUI<Button>("InventUnitCloseBtn");
        SetUICallback(_inventUnitCloseBtn.gameObject, EUIEventState.Click, ClickInventUnitCloseBtn);
        _inventUnitNameTxt = GetUI<TMP_Text>("InventUnitNameTxt");
        _inventUnitNameTxt.text = _selectUnitNameTxt;
        _inventUnitImg = GetUI<Image>("InventUnitImg");
        //_inventUnitImg.sprite = 
        _inventUnitDescriTxt = GetUI<TMP_Text>("InventUnitDescriTxt");
        _inventUnitDescriTxt.text = _selectUnitDescriTxt;
        _inventUnitDeleteBtn = GetUI<Button>("InventUnitDeleteBtn");
        SetUICallback(_inventUnitDeleteBtn.gameObject, EUIEventState.Click, ClickInventUnitDeleteBtn);
        _firstSlot = GetUI<Button>("FirstSlot");
        // todo : _firstSlot.image = 
        SetUICallback(_firstSlot.gameObject, EUIEventState.Click, ClickFirstSlot);
        _firstEquipCancelImg = GetUI<Image>("FirstEquipCancelImg");
        _secondSlot = GetUI<Button>("SecondSlot");
        // todo : _secondSlot.image = 
        SetUICallback(_secondSlot.gameObject, EUIEventState.Click, ClickSecondSlot);
        _secondEquipCancelImg = GetUI<Image>("SecondEquipCancelImg");
        _thirdSlot = GetUI<Button>("ThirdSlot");
        // todo : _thirdSlot.image = 
        SetUICallback(_thirdSlot.gameObject, EUIEventState.Click, ClickThirdSlot);
        _thirdEquipCancelImg = GetUI<Image>("ThirdEquipCancelImg");
        _myItemsContent = GetUI<Transform>("MyItems_Content");
        // todo : 보유중인 아이템(MyItems_Content) 위에 마우스 올렸을 떄 정보가 뜨게끔 !
        _inventItemDetailBox = GetUI<Image>("InventItemDetailBox");
        _inventItemNameTxt = GetUI<TMP_Text>("InventItemNameTxt");
        _inventItemNameTxt.text = _selectItemNameTxt;
        _inventItemDescriTxt = GetUI<TMP_Text>("InventItemDescriTxt");
        _inventItemDescriTxt.text = _selectItemDescriTxt;
    }

    private void ClickBackBtn(PointerEventData EventData)
    {
        Main.Get<UIManager>().ClosePopup();
    }

    private void ClickRoomBtn(PointerEventData EventData)
    {
        _inventRoomScrollView.gameObject.SetActive(true);
        _inventUnitScrollView.gameObject.SetActive(false);
        // todo : 아래는 scroll view 에서 content 하나 클릭했을 때 떠야하는내용
        _inventRoomDescriBox.gameObject.SetActive(true);
        _inventUnitDescriBox.gameObject.SetActive(false);

    }
    private void ClickUnitBtn(PointerEventData EventData)
    {
        _inventRoomScrollView.gameObject.SetActive(false);
        _inventUnitScrollView.gameObject.SetActive(true);
        // todo : 아래는 scroll view 에서 content 하나 클릭했을 때 떠야하는내용
        _inventRoomDescriBox.gameObject.SetActive(false);
        _inventUnitDescriBox.gameObject.SetActive(true);
    }

    private void ClickInventRoomCloseBtn(PointerEventData EventData)
    {
        _inventRoomDescriBox.gameObject.SetActive(false);
    }

    private void ClickInventRoomUpgradeBtn(PointerEventData EventData)
    {
        _upgradePopup.gameObject.SetActive(true);
    }

    private void ClickInventRoomDeleteBtn(PointerEventData EventData) // todo : ClickInventRoomDeleteBtn()
    {
        
    }

    private void ClickUpgradeConfirmButton(PointerEventData EventData) // todo : ClickUpgradeConfirmButton()
    {

    }
    
    private void ClickUpgradeCancelButton(PointerEventData EventData)
    {
        _upgradePopup.gameObject.SetActive(false);
    }

    private void ClickInventUnitCloseBtn(PointerEventData EventData)
    {
        _inventUnitDescriBox.gameObject.SetActive(false);
    }

    private void ClickInventUnitDeleteBtn(PointerEventData EventData) // todo : ClickInventUnitDeleteBtn()
    {

    }

    private void ClickFirstSlot(PointerEventData EventData) // todo : ClickFirstSlot()
    {

    }

    private void ClickSecondSlot(PointerEventData EventData) // todo : ClickSecondSlot()
    {

    }

    private void ClickThirdSlot(PointerEventData EventData) // todo : ClickThirdSlot()
    {

    }
}
