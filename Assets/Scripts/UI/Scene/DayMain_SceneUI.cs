using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DayMain_SceneUI : BaseUI
{
    #region Field
    private StageManager stageManager;
    private TileManager tileManager;

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
    private Button _speed1Button;
    private Button _speed2Button;

    private TextMeshProUGUI _playerMoneyText;
    private TextMeshProUGUI _stageText;

    private Slider _hpProgressbar;

    private Animator _dayImageAnimator;

    private List<RectTransform> _downMoveUIList = new List<RectTransform>();
    private List<RectTransform> _upMoveUIList = new List<RectTransform>();
    private RectTransform _categoryTransform;
    private RectTransform _placingPanelTransform;
    private RectTransform _backBtnTransform;
    private RectTransform _nightTransform;

    private PocketBlock_PopupUI _pocketBlock;

    private Stack<Action> _btnActions = new Stack<Action>();

    public CameraMovement maincamera;
    public bool isInventOpen { get; set; }

    #endregion

    protected override void Init()
    {
        base.Init();

        SetButton();
        SetImage();
        SetText();
        SetOtherItems();
        SetMoveUI();

        stageManager = Main.Get<StageManager>();
        tileManager = Main.Get<TileManager>();

        maincamera = Camera.main.GetComponent<CameraMovement>();

        _gameManager.OnChangeMoney += UpdateMoneyText;
        _gameManager.PlayerHP.OnCurValueChanged += UpdateHpUI;

        stageManager.OnStageStartEvent += ClickStart;
        stageManager.OnStageClearEvent += ClickStart;
        stageManager.OnStageClearEvent += UpdateDayCount;
    }

    #region UiBind

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
        _speed1Button = GetUI<Button>("PlayButton");
        _speed2Button = GetUI<Button>("X2SpeedButton");

        SetUICallback(_shopButton.gameObject, EUIEventState.Click, ClickShopBtn);
        SetUICallback(_inventoryButton.gameObject, EUIEventState.Click, ClickInventoryBtn);
        SetUICallback(_stageStartButton.gameObject, EUIEventState.Click, ClickStageStartBtn);
        SetUICallback(_placingButton.gameObject, EUIEventState.Click, ClickPlacingBtn);
        SetUICallback(_backButton.gameObject, EUIEventState.Click, ClickBackBtn);
        SetUICallback(_unitButton.gameObject, EUIEventState.Click, ClickUnitBtn);
        SetUICallback(_roomButton.gameObject, EUIEventState.Click, ClickRoomBtn);
        SetUICallback(_speed1Button.gameObject, EUIEventState.Click, ClickSpeed1Btn);
        SetUICallback(_speed2Button.gameObject, EUIEventState.Click, ClickSpeed2Btn);
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
        _nightTransform = _speed1Button.transform.parent.GetComponent<RectTransform>();
    }

    private void SetOtherItems()
    {
        SetUI<Slider>();
        SetUI<Animator>();

        _hpProgressbar = GetUI<Slider>("HP_Slider");
        _dayImageAnimator = GetUI<Animator>("CurStageTimeImg");
    }

    #endregion

    #region UIUpdateMethod

    private void UpdateHpUI(float hp)
    {
        _hpProgressbar.value = _gameManager.PlayerHP.Normalized();
    }

    private void UpdateMoneyText(int amount)
    {
        _playerMoneyText.text = amount.ToString();
    }

    private void UpdateDayCount(int stage)
    {
        _stageText.text = $"Day {stage}";
    }

    #endregion

    #region ButtonEvents

    private void ClickStageStartBtn(PointerEventData eventData)
    {
        Main.Get<StageManager>().StartStage();
    }

    private void ClickUnitBtn(PointerEventData eventData)
    {
        OpenPoketBlock(true);

        Action curAction = _btnActions.Pop();
        curAction -= tileManager.InactiveBatSlot;
        curAction += tileManager.InactiveBatSlot;
        _btnActions.Push(curAction);
    }

    private void ClickRoomBtn(PointerEventData eventData)
    {
        OpenPoketBlock(false);
    }

    private void ClickBackBtn(PointerEventData eventData)
    {
        if (_btnActions.Count >= 1)
            _btnActions.Pop().Invoke();
    }

    private void ClickPlacingBtn(PointerEventData eventData)
    {
        ClickPlacing();
        _btnActions.Push(ClickPlacing);
    }

    private void ClickShopBtn(PointerEventData eventData)
    {
        _ui.OpenPopup<Shop_PopupUI>("Shop_PopupUI");
    }

    private void ClickSpeed1Btn(PointerEventData eventData)
    {
        Time.timeScale = 1.5f;
        _speed1Button.gameObject.SetActive(false);
        _speed2Button.gameObject.SetActive(true);
    }

    private void ClickSpeed2Btn(PointerEventData eventData)
    {
        Time.timeScale = 1.0f;
        _speed1Button.gameObject.SetActive(true);
        _speed2Button.gameObject.SetActive(false);
    }

    #endregion

    #region ButtonClickMethod

    private void ClickStart(int stage)
    {
        if (_nightTransform.gameObject.activeSelf)
        {
            _dayImageAnimator.SetTrigger(Literals.StageEnd);
        }
        else
        {
            _dayImageAnimator.SetTrigger(Literals.StageStart);
        }

        DownMoveUI();
        NightUIMove();
    }

    private void ClickInventoryBtn(PointerEventData eventData)
    {
        if (isInventOpen) // 인벤토리 열려있으면 버튼 작동 안 하게끔
        {
            return;
        }
        else if (!isInventOpen)
        {
            Inventory_PopupUI ui = _ui.OpenPopup<Inventory_PopupUI>("Inventory_PopupUI");
            ui.Owner = this;
            isInventOpen = true;
        }
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
        }
    }

    public void TileBat()
    {
        SetTileBatUI();

        if (_btnActions.Peek() != SetTileBatUI)
            _btnActions.Push(SetTileBatUI);
    }

    public void SetTileBatUI()
    {
        if (_btnActions.Count <= 0)
            return;

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
        BackBtnMove();
        PlacingPanel();
    }

    #endregion

    #region UIDOTween

    private void UpMoveUI()
    {
        if (_upMoveUIList.Count == 0)
            return;

        if (_upMoveUIList[0].gameObject.activeSelf)
        {
            foreach (var rect in _upMoveUIList)
            {
                rect.DOAnchorPosY(rect.anchoredPosition.y + 240f, 0.3f).OnComplete(() => 
                {
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

        if (_downMoveUIList[0].gameObject.activeSelf)
        {
            foreach (var rect in _downMoveUIList)
            {
                rect.DOAnchorPosY(rect.anchoredPosition.y - 220f, 0.3f).OnComplete(() => 
                {
                    rect.gameObject.SetActive(false);
                });
            }
        }
        else
        {
            foreach (var rect in _downMoveUIList)
            {
                rect.gameObject.SetActive(true);
                rect.DOAnchorPosY(rect.anchoredPosition.y + 220f, 0.3f);
            }
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
            _placingPanelTransform.DOAnchorPosY(_placingPanelTransform.anchoredPosition.y - 220f, 0.3f).OnComplete(() => 
            {
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
            _categoryTransform.DOAnchorPosX(_categoryTransform.anchoredPosition.x + 200f, 0.4f).OnComplete(() => 
            {
                _categoryPanel.gameObject.SetActive(false);
            });
            Camera.main.DOOrthoSize(5f, 1.0f);
        }
    }

    private void BackBtnMove()
    {
        if (!_backBtnTransform.gameObject.activeSelf)
        {
            _backBtnTransform.gameObject.SetActive(true);
            _backBtnTransform.DOAnchorPosY(_backBtnTransform.anchoredPosition.y - 220f, 0.3f);
        }
        else
        {
            _backBtnTransform.DOAnchorPosY(_backBtnTransform.anchoredPosition.y + 220f, 0.3f).OnComplete( () =>
            {
                _backBtnTransform.gameObject.SetActive(false);
            });
        }
    }

    private void NightUIMove()
    {
        if (!_nightTransform.gameObject.activeSelf)
        {
            _nightTransform.gameObject.SetActive(true);
            _nightTransform.DOAnchorPosY(_nightTransform.anchoredPosition.y + 200f, 0.3f);
        }
        else
        {
            _nightTransform.DOAnchorPosY(_nightTransform.anchoredPosition.y - 200f, 0.3f).OnComplete(() =>
            {
                _nightTransform.gameObject.SetActive(false);
            });
        }
    }

    #endregion
}