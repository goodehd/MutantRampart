using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyItemsImgBtnUI : BaseUI
{
    private GameManager gameManager;
    private StageManager stageManager;

    private Image _itemImg;
    private Button _itemImgBtn;
    private Image _equipCheckImg;
    private bool _isIEquiped;
    public Item ItemData { get; set; }
    public InventUnitDescri_PopupUI Owner { get; set; }


    private InventItemDetailBox _descriptPopupUI;

    protected override void Init()
    {
        SetUI<Image>();
        SetUI<Button>();

        gameManager = Main.Get<GameManager>();
        stageManager = Main.Get<StageManager>();

        _itemImg = GetUI<Image>("MyItemsImgBtnUI");
        _itemImgBtn = GetUI<Button>("MyItemsImgBtnUI");
        _equipCheckImg = GetUI<Image>("ItemEquipCheckImg");

        SetUICallback(_itemImgBtn.gameObject, EUIEventState.Click, ClickUItemImgBtn);
        SetUICallback(_itemImgBtn.gameObject, EUIEventState.Hovered, HoveredUnitContentBtn);
        SetUICallback(_itemImgBtn.gameObject, EUIEventState.Exit, ExitUnitContentBtn);

        SetInfo();
    }

    public void UnEquiped()
    {
        ItemData.IsEquiped = false;
    }

    public void SetInfo()
    {
        _itemImg.sprite = Main.Get<ResourceManager>()
            .Load<Sprite>($"{Literals.ITEM_SPRITE_PATH}{ItemData.EquipItemData.Key}");
        if (ItemData == null)
        {
            _isIEquiped = false;
        }
        else
        {
            _isIEquiped = ItemData.IsEquiped;
        }
        _equipCheckImg.gameObject.SetActive(_isIEquiped);
    }

    private void ClickUItemImgBtn(PointerEventData data)
    {
        if (stageManager.GetIsStageStart())
        {
            return;
        }

        if (_isIEquiped)
        {
            if (ItemData.Owner == Owner.UnitData)
            {
                Owner.SlotUnEquip(ItemData.SlotIndex);
            }
            else
            {
                UnEquipItem();
            }
            _equipCheckImg.gameObject.SetActive(false);
            _isIEquiped = false;
            return;
        }
        else if (!Owner.ItemEquip(this))
        {
            return;
        }

        _equipCheckImg.gameObject.SetActive(true);
        _isIEquiped = true;

        if (gameManager.isTutorial) // 튜토리얼 중이라면
        {
            if (Owner.tweener.IsActive())
            {
                Owner.tweener.Kill(); // 아이템 강조하던 화살표 kill
            }
            Owner.arrowImg.gameObject.SetActive(false); // 아이템 강조하던 화살표 inactive.

            TutorialMsg_PopupUI ui = Main.Get<UIManager>().OpenPopup<TutorialMsg_PopupUI>();
            ui.curTutorialText = Main.Get<DataManager>().Tutorial["T7"].Description;
            ui.isBackgroundActive = true;
            ui.isCloseBtnActive = true;

            if (gameManager.PlayerItems[0].IsEquiped) // 유닛에 아이템 장착이 되었다면
            {
                Owner.Owner.Owner.closeButton.gameObject.SetActive(true); // 인벤토리 닫기 버튼 활성화
                Owner.Owner.Owner.inventArrowImg.gameObject.SetActive(true);
                Owner.Owner.Owner.inventArrowTransform.anchoredPosition = new Vector3(662f, -400f, 0f); // 인벤토리 닫기 버튼 가리키는 화살표.
                Owner.Owner.Owner.inventArrowTransform.Rotate(0f, 0f, 180f);
                Owner.Owner.Owner.tweener = Owner.Owner.Owner.inventArrowTransform.DOAnchorPosY(-430f, Owner.animationDuration).SetLoops(-1, LoopType.Yoyo);
            }
        }
    }

    private void UnEquipItem()
    {
        ItemData.Owner.Item[ItemData.SlotIndex] = null;
        ItemData.UnEquipItem(ItemData.Owner);
    }

    private void HoveredUnitContentBtn(PointerEventData data)
    {
        _itemImg.color = Color.cyan;
        if (_descriptPopupUI != null) return;
        _descriptPopupUI = Main.Get<UIManager>().CreateSubitem<InventItemDetailBox>("InventItemDetailBox");
        _descriptPopupUI.HoveredItemData = ItemData.EquipItemData;
    }

    private void ExitUnitContentBtn(PointerEventData data)
    {
        _itemImg.color = Color.white;
        if (_descriptPopupUI == null) return;
        Main.Get<UIManager>().DestroySubItem(_descriptPopupUI.gameObject);
    }
}
