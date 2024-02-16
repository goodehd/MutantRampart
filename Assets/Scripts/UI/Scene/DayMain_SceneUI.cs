using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIState
{
    public Action BtnActions;
    public EUIstate UIStagte;

    public UIState(Action btnActions, EUIstate uIStagte)
    {
        BtnActions = btnActions;
        UIStagte = uIStagte;
    }
}

public class DayMain_SceneUI : BaseUI
{
    #region Field
    private StageManager stageManager;
    private TileManager tileManager;
    private GameManager gameManager;

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

    private Error_PopupUI _errorPopupUI;

    private PocketBlock_PopupUI _pocketBlock;

    public Inventory_PopupUI inventory_PopupUI;

    private Stack<UIState> _btnActions = new Stack<UIState>();

    public CameraMovement maincamera;
    public bool isInventOpen { get; set; } = false;
    public bool isUIAnimating { get; set; } = false;
    public float animationDuration = 0.3f;

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
        gameManager = Main.Get<GameManager>();

        maincamera = Camera.main.GetComponent<CameraMovement>();

        _gameManager.OnChangeMoney += UpdateMoneyText;
        _gameManager.PlayerHP.OnCurValueChanged += UpdateHpUI;

        stageManager.OnStageStartEvent += ClickStart;
        stageManager.OnStageClearEvent += ClickStart;
        stageManager.OnStageClearEvent += UpdateDayCount;

        tileManager.OnSlectRoomEvent += TileBat;
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
        SetUICallback(_settingButton.gameObject, EUIEventState.Click, ClickSettingBtn);
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

        _playerMoneyText.text = Main.Get<GameManager>().PlayerMoney.ToString();
        UpdateDayCount(Main.Get<GameManager>().CurStage);
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
        //stageManager.StartStage();

        if (gameManager.isHomeSet)
        {
            stageManager.StartStage();
        }
        else
        {
            Error_PopupUI ui = _ui.OpenPopup<Error_PopupUI>();
            ui.curErrorText = "홈타입의 방을 설치해 주세요.";
        }
    }

    private void ClickUnitBtn(PointerEventData eventData)
    {
        OpenPoketBlock(true);
    }

    private void ClickRoomBtn(PointerEventData eventData)
    {
        OpenPoketBlock(false);
    }

    private void ClickBackBtn(PointerEventData eventData)
    {
        if (_btnActions.Count >= 1 && !isUIAnimating)
        {
            _btnActions.Pop().BtnActions.Invoke();
        }
    }

    private void ClickPlacingBtn(PointerEventData eventData)
    {
        _ui.CloseAllPopup();

        ClickPlacing();
        _btnActions.Push(new UIState(ClickPlacing, EUIstate.Main));
    }

    private void ClickShopBtn(PointerEventData eventData)
    {
        _ui.CloseAllPopup();

        //_ui.OpenPopup<PastShop_PopupUI>("PastShop_PopupUI");
        maincamera.Rock = true;
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
        MovePosYUI(_downMoveUIList, -240f);
        MovePosYUI(_nightTransform, 200f);
    }

    private void ClickInventoryBtn(PointerEventData eventData)
    {
        maincamera.Rock = true;

        if (inventory_PopupUI == null) // 유니티 play 하고 Hierarchy 창을 클릭한 후에 게임Scene 에서 타일을 누르면 인벤토리가 없어지고, 인벤토리 버튼 누르면 인벤토리가 다시 안 뜨는 이슈해결을 위해.
        {
            isInventOpen = false;
        }

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

    private void ClickSettingBtn(PointerEventData eventData)
    {
        maincamera.Rock = true;
        Main.Get<UIManager>().OpenPopup<Setting_PopupUI>();
        Main.Get<GameManager>().SaveData();
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

    private void ClickPlacing()
    {
        if (isUIAnimating)
            return;

        StartCoroutine(ButtonRock());
        _backPanel.gameObject.SetActive(!_backPanel.gameObject.activeSelf);
        MovePosYUI(_upMoveUIList, 240f);
        MovePosYUI(_placingPanelTransform, 220f);
        MovePosYUI(_downMoveUIList, -240f);
        MovePosYUI(_backBtnTransform, -220f);
    }

    public void TileBat()
    {
        if (_btnActions.Count <= 0)
        {
            return;
        }

        if (isUIAnimating)
        {
            tileManager.SetSelectRoom(null);
            return;
        }

        maincamera.Rock = true;
        tileManager.SelectRoom.StartFlashing();
        tileManager.InactiveBatSlot();
        _ui.CloseAllPopup();
        FocusCamera();

        if (_btnActions.Peek().UIStagte != EUIstate.ChangeTileSelect)
        {
            _btnActions.Push(new UIState(SetTileBatUI, EUIstate.ChangeTileSelect));
            SetTileBatUI();
        }
    }

    public void SetTileBatUI()
    {
        if (isUIAnimating)
            return;

        StartCoroutine(ButtonRock());
        MovePosYUI(_placingPanelTransform, 220f);
        MovePosXUI(_categoryTransform, 200f);

        if (_btnActions.Peek().UIStagte != EUIstate.ChangeTileSelect)
        {
            tileManager.SelectRoom.StopFlashing();
            tileManager.InactiveBatSlot();
            tileManager.SetSelectRoom(null);
            _ui.CloseAllPopup();
            Camera.main.DOOrthoSize(5.0f, animationDuration);
            maincamera.Rock = false;
        }
    }

    #endregion

    #region UIDOTween

    private void MovePosYUI(RectTransform ui, float offset)
    {
        if (!ui.gameObject.activeSelf)
        {
            ui.gameObject.SetActive(true);
            ui.DOAnchorPosY(ui.anchoredPosition.y + offset, animationDuration);
        }
        else
        {
            ui.DOAnchorPosY(ui.anchoredPosition.y - offset, animationDuration).OnComplete(() =>
            {
                ui.gameObject.SetActive(false);
            });
        }
    }

    private void MovePosYUI(List<RectTransform> ui, float offset)
    {
        if (ui.Count == 0)
            return;

        if (ui[0].gameObject.activeSelf)
        {
            foreach (var rect in ui)
            {
                rect.DOAnchorPosY(rect.anchoredPosition.y + offset, animationDuration).OnComplete(() =>
                {
                    rect.gameObject.SetActive(false);
                });
            }
        }
        else
        {
            foreach (var rect in ui)
            {
                rect.gameObject.SetActive(true);
                rect.DOAnchorPosY(rect.anchoredPosition.y - offset, animationDuration);
            }
        }
    }

    private void MovePosXUI(RectTransform ui, float offset)
    {
        if (!ui.gameObject.activeSelf)
        {
            ui.gameObject.SetActive(true);
            ui.DOAnchorPosX(ui.anchoredPosition.x - offset, animationDuration);
        }
        else
        {
            ui.DOAnchorPosX(ui.anchoredPosition.x + offset, animationDuration).OnComplete(() =>
            {
                ui.gameObject.SetActive(false);
            });
        }
    }

    private void FocusCamera()
    {
        Vector3 pos = new Vector3(tileManager.SelectRoom.transform.position.x,
            tileManager.SelectRoom.transform.position.y + 1.8f, Camera.main.transform.position.z);
        Camera.main.transform.DOMove(pos, animationDuration);
        Camera.main.DOOrthoSize(2.5f, animationDuration);
    }

    private IEnumerator ButtonRock()
    {
        isUIAnimating = true;
        var sec = new WaitForSeconds(animationDuration);
        yield return sec;
        isUIAnimating = false;
    }
    #endregion
}