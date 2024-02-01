using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDescript_PopupUI : BaseUI
{
    
    private Image itemDescriptPopup;
    private TMP_Text _itemName;
    private TMP_Text _itemDescript;
    private Button _descriptPopupCloseBtn;

    public CharacterData ShopUnitData { get; set; }
    public RoomData ShopRoomData { get; set; }
    
    // TODO : 아이템데이터도 추가해야함.

    protected override void Init()
    {
        SetUI<Image>();
        SetUI<TMP_Text>();
        SetUI<Button>();

        // 아이템 설명 팝업창
        itemDescriptPopup = GetUI<Image>("ItemDescriptPopupImg");
        _itemName = GetUI<TMP_Text>("ItemNameTxt");
        _itemDescript = GetUI<TMP_Text>("ItemDescriptTxt");
        _descriptPopupCloseBtn = GetUI<Button>("DescriptPopupCloseBtn");
        SetUICallback(_descriptPopupCloseBtn.gameObject, EUIEventState.Click, ClickPopupCloseBtn);

        if (ShopUnitData != null) // 구분 - 설명팝업 데이터가 유닛 일 때
        {
            SetUnitItemPopupInfo(ShopUnitData);
        }
        else // 구분 - 설명팝업 데이터가 Room 일 때
        {
            SetRoomItemPopupInfo(ShopRoomData);
        }
    }

    private void SetUnitItemPopupInfo(CharacterData characterData)
    {
        // 아이템 설명 팝업에 뜨는 정보
        _itemName.text = characterData.Key;
        _itemDescript.text =
            $"Hp : {characterData.Hp.ToString()}\nDamage : {characterData.Damage.ToString()}\nDefense : {characterData.Defense.ToString()}\nAttackSpeed : {characterData.AttackSpeed.ToString()}";
    }

    private void SetRoomItemPopupInfo(RoomData roomData)
    {
        // 아이템 설명 팝업에 뜨는 정보
        _itemName.text = roomData.Key;
        _itemDescript.text =
            $"설명 :\n{roomData.Instruction.ToString()}";
    }

    private void ClickPopupCloseBtn(PointerEventData EventData)
    {
        Main.Get<UIManager>().ClosePopup();
    }
}
