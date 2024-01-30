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
    private Button _unitButton;
    private Button _roomButton;

    private TextMeshProUGUI _playerMoneyText;
    private TextMeshProUGUI _stageText;

    private List<RectTransform> _downMoveUIList = new List<RectTransform>();
    private List<RectTransform> _upMoveUIList = new List<RectTransform>();
    private RectTransform _categoryTransform;
    private RectTransform _placingPanelTransform;

    private PocketBlock_PopupUI _pocketBlock;

    public CameraMovement maincamera;

    protected override void Init()
    {
        base.Init();

        SetButton();
        SetImage();
        SetText();
        SetMoveUI();
        maincamera = Camera.main.GetComponent<CameraMovement>();

        Main.Get<GameManager>().OnChangeMoney += UpdateMoneyText;
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
        _unitButton = GetUI<Button>("UnitBtn");
        _roomButton = GetUI<Button>("RoomBtn");

        SetUICallback(_shopButton.gameObject, EUIEventState.Click, ClickShopBtn);
        SetUICallback(_inventoryButton.gameObject, EUIEventState.Click, ClickInventoryBtn);
        SetUICallback(_stageStartButton.gameObject, EUIEventState.Click, ClickStageStartBtn);
        SetUICallback(_placingButton.gameObject, EUIEventState.Click, ClickPlacingBtn);
        SetUICallback(_backButton.gameObject, EUIEventState.Click, ClickBackBtn);
        SetUICallback(_unitButton.gameObject, EUIEventState.Click, ClickUnitBtn);
        SetUICallback(_roomButton.gameObject, EUIEventState.Click, ClickRoomBtn);
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

        _playerMoneyText.text = Main.Get<GameManager>()._playerMoney.ToString();
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

    private void UpdateMoneyText(int amount)
    {
        _playerMoneyText.text = amount.ToString();
    }

    private void ClickShopBtn(PointerEventData eventData)
    {
        _ui.OpenPopup<Shop_PopupUI>("Shop_PopupUI");
    }

    private void ClickInventoryBtn(PointerEventData eventData)
    {
        _ui.OpenPopup<Inventory_PopupUI>("Inventory_PopupUI");
    }

    private void ClickStageStartBtn(PointerEventData eventData)
    {
        if(!Main.Get<GameManager>().isHomeSet)return; //아 이걸 어따놓지
        Main.Get<StageManager>().StartStage();
    }

    private void ClickUnitBtn(PointerEventData eventData)
    {
        OpenPoketBlock(true);
    }

    private void ClickRoomBtn(PointerEventData eventData)
    {
        OpenPoketBlock(false);
    }

    private void OpenPoketBlock(bool isUint)
    {
        if (_pocketBlock == null)
        {
            _pocketBlock = _ui.OpenPopup<PocketBlock_PopupUI>();
            _pocketBlock.IsUnit = isUint;
        }
        else if (_pocketBlock.IsUnit != isUint)
        {
            _pocketBlock.ToggleContents(isUint);
            _pocketBlock.IsUnit = isUint;
        }
        else
        {
            _ui.ClosePopup();
            _pocketBlock = null;
            Main.Get<TileManager>().BatSlot.SetActive(false);
        }
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
            _categoryTransform.DOAnchorPosX(_categoryTransform.anchoredPosition.x + 200f, 0.5f).OnComplete(() =>
            {
                _categoryPanel.gameObject.SetActive(false);
            });
        }

        if (_placingPanel.gameObject.activeSelf)
        {
            _placingPanelTransform.DOAnchorPosY(_placingPanelTransform.anchoredPosition.y - 220f, 0.5f)
                .OnComplete(() => { _placingPanelTransform.gameObject.SetActive(false); });
        }
        
        _ui.CloseAllPopup();
        Main.Get<TileManager>().BatSlot.SetActive(false);
        Main.Get<TileManager>().SelectRoom = null;

        Camera.main.DOOrthoSize(5f, 1.0f);
        maincamera.isOnPlacingPanel = false;
    }

    private void ClickPlacingBtn(PointerEventData eventData)
    {
        _backPanel.gameObject.SetActive(false);

        foreach (var rect in _downMoveUIList)
        {
            rect.DOAnchorPosY(rect.anchoredPosition.y - 220f, 0.5f);
        }

        foreach (var rect in _upMoveUIList)
        {
            rect.DOAnchorPosY(rect.anchoredPosition.y + 200f, 0.5f);
        }

        if (!_placingPanel.gameObject.activeSelf)
        {
            _placingPanel.gameObject.SetActive(true);
            maincamera.isOnPlacingPanel = true;
            _placingPanelTransform.DOAnchorPosY(_placingPanelTransform.anchoredPosition.y + 220f, 0.5f);
        }
    }

    private void ClickPlacingBtn2(PointerEventData eventData)
    {
        _backPanel.gameObject.SetActive(false);

        if (!_placingPanel.gameObject.activeSelf)
        {
            _placingPanel.gameObject.SetActive(true);
            maincamera.isOnPlacingPanel = true;
            _placingPanelTransform.DOAnchorPosY(_placingPanelTransform.anchoredPosition.y + 220f, 0.5f);
        }
    }

    public void ActiveCategory()
    {
        if (!_categoryPanel.gameObject.activeSelf)
        {
            _categoryPanel.gameObject.SetActive(true);

            _placingPanelTransform.DOAnchorPosY(_placingPanelTransform.anchoredPosition.y - 220f, 0.5f).OnComplete(() =>
            {
                _placingPanelTransform.gameObject.SetActive(false);
            });
            _categoryTransform.DOAnchorPosX(_categoryTransform.anchoredPosition.x - 200f, 0.5f);
        }
    }
}