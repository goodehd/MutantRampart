using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventUnit_ContentsBtnUI : BaseUI
{
    private Image _unitContentsImg;
    private Button _unitContentsBtn;
    private Image _equipCheckImg;

    public Character UnitData { get; set; }
    //public Inventory_PopupUI Owner { get; set; }

    protected override void Init()
    {
        SetUI<Image>();
        SetUI<Button>();

        _unitContentsImg = GetUI<Image>("InventUnit_ContentsBtnUI");
        _unitContentsBtn = GetUI<Button>("InventUnit_ContentsBtnUI");
        _equipCheckImg = GetUI<Image>("InventUnitEquipCheckImg");

        SetUICallback(_unitContentsBtn.gameObject, EUIEventState.Click, ClickUnitContentBtn);
        //SetUICallback(_unitContentsBtn.gameObject, EUIEventState.Hovered, HoveredUnitContentBtn);

        SetInfo();
    }

    private void SetInfo()
    {
        _unitContentsImg.sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.UNIT_SPRITE_PATH}{UnitData.Data.Key}");
        //_equipCheckImg = 

    }

    private void ClickUnitContentBtn(PointerEventData data)
    {
        // 버튼 누를때마다 창이 여러개 뜬다 !
        Main.Get<UIManager>().OpenPopup<InventUnitDescri_PopupUI>("InventUnitDescri_PopupUI").UnitData = UnitData;
        _equipCheckImg.gameObject.SetActive(true);

    }

    //private void HoveredUnitContentBtn(PointerEventData data)
    //{
    //    _equipCheckImg.gameObject.SetActive(true);
    //}
}
