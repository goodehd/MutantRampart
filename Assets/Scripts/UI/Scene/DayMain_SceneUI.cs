using DG.Tweening;
using System;
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
    private Image _hpPanel;

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
    private RectTransform _backBtnTransform;

    private PocketBlock_PopupUI _pocketBlock;

    private Stack<Action> _btnActions = new Stack<Action>();

    protected override void Init()
    {
        base.Init();

        SetButton();
        SetImage();
        SetText();
        SetMoveUI();

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
        _hpPanel = GetUI<Image>("HPBlock");

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

        _upMoveUIList.Add(_stageImagePanel.GetComponent<RectTransform>());
        _upMoveUIList.Add(_playerMoneyImage.GetComponent<RectTransform>());
        _upMoveUIList.Add(_settingButton.GetComponent<RectTransform>());
        _upMoveUIList.Add(_hpPanel.GetComponent<RectTransform>());

        _categoryTransform = _categoryPanel.GetComponent<RectTransform>();
        _placingPanelTransform = _placingPanel.GetComponent<RectTransform>();
        _backBtnTransform = _backButton.GetComponent<RectTransform>();
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
        if(_btnActions.Count >= 1)
            _btnActions.Pop().Invoke();
    }

    private void ClickPlacingBtn(PointerEventData eventData)
    {
        ClickPlacing();
        _btnActions.Push(ClickPlacing);
    }

    public void TileBat()
    {
        SetTileBatUI();

        if (_btnActions.Peek() != SetTileBatUI)
            _btnActions.Push(SetTileBatUI);
    }

    public void SetTileBatUI()
    {
        if (_btnActions.Peek() == SetTileBatUI)
            return;

        ActiveCategory();
        PlacingPanel();
        _ui.CloseAllPopup();
    }

    private void ClickPlacing()
    {
        _backPanel.gameObject.SetActive(!_backPanel.gameObject.activeSelf);
        UpMoveUI();
        DownMoveUI();
        PlacingPanel();
    }

    #region UIDOTween

    private void UpMoveUI()
    {
        if (_upMoveUIList.Count == 0)
            return;

        if (_upMoveUIList[0].gameObject.activeSelf)
        {
            foreach (var rect in _upMoveUIList)
            {
                rect.DOAnchorPosY(rect.anchoredPosition.y + 240f, 0.3f).OnComplete(() => {
                    rect.gameObject.SetActive(false);
                });
            }
        }
        else
        {
            foreach (var rect in _upMoveUIList)
            {
                rect.gameObject.SetActive(true);
                rect.DOAnchorPosY(rect.anchoredPosition.y - 240f, 0.3f);
            }
        }
    }

    private void DownMoveUI()
    {
        if (_downMoveUIList.Count == 0)
            return;

        if(_downMoveUIList[0].gameObject.activeSelf)
        {
            foreach (var rect in _downMoveUIList)
            {
                rect.DOAnchorPosY(rect.anchoredPosition.y - 220f, 0.3f).OnComplete(() => {
                    rect.gameObject.SetActive(false);
                }); ;
            }
            _backBtnTransform.DOAnchorPosY(_backBtnTransform.anchoredPosition.y - 220f, 0.3f);
        }
        else
        {
            foreach (var rect in _downMoveUIList)
            {
                rect.gameObject.SetActive(true);
                rect.DOAnchorPosY(rect.anchoredPosition.y + 220f, 0.3f);
            }
            _backBtnTransform.DOAnchorPosY(_backBtnTransform.anchoredPosition.y + 220f, 0.3f);
        }
    }

    private void PlacingPanel()
    {
        if (!_placingPanel.gameObject.activeSelf)
        {
            _placingPanel.gameObject.SetActive(true);
            _placingPanelTransform.DOAnchorPosY(_placingPanelTransform.anchoredPosition.y + 220f, 0.3f);
        }
        else
        {
            _placingPanelTransform.DOAnchorPosY(_placingPanelTransform.anchoredPosition.y - 220f, 0.3f).OnComplete(() => {
                _placingPanelTransform.gameObject.SetActive(false);
            });
        }
    }

    private void ActiveCategory()
    {
        if (!_categoryPanel.gameObject.activeSelf)
        {
            _categoryPanel.gameObject.SetActive(true);
            _categoryTransform.DOAnchorPosX(_categoryTransform.anchoredPosition.x - 200f, 0.4f);
            Camera.main.DOOrthoSize(2.5f, 0.5f);
        }
        else
        {
            _categoryTransform.DOAnchorPosX(_categoryTransform.anchoredPosition.x + 200f, 0.4f).OnComplete(() => {
                _categoryPanel.gameObject.SetActive(false);
            });
            Camera.main.DOOrthoSize(5f, 1.0f);
        }
    }
    #endregion
}