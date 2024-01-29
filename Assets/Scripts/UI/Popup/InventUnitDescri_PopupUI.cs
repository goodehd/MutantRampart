using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventUnitDescri_PopupUI : BaseUI
{
    private Button _closeBtn;
    private Button _deleteBtn;
    private Button[] _equipSlots = new Button[3];
    //private Button _firstSlot;
    //private Button _secondSlot;
    //private Button _thirdSlot;

    private TMP_Text _unitName;
    private TMP_Text _unitDescription;

    private Image _unitImg;
    private Image[] _equipSlotsImgs = new Image[3];
    private Image[] _equipCancelImgs = new Image[3];
    //private Image _firstEquipCancelImg;
    //private Image _secondEquipCancelImg;
    //private Image _thirdEquipCancelImg;

    private ScrollRect _myItemsScrollView;

    private Transform _myItemsContent;

    public Character UnitData { get; set; }

    public Inventory_PopupUI inventoryPopupUI;


    protected override void Init()
    {
        SetUI<Button>();
        SetUI<TMP_Text>();
        SetUI<Image>();
        SetUI<ScrollRect>();
        SetUI<Transform>();

        _closeBtn = GetUI<Button>("InventUnitCloseBtn");
        _deleteBtn = GetUI<Button>("InventUnitDeleteBtn");

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

        SetUICallback(_closeBtn.gameObject, EUIEventState.Click, ClickInventUnitCloseBtn);
        SetUICallback(_deleteBtn.gameObject, EUIEventState.Click, ClickInventUnitDeleteBtn);
        SetUICallback(_equipSlots[0].gameObject, EUIEventState.Click, ClickFirstSlot);
        SetUICallback(_equipSlots[1].gameObject, EUIEventState.Click, ClickSecondSlot);
        SetUICallback(_equipSlots[2].gameObject, EUIEventState.Click, ClickThirdSlot);

        _unitName = GetUI<TMP_Text>("InventUnitNameTxt");
        _unitDescription = GetUI<TMP_Text>("InventUnitDescriTxt");

        _unitImg = GetUI<Image>("InventUnitImg");

        _myItemsScrollView = GetUI<ScrollRect>("MyItems_Scroll View");

        _myItemsContent = GetUI<Transform>("MyItems_Content");

        // 아이템 (유닛 능력 올려주는, '서브아이템'이라고 하겠음.)
        // 1. 게임매니저에 구매한 서브아이템 담을 list 만들어서 연결하기
        // 2. MyItems_Content 자식으로 List.Count 만큼 생성하기

        //  List<ThisRoom> playerSubItems = Main.Get<GameManager>().PlayerRooms;
        //foreach (Transform item in _myItemsContent.transform) // todo : 이건 무슨의미일까 ? 초기화 관련한걸까 ?
        //{
        //    Destroy(item.gameObject);
        //}
        //for (int i = 0; i < playerRooms.Count; i++)
        //{
        //    MyItemsImgBtnUI inventSubItems = Main.Get<UIManager>().CreateSubitem<MyItemsImgBtnUI>("MyItemsImgBtnUI", _myItemsContent);
        //    inventSubItems.RoomData = playerRooms[i];
        //}

        SetInfo();
    }

    private void SetInfo()
    {
        _unitName.text = UnitData.Data.Key;
        _unitDescription.text = $"Hp : {UnitData.Data.Hp.ToString()}\nDamage : {UnitData.Data.Damage.ToString()}\nDefense : {UnitData.Data.Defense.ToString()}\nATK Speed : {UnitData.Data.AttackSpeed.ToString()}";
        _unitImg.sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.UNIT_SPRITE_PATH}{UnitData.Data.Key}");

    }
    private void ClickInventUnitCloseBtn(PointerEventData EventData)
    {
        Main.Get<UIManager>().ClosePopup();
    }
    private void ClickInventUnitDeleteBtn(PointerEventData EventData)
    {
        // 인벤토리를 껐다가 다시 키면 없어져있긴 하는데 삭제되는 순간에 바로 인벤토리 업데이트까지는 안 됨..
        Main.Get<GameManager>().playerUnits.Remove(UnitData);
        //inventoryPopupUI.SetUnitInventory();
    }
    private void ClickFirstSlot(PointerEventData EventData)
    {
        // 1번 장착된 아이템 해제
    }
    private void ClickSecondSlot(PointerEventData EventData)
    {
        // 2번 장착된 아이템 해제
    }
    private void ClickThirdSlot(PointerEventData EventData)
    {
        // 3번 장착된 아이템 해제
    }

}

// slot 위에 마우스 hovered 하면 _equipCancelImgs active 되게끔 츄라이 !
// todo : 보유중인 아이템(MyItems_Content) 위에 마우스 올렸을 떄 정보가 뜨게끔 !

/*
 *   
        //_inventItemNameTxt = GetUI<TMP_Text>("InventItemNameTxt");
        //_inventItemDescriTxt = GetUI<TMP_Text>("InventItemDescriTxt");

        //_inventItemNameTxt.text = _selectItemNameTxt;
        //_inventItemDescriTxt.text = _selectItemDescriTxt;

        _inventUnitImg = GetUI<Image>("InventUnitImg");
        //_inventUnitImg.sprite = 
        _inventItemDetailBox = GetUI<Image>("InventItemDetailBox");

        _myItemsContent = GetUI<Transform>("MyItems_Content");

    }
*/

