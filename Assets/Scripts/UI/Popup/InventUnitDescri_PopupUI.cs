using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventUnitDescri_PopupUI : BaseUI
{
    private GameManager gameManager;

    private Button _closeBtn;
    private Button _deleteBtn;
    private Button[] _equipSlots = new Button[3];

    //private Button _firstSlot;
    //private Button _secondSlot;
    //private Button _thirdSlot;

    private TMP_Text _unitName;
    private TMP_Text _unitDescription;
    private TMP_Text _unitSkillDescription;

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
    
    public InventUnit_ContentsBtnUI Owner { get; set; }


    protected override void Init()
    {
        base.Init();
        SetUI<Button>();
        SetUI<TMP_Text>();
        SetUI<Image>();
        SetUI<ScrollRect>();
        SetUI<Transform>();

        gameManager = Main.Get<GameManager>();

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
        _unitSkillDescription = GetUI<TMP_Text>("InventUnitSkillDescriTxt");

        _unitImg = GetUI<Image>("InventUnitImg");

        _myItemsScrollView = GetUI<ScrollRect>("MyItems_Scroll View");

        _myItemsContent = GetUI<Transform>("MyItems_Content");

        List<Item> playerSubItems = gameManager.PlayerItems;
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

        if (_tutorialManager.isTutorial) // 튜토리얼 진행 중일 때 삭제버튼, 닫기버튼 비활성화.
        {
            _deleteBtn.gameObject.SetActive(false);
            _closeBtn.gameObject.SetActive(false);

            _tutorialManager.SetArrowPosition(48f, 80f); // Description 에서 아이템 가리키는 화살표
            _tutorialManager.SetDOTweenY(110f);
        }
    }
    
    
    public void SetInfo()
    {
        _unitName.text = UnitData.Data.PrefabName;
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
            $"Hp : {string.Format("{0:0.#}", UnitData.Status[EstatType.Hp].Value)}({string.Format("{0:0.#}", UnitData.Status[EstatType.Hp].Value - UnitData.Data.Hp)})\n" +
            $"Damage : {string.Format("{0:0.#}", UnitData.Status[EstatType.Damage].Value)}({string.Format("{0:0.#}", UnitData.Status[EstatType.Damage].Value - UnitData.Data.Damage)})\n" +
            $"Defense : {string.Format("{0:0.#}", UnitData.Status[EstatType.Defense].Value)}({string.Format("{0:0.#}", UnitData.Status[EstatType.Defense].Value - UnitData.Data.Defense)})\n" +
            $"ATK Speed : {string.Format("{0:0.#}", UnitData.Status[EstatType.AttackSpeed].Value)}({string.Format("{0:0.#}",UnitData.Status[EstatType.AttackSpeed].Value - UnitData.Data.AttackSpeed)})";

        if(UnitData.SkillData != null)
        {
            _unitSkillDescription.text = $"Skill : {UnitData.SkillData.Description}";
        }
        else
        {
            _unitSkillDescription.text = "Skill : None";
        }
    }

    private void ClickInventUnitCloseBtn(PointerEventData EventData)
    {
        if(Owner != null)
        {
            Owner._selectCheckImg.gameObject.SetActive(false);
            _ui.ClosePopup();
        }

        if (Owner == null)
        {
            ((DayMain_SceneUI)_ui.SceneUI).ReMoveUnitUI();
        }
    }

    private void ClickInventUnitDeleteBtn(PointerEventData EventData)
    {
        if (Main.Get<StageManager>().GetIsStageStart())
        {
            return;
        }

        Sell_PopupUI sell_popupui = _ui.OpenPopup<Sell_PopupUI>();
        sell_popupui.ShopUnitData = UnitData;

        if(Owner != null)
        {
            sell_popupui.Owner = Owner.Owner;
        }
    }

    private void HoveredFirstSlot(PointerEventData EventData)
    {
        if (_tutorialManager.isTutorial) return;
        _equipCancelImgs[0].gameObject.SetActive(true);
    }
    private void ExitFirstSlot(PointerEventData EventData)
    {
        if (_tutorialManager.isTutorial) return;
        _equipCancelImgs[0].gameObject.SetActive(false);
    }

    private void HoveredSecondSlot(PointerEventData EventData)
    {
        if (_tutorialManager.isTutorial) return;
        _equipCancelImgs[1].gameObject.SetActive(true);
    }
    private void ExitSecondSlot(PointerEventData EventData)
    {
        if (_tutorialManager.isTutorial) return;
        _equipCancelImgs[1].gameObject.SetActive(false);
    }

    private void HoveredThirdSlot(PointerEventData EventData)
    {
        if (_tutorialManager.isTutorial) return;
        _equipCancelImgs[2].gameObject.SetActive(true);
    }
    private void ExitThirdSlot(PointerEventData EventData)
    {
        if (_tutorialManager.isTutorial) return;
        _equipCancelImgs[2].gameObject.SetActive(false);
    }

    // TODO : 아 매우 불편한 함수 구조... 이건 언젠가 바꾸고 만다 중간발표 이후에 ㅋㅋ 버튼3개
    private void ClickFirstSlot(PointerEventData EventData)
    {
        if (_tutorialManager.isTutorial) return;
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

    public void SlotUnEquip(int i)
    {
        if (UnitData.Item[i] == null) return;
        UnitData.Item[i].UnEquipItem(UnitData); //능력치를 빼고
        UnitData.Item[i] = null; //캐릭터의 아이템도 빼버리고
        _equipSlotsImgs[i].sprite = null; //이미지도 빼버리고
        _equipSlotsImgs[i].enabled = false; // image 컴포넌트 체크 해제.
        SetInfo();
        inventSubItems[UnitData.itemnumbers[i]].UnEquiped();
        inventSubItems[UnitData.itemnumbers[i]].SetInfo();
    }

    public bool ItemEquip(MyItemsImgBtnUI Imagedata)
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
                Imagedata.ItemData.SlotIndex = i;
                break;
            }

            if (i >= 2)
            {
                return false;
            }
        }
        SetInfo();
        return true;
    }

    private void OnDestroy()
    {
        if(Owner != null)
        {
            Owner.Owner.inventUnitDescri_PopupUI = null;
        }
    }
}
