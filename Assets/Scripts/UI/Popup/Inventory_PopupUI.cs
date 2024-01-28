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
    private Button _inventRoomUpgradeBtn;
    private Button _inventRoomDeleteBtn;
    private Button _upgradeConfirmButton;
    private Button _upgradeCancelBtn;

    private TMP_Text _inventRoomNameTxt;
    private TMP_Text _inventRoomTypeTxt;
    private TMP_Text _inventRoomDescriTxt;
    private TMP_Text _curUpgradeTxt;
    private TMP_Text _nextUpgradeTxt;
    private TMP_Text _priceTxt;

    private Image _inventRoomImg;
    private Image _upgradePopup;

    // InventUnitDescriBox
    private Button _inventUnitCloseBtn;
    private Button _inventUnitDeleteBtn;
    private Button[] _equipSlots = new Button[3];
    //private Button _firstSlot;
    //private Button _secondSlot;
    //private Button _thirdSlot;

    private TMP_Text _inventUnitNameTxt;
    private TMP_Text _inventUnitDescriTxt;
    private TMP_Text _inventItemNameTxt;
    private TMP_Text _inventItemDescriTxt;

    private Image _inventUnitImg;
    private Image[] _equipSlotsImgs = new Image[3];
    private Image[] _equipCancelImgs = new Image[3];
    //private Image _firstEquipCancelImg;
    //private Image _secondEquipCancelImg;
    //private Image _thirdEquipCancelImg;
    private Image _inventItemDetailBox;

    private Transform _myItemsContent;


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

        SetUIInventRoomDescriBox();
        SetUIInventUnitDescriBox();

    }

    private void SetUIInventRoomDescriBox() // Set _inventRoomDescriBox
    {
        _inventRoomCloseBtn = GetUI<Button>("InventRoomCloseBtn");
        _inventRoomUpgradeBtn = GetUI<Button>("InventRoomUpgradeBtn");
        _inventRoomDeleteBtn = GetUI<Button>("InventRoomDeleteBtn");
        _upgradeConfirmButton = GetUI<Button>("UpgradeConfirmBtn");
        _upgradeCancelBtn = GetUI<Button>("UpgradeCancelBtn");

        SetUICallback(_inventRoomCloseBtn.gameObject, EUIEventState.Click, ClickInventRoomCloseBtn);
        SetUICallback(_inventRoomUpgradeBtn.gameObject, EUIEventState.Click, ClickInventRoomUpgradeBtn);
        SetUICallback(_inventRoomDeleteBtn.gameObject, EUIEventState.Click, ClickInventRoomDeleteBtn);
        SetUICallback(_upgradeConfirmButton.gameObject, EUIEventState.Click, ClickUpgradeConfirmButton);
        SetUICallback(_upgradeCancelBtn.gameObject, EUIEventState.Click, ClickUpgradeCancelButton);

        _inventRoomNameTxt = GetUI<TMP_Text>("InventRoomNameTxt");
        _inventRoomTypeTxt = GetUI<TMP_Text>("InventRoomTypeTxt");
        _inventRoomDescriTxt = GetUI<TMP_Text>("InventRoomDescriTxt");
        _curUpgradeTxt = GetUI<TMP_Text>("CurTxt");
        _nextUpgradeTxt = GetUI<TMP_Text>("NextTxt");
        _priceTxt = GetUI<TMP_Text>("PriceTxt");

        _inventRoomNameTxt.text = _selectRoomNameTxt;
        _inventRoomTypeTxt.text = _selectRoomTypeTxt;
        _inventRoomDescriTxt.text = _selectRoomDescriTxt;
        _curUpgradeTxt.text = _upgradeCurTxt;
        _nextUpgradeTxt.text = _upgradeNextTxt;
        _priceTxt.text = _upgradePriceTxt;

        _inventRoomImg = GetUI<Image>("InventRoomImg");
        // todo : _inventRoomImg.sprite = 
        _upgradePopup = GetUI<Image>("UpgradePopup");

    }

    private void SetUIInventUnitDescriBox() // Set InventUnitDescriBox
    {
        _inventUnitCloseBtn = GetUI<Button>("InventUnitCloseBtn");
        _inventUnitDeleteBtn = GetUI<Button>("InventUnitDeleteBtn");

        for (int i = 0; i < _equipSlots.Length; i++)
        { // slot 의 버튼과 이미지와, 장착여부 이미지
            _equipSlots[i] = GetUI<Button>($"EquipSlot{i + 1}");
            _equipSlotsImgs[i] = GetUI<Image>($"EquipSlot{i + 1}");
            _equipCancelImgs[i] = GetUI<Image>($"EquipCancelImg{i + 1}");

            if (i < 1)
            {
                _equipSlotsImgs[i].color = Color.white;
            }
        }

        SetUICallback(_inventUnitCloseBtn.gameObject, EUIEventState.Click, ClickInventUnitCloseBtn);
        SetUICallback(_inventUnitDeleteBtn.gameObject, EUIEventState.Click, ClickInventUnitDeleteBtn);
        SetUICallback(_equipSlots[0].gameObject, EUIEventState.Click, ClickFirstSlot);
        SetUICallback(_equipSlots[1].gameObject, EUIEventState.Click, ClickSecondSlot);
        SetUICallback(_equipSlots[2].gameObject, EUIEventState.Click, ClickThirdSlot);

        _inventUnitNameTxt = GetUI<TMP_Text>("InventUnitNameTxt");
        _inventUnitDescriTxt = GetUI<TMP_Text>("InventUnitDescriTxt");
        _inventItemNameTxt = GetUI<TMP_Text>("InventItemNameTxt");
        _inventItemDescriTxt = GetUI<TMP_Text>("InventItemDescriTxt");

        _inventUnitNameTxt.text = _selectUnitNameTxt;
        _inventUnitDescriTxt.text = _selectUnitDescriTxt;
        _inventItemNameTxt.text = _selectItemNameTxt;
        _inventItemDescriTxt.text = _selectItemDescriTxt;

        _inventUnitImg = GetUI<Image>("InventUnitImg");
        //_inventUnitImg.sprite = 
        _inventItemDetailBox = GetUI<Image>("InventItemDetailBox");

        _myItemsContent = GetUI<Transform>("MyItems_Content");

    }
 
    // todo : 보유중인 아이템(MyItems_Content) 위에 마우스 올렸을 떄 정보가 뜨게끔 !

    private void SetInfo()
    {

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

    private void ClickInventRoomDeleteBtn(PointerEventData EventData) 
    {

    }

    private void ClickUpgradeConfirmButton(PointerEventData EventData)
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

    private void ClickInventUnitDeleteBtn(PointerEventData EventData)
    {

    }

    private void ClickFirstSlot(PointerEventData EventData) 
    {

    }

    private void ClickSecondSlot(PointerEventData EventData) 
    {

    }

    private void ClickThirdSlot(PointerEventData EventData) 
    {

    }
}
