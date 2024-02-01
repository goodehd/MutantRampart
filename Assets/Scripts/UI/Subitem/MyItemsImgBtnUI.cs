using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyItemsImgBtnUI : BaseUI
{
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

        _itemImg = GetUI<Image>("MyItemsImgBtnUI");
        _itemImgBtn = GetUI<Button>("MyItemsImgBtnUI");
        _equipCheckImg = GetUI<Image>("ItemEquipCheckImg");

        SetUICallback(_itemImgBtn.gameObject, EUIEventState.Click, ClickUItemImgBtn);
        SetUICallback(_itemImgBtn.gameObject, EUIEventState.Hovered, HoveredUnitContentBtn);
        SetUICallback(_itemImgBtn.gameObject, EUIEventState.Exit, ExitUnitContentBtn);

        SetInfo();
    }

    private void SetInfo()
    {
        _itemImg.sprite = Main.Get<ResourceManager>()
            .Load<Sprite>($"{Literals.ITEM_SPRITE_PATH}{ItemData.EquipItemData.Key}");
        if(ItemData == null)return;
        _isIEquiped = ItemData.IsEquiped;
        _equipCheckImg.gameObject.SetActive(_isIEquiped);
    }

    private void ClickUItemImgBtn(PointerEventData data)
    {
        if(_isIEquiped)return;
        Owner.ItemEquip(ItemData);
        _equipCheckImg.gameObject.SetActive(true);
        _isIEquiped = true;
    }
    private void HoveredUnitContentBtn(PointerEventData data)
    {
        _itemImg.color = Color.cyan;
        if(_descriptPopupUI != null) return;
        _descriptPopupUI = Main.Get<UIManager>().CreateSubitem<InventItemDetailBox>("InventItemDetailBox");
        _descriptPopupUI.HoveredItemData = ItemData.EquipItemData;
    }

    private void ExitUnitContentBtn(PointerEventData data)
    {
        _itemImg.color = Color.white;
        if(_descriptPopupUI == null) return;
        Main.Get<UIManager>().DestroySubItem(_descriptPopupUI.gameObject);
    }
}
