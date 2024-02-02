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

    private List<MyItemsImgBtnUI> inventSubItems = new List<MyItemsImgBtnUI>();
    
    //public Inventory_PopupUI inventoryPopupUIOwner { get; set; }
    
    public InventUnit_ContentsBtnUI Owner { get; set; }


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
        {
            // slot 의 버튼과 이미지와, 장착여부 이미지
            _equipSlots[i] = GetUI<Button>($"EquipSlotBtn{i + 1}");
            _equipSlotsImgs[i] = GetUI<Image>($"EquipSlotBtn{i + 1}");
            _equipCancelImgs[i] = GetUI<Image>($"EquipCancelImg{i + 1}");

        }

        SetUICallback(_closeBtn.gameObject, EUIEventState.Click, ClickInventUnitCloseBtn);
        SetUICallback(_deleteBtn.gameObject, EUIEventState.Click, ClickInventUnitDeleteBtn);
        SetUICallback(_equipSlots[0].gameObject, EUIEventState.Click, ClickFirstSlot);
        SetUICallback(_equipSlots[1].gameObject, EUIEventState.Click, ClickSecondSlot);
        SetUICallback(_equipSlots[2].gameObject, EUIEventState.Click, ClickThirdSlot);
        SetUICallback(_equipSlots[0].gameObject, EUIEventState.Hovered, HoveredFirstSlot);
        SetUICallback(_equipSlots[0].gameObject, EUIEventState.Exit, ExitFirstSlot);
        SetUICallback(_equipSlots[1].gameObject, EUIEventState.Hovered, HoveredSecondSlot);
        SetUICallback(_equipSlots[1].gameObject, EUIEventState.Exit, ExitSecondSlot);
        SetUICallback(_equipSlots[2].gameObject, EUIEventState.Hovered, HoveredThirdSlot);
        SetUICallback(_equipSlots[2].gameObject, EUIEventState.Exit, ExitThirdSlot);

        _unitName = GetUI<TMP_Text>("InventUnitNameTxt");
        _unitDescription = GetUI<TMP_Text>("InventUnitDescriTxt");

        _unitImg = GetUI<Image>("InventUnitImg");

        _myItemsScrollView = GetUI<ScrollRect>("MyItems_Scroll View");

        _myItemsContent = GetUI<Transform>("MyItems_Content");

        // 아이템 (유닛 능력 올려주는, '서브아이템'이라고 하겠음.)
        // 1. 게임매니저에 구매한 서브아이템 담을 list 만들어서 연결하기
        // 2. MyItems_Content 자식으로 List.Count 만큼 생성하기

        List<Item> playerSubItems = Main.Get<GameManager>().PlayerItems;
        foreach (Transform item in _myItemsContent.transform) // 초기화 관련 ?
        {
            Destroy(item.gameObject);
        }

        for (int i = 0; i < playerSubItems.Count; i++)
        {
            inventSubItems.Add(Main.Get<UIManager>().CreateSubitem<MyItemsImgBtnUI>("MyItemsImgBtnUI", _myItemsContent));
            inventSubItems[i].ItemData = playerSubItems[i];
            inventSubItems[i].Owner = this;
            inventSubItems[i].ItemData.ItemIndex = i;
        }
        SetInfo();
    }
    
    
    public void SetInfo()
    {
        _unitName.text = UnitData.Data.Key;
        _unitImg.sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.UNIT_SPRITE_PATH}{UnitData.Data.Key}");

        for (int i = 0; i < _equipSlots.Length; i++)
        {
            if (UnitData.Item[i] != null)
            {
                _equipSlotsImgs[i].sprite = Main.Get<ResourceManager>()
                    .Load<Sprite>($"{Literals.ITEM_SPRITE_PATH}{UnitData.Item[i].EquipItemData.Key}");
                _equipSlotsImgs[i].enabled = true; // image 컴포넌트 체크 설정.
            }
            else if (UnitData.Item[i] == null)
            {
                _equipSlotsImgs[i].sprite = null;
                _equipSlotsImgs[i].enabled = false; // image 컴포넌트 체크 해제.
            }
        }

        _unitDescription.text =
            $"Hp : {UnitData.Status[EstatType.Hp].Value}({UnitData.Status[EstatType.Hp].Value - UnitData.Data.Hp})\n" +
            $"Damage : {UnitData.Status[EstatType.Damage].Value}({UnitData.Status[EstatType.Damage].Value - UnitData.Data.Damage})\n" +
            $"Defense : {UnitData.Status[EstatType.Defense].Value}({UnitData.Status[EstatType.Defense].Value - UnitData.Data.Defense})\n" +
            $"ATK Speed : {UnitData.Status[EstatType.AttackSpeed].Value}({UnitData.Status[EstatType.AttackSpeed].Value - UnitData.Data.AttackSpeed})";
    }

    private void ClickInventUnitCloseBtn(PointerEventData EventData)
    {
        Main.Get<UIManager>().ClosePopup();
        Owner._selectCheckImg.gameObject.SetActive(false);

        //Owner.isUnitContentPressed = false;

    }

    private void ClickInventUnitDeleteBtn(PointerEventData EventData)
    {
        Main.Get<GameManager>().playerUnits.Remove(UnitData);
        Main.Get<UIManager>().ClosePopup(); // 설명창 닫아주고

        Owner.Owner.SetUnitInventory();// 인벤토리 리프레쉬

    }

    private void HoveredFirstSlot(PointerEventData EventData)
    {
        _equipCancelImgs[0].gameObject.SetActive(true);
    }
    private void ExitFirstSlot(PointerEventData EventData)
    {
        _equipCancelImgs[0].gameObject.SetActive(false);
    }

    private void HoveredSecondSlot(PointerEventData EventData)
    {
        _equipCancelImgs[1].gameObject.SetActive(true);
    }
    private void ExitSecondSlot(PointerEventData EventData)
    {
        _equipCancelImgs[1].gameObject.SetActive(false);
    }

    private void HoveredThirdSlot(PointerEventData EventData)
    {
        _equipCancelImgs[2].gameObject.SetActive(true);
    }
    private void ExitThirdSlot(PointerEventData EventData)
    {
        _equipCancelImgs[2].gameObject.SetActive(false);
    }

    // TODO : 아 매우 불편한 함수 구조... 이건 언젠가 바꾸고 만다 중간발표 이후에 ㅋㅋ 버튼3개
    private void ClickFirstSlot(PointerEventData EventData)
    {
        SlotUnEquip(0);
    }

    private void ClickSecondSlot(PointerEventData EventData)
    {
        SlotUnEquip(1);
    }

    private void ClickThirdSlot(PointerEventData EventData)
    {
        SlotUnEquip(2);
    }

    private void SlotUnEquip(int i)
    {
        if (UnitData.Item[i] == null) return;
        UnitData.Item[i].UnEquipItem(UnitData); //능력치를 빼고
        UnitData.Item[i] = null; //캐릭터의 아이템도 빼버리고
        _equipSlotsImgs[i].sprite = null; //이미지도 빼버리고
        _equipSlotsImgs[i].enabled = false; // image 컴포넌트 체크 해제.
        SetInfo();
        inventSubItems[UnitData.itemnumbers[i]].SetInfo();
        
    }

    public void ItemEquip(MyItemsImgBtnUI Imagedata)
    {
        for (int i = 0; i < _equipSlots.Length; i++)
        {
            if (UnitData.Item[i] == null)
            {
                _equipSlotsImgs[i].sprite = Main.Get<ResourceManager>()
                    .Load<Sprite>($"{Literals.ITEM_SPRITE_PATH}{Imagedata.ItemData.EquipItemData.Key}");
                UnitData.Item[i] = Imagedata.ItemData;
                UnitData.Item[i].EquipItem(UnitData);
                UnitData.itemnumbers[i] = Imagedata.ItemData.ItemIndex;
                break;
            }
            else
            {
                _equipSlotsImgs[i].sprite = Main.Get<ResourceManager>()
                    .Load<Sprite>($"{Literals.ITEM_SPRITE_PATH}{UnitData.Item[i].EquipItemData.Key}");
            }
        }
        SetInfo();
    }

    private void OnDestroy()
    {
        Owner.Owner.inventUnitDescri_PopupUI = null; // null 처리 !
    }
}
