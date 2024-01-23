using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DayMainUI : BaseUI
{
    private Button _shopButton;
    private Button _placingButton;
    private Button _inventoryButton;
    private Button _settingButton;
    private Button _stageStartButton;

    protected override void Init()
    {
        SetUI<Button>();

        _shopButton = GetUI<Button>("ShopButton");
        _placingButton = GetUI<Button>("PlacingButton");
        _inventoryButton = GetUI<Button>("InventoryButton");
        _settingButton = GetUI<Button>("SettingButton");
        _stageStartButton = GetUI<Button>("StageStartButton");

        SetUICallback(_shopButton.gameObject, EUIEventState.Click, ClickShopBtn);

    }

    
    private void ClickShopBtn(PointerEventData eventData)
    {
        Main.Get<UIManager>().OpenPopup<ShopUI>("Shop_PopupUI");
    }

    // 이어서 placing, inventory, setting, stagestart Btn 함수 추가하기

}
