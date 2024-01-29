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

    // item data 추가하기

    protected override void Init()
    {
        SetUI<Image>();
        SetUI<Button>();

        _itemImg = GetUI<Image>("MyItemsImgBtnUI");
        _itemImgBtn = GetUI<Button>("MyItemsImgBtnUI");
        _equipCheckImg = GetUI<Image>("ItemEquipCheckImg");

        SetUICallback(_itemImgBtn.gameObject, EUIEventState.Click, ClickUItemImgBtn);

        SetInfo();
    }

    private void SetInfo()
    {

    }

    private void ClickUItemImgBtn(PointerEventData data)
    {
        // 슬롯에 장착하기
        _equipCheckImg.gameObject.SetActive(true);
    }

}
