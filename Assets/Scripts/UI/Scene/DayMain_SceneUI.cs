using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DayMain_SceneUI : BaseUI
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
        SetUICallback(_stageStartButton.gameObject, EUIEventState.Click, ClickStageStartBtn);

    }

    private void ClickShopBtn(PointerEventData eventData)
    {
        Main.Get<UIManager>().OpenPopup<Shop_PopupUI>("Shop_PopupUI");
    }

    private void ClickStageStartBtn(PointerEventData eventData)
    {
        Main.Get<StageManager>().StartStage();
    }

    // 이어서 placing, inventory, setting, stagestart Btn 함수 추가하기

}
