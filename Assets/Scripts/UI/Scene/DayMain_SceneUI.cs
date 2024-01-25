using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DayMain_SceneUI : BaseUI
{
    private Button _shopButton;
    private Button _placingButton;
    private Button _inventoryButton;
    private Button _settingButton;
    private Button _stageStartButton;
    private Image _backPanel;
    private Image _placingBtnImage;

    protected override void Init()
    {
        SetUI<Button>();
        SetUI<Image>();

        _shopButton = GetUI<Button>("ShopButton");
        _placingButton = GetUI<Button>("PlacingButton");
        _inventoryButton = GetUI<Button>("InventoryButton");
        _settingButton = GetUI<Button>("SettingButton");
        _stageStartButton = GetUI<Button>("StageStartButton");
        _backPanel = GetUI<Image>("BackPanel");
        _placingBtnImage = GetUI<Image>("PlacingButton");
        
        SetUICallback(_shopButton.gameObject, EUIEventState.Click, ClickShopBtn);

        SetUICallback(_stageStartButton.gameObject, EUIEventState.Click, ClickStageStartBtn);
        SetUICallback(_placingButton.gameObject, EUIEventState.Click, ClickPlacingBtn);
    }

    private void ClickShopBtn(PointerEventData eventData)
    {
        Main.Get<UIManager>().OpenPopup<Shop_PopupUI>("Shop_PopupUI");
    }

    private void ClickStageStartBtn(PointerEventData eventData)
    {
        Main.Get<StageManager>().StartStage();
    }

    private void ClickPlacingBtn(PointerEventData eventData)
    {
        if (_backPanel.gameObject.activeSelf)
        {
            _backPanel.gameObject.SetActive(false);
            _placingBtnImage.color = Color.red;
        }
        else
        {
            _backPanel.gameObject.SetActive(true);
            _placingBtnImage.color = Color.white;
        }
    }
}