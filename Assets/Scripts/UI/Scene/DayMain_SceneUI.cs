using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DayMain_SceneUI : BaseUI
{
    private Image _backPanel;
    private Image _stageImagePanel;
    private Image _stageImage;
    private Image _playerMoneyImage;
    private Image _buttonsPanel;
    private Image _categoryPanel;
    private Image _placingPanel;

    private Button _shopButton;
    private Button _placingButton;
    private Button _inventoryButton;
    private Button _stageStartButton;
    private Button _settingButton;
    private Button _backButton;

    private TextMeshProUGUI _playerMoneyText;
    private TextMeshProUGUI _stageText;

    private List<RectTransform> _downMoveUIList = new List<RectTransform>();
    private List<RectTransform> _upMoveUIList = new List<RectTransform>();
    private RectTransform _categoryTransform;
    private RectTransform _placingPanelTransform;

    protected override void Init()
    {
        SetButton();
        SetImage();
        SetText();
        SetMoveUI();
    }

    private void SetButton()
    {
        SetUI<Button>();

        _shopButton = GetUI<Button>("ShopButton");
        _placingButton = GetUI<Button>("PlacingButton");
        _inventoryButton = GetUI<Button>("InventoryButton");
        _settingButton = GetUI<Button>("SettingButton");
        _stageStartButton = GetUI<Button>("StageStartButton");
        _backButton = GetUI<Button>("BackButton");

        SetUICallback(_shopButton.gameObject, EUIEventState.Click, ClickShopBtn);
        SetUICallback(_inventoryButton.gameObject, EUIEventState.Click, ClickInventoryBtn);
        SetUICallback(_stageStartButton.gameObject, EUIEventState.Click, ClickStageStartBtn);
        SetUICallback(_placingButton.gameObject, EUIEventState.Click, ClickPlacingBtn);
        SetUICallback(_backButton.gameObject, EUIEventState.Click, ClickBackBtn);
    }

    private void SetImage()
    {
        SetUI<Image>();

        _backPanel = GetUI<Image>("BackPanel");
        _playerMoneyImage = GetUI<Image>("MainPlayerMoneyImg");
        _stageImagePanel = GetUI<Image>("CurStage");
        _stageImage = GetUI<Image>("CurStageTimeImg");
        _buttonsPanel = GetUI<Image>("ButtonPosBlock");
        _categoryPanel = GetUI<Image>("CategoryBlock");
        _placingPanel = GetUI<Image>("PlacingPanel");

        _categoryPanel.gameObject.SetActive(false);
    }

    private void SetText()
    {
        SetUI<TextMeshProUGUI>();

        _playerMoneyText = GetUI<TextMeshProUGUI>("MainPlayerMoneyText");
        _stageText = GetUI<TextMeshProUGUI>("CurStageTxt");
    }

    private void SetMoveUI()
    {
        _downMoveUIList.Add(_buttonsPanel.GetComponent<RectTransform>());
        _downMoveUIList.Add(_stageStartButton.GetComponent<RectTransform>());
        _downMoveUIList.Add(_backButton.GetComponent<RectTransform>());

        _upMoveUIList.Add(_stageImagePanel.GetComponent<RectTransform>());
        _upMoveUIList.Add(_playerMoneyImage.GetComponent<RectTransform>());
        _upMoveUIList.Add(_settingButton.GetComponent<RectTransform>());

        _categoryTransform = _categoryPanel.GetComponent<RectTransform>();
        _placingPanelTransform = _placingPanel.GetComponent<RectTransform>();
    }

    private void ClickShopBtn(PointerEventData eventData)
    {
        Main.Get<UIManager>().OpenPopup<Shop_PopupUI>("Shop_PopupUI");
    }

    private void ClickInventoryBtn(PointerEventData eventData)
    {
        Main.Get<UIManager>().OpenPopup<Inventory_PopupUI>("Inventory_PopupUI");
    }

    private void ClickStageStartBtn(PointerEventData eventData)
    {
        Main.Get<StageManager>().StartStage();
    }

    private void ClickBackBtn(PointerEventData eventData)
    {
        _backPanel.gameObject.SetActive(true);

        foreach (var rect in _downMoveUIList)
        {
            rect.DOAnchorPosY(rect.anchoredPosition.y + 220f, 0.5f);
        }

        foreach (var rect in _upMoveUIList)
        {
            rect.DOAnchorPosY(rect.anchoredPosition.y - 200f, 0.5f);
        }

        if (_categoryPanel.gameObject.activeSelf)
        {
            _categoryTransform.DOAnchorPosX(_categoryTransform.anchoredPosition.x + 200f, 0.5f).OnComplete(() => {
                _categoryPanel.gameObject.SetActive(false);
            });
        }

        if (_placingPanel.gameObject.activeSelf)
        {
            _placingPanelTransform.DOAnchorPosY(_placingPanelTransform.anchoredPosition.y - 220f, 0.5f).OnComplete(() => {
                _placingPanelTransform.gameObject.SetActive(false);
            });
        }

        Camera.main.DOOrthoSize(5f, 1.0f);
    }

    private void ClickPlacingBtn(PointerEventData eventData)
    {
        _backPanel.gameObject.SetActive(false);

        foreach(var rect in _downMoveUIList)
        {
            rect.DOAnchorPosY(rect.anchoredPosition.y - 220f, 0.5f);
        }

        foreach (var rect in _upMoveUIList)
        {
            rect.DOAnchorPosY(rect.anchoredPosition.y + 200f, 0.5f);
        }

        if(!_placingPanel.gameObject.activeSelf)
        {
            _placingPanel.gameObject.SetActive(true);
            _placingPanelTransform.DOAnchorPosY(_placingPanelTransform.anchoredPosition.y + 220f, 0.5f);
        }
    }

    public void ActiveCategory()
    {
        if (!_categoryPanel.gameObject.activeSelf)
        {
            _categoryPanel.gameObject.SetActive(true);

            _placingPanelTransform.DOAnchorPosY(_placingPanelTransform.anchoredPosition.y - 220f, 0.5f).OnComplete(() => {
                _placingPanelTransform.gameObject.SetActive(false);
            });
            _categoryTransform.DOAnchorPosX(_categoryTransform.anchoredPosition.x - 200f, 0.5f);
        }
    }
}