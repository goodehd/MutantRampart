using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventItemDetailBox : BaseUI
{
    private TMP_Text _itemName;
    private TMP_Text _itemDescript;
    private Image _itemImage;
    public ItemData HoveredItemData { get; set; }

    protected override void Init()
    {
        SetUI<TMP_Text>();
        SetUI<Image>();
        
        _itemName = GetUI<TMP_Text>("InventItemNameTxt");
        _itemDescript = GetUI<TMP_Text>("InventItemDescriTxt");
        _itemImage = GetUI<Image>("InventItemDetailBox");
        
        if(HoveredItemData == null)return;
        SetItemPopupInfo(HoveredItemData);
        _itemImage.transform.position = Input.mousePosition + new Vector3(160,-160,0);
    }
    
    private void SetItemPopupInfo(ItemData itemData)
    {
        _itemName.text = itemData.Key;
        _itemDescript.text =
            $"Type : {itemData.Type.ToString()}\nHp : {itemData.HpAdd.ToString()}\nDamage : {itemData.AttackAdd.ToString()}\nDefense : {itemData.DefenseAdd.ToString()}\nAttackSpeed : {itemData.SpeedAdd.ToString("F2")}\n\n설명\n {itemData.Instruction}";
        
    }

    private void FixedUpdate()
    {
        _itemImage.transform.position = Input.mousePosition + new Vector3(160,-160,0);
    }
}
