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
    private Image _startYesNoPanel;
    public Image _categoryPanel { get; set; }
    public Image _placingPanel { get; set; }
    private Image _hpPanel;
    public Image _dayArrowImg { get; set; }


    public Button shopButton { get; set; }
    public Button placingButton { get; set; }
    private Button _inventoryButton;
    private Button _stageStartButton;
    private Button _stageStartYesButton;
    private Button _stageStartNoButton;
    private Button _settingButton;
    public Button backButton { get; set; }
    public Button unitButton { get; set; }
    public Button roomButton { get; set; }
    private Button _speed1Button;
    private Button _speed2Button;
    private Button _speed3Button;
    private Button _nextStageInfoButton;

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
    public RectTransform dayArrowTransform { get; set; }

    public RoomDirBtnsUI roomDirBtsnUI { get; set; }

    private Error_PopupUI _errorPopupUI;

    private PocketBlock_PopupUI _pocketBlock;

    private Stack<UIState> _btnActions = new Stack<UIState>();

    public Tweener tweener { get; set; }

    public Inventory_PopupUI inventory_PopupUI;

    public TutorialMsg_PopupUI tutorialMsg_PopupUI;

    public CameraMovement maincamera;

    public Shop_PopupUI shop_PopupUI { get; set; }
    private InventUnitDescri_PopupUI _clickUnitUI;

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
        stageManager.OnStageStartEvent += SetTimeScale;

        stageManager.OnStageClearEvent += ClickStart;
        stageManager.OnStageClearEvent += UpdateDayCount;

        tileManager.OnSlectRoomEvent += TileBat;

        if (gameManager.isTutorial) // 튜토리얼 중이라면
        {
            placingButton.gameObject.SetActive(false); // 배치모드 버튼 비활성화.
            unitButton.gameObject.SetActive(false); // 배치모드의 unit 버튼 비활성화.
            backButton.gameObject.SetActive(false); // 배치모드 뒤로가기 버튼 비활성화.
            _inventoryButton.gameObject.SetActive(false); // 인벤토리 버튼 비활성화.
            _stageStartButton.gameObject.SetActive(false); // Battle 버튼 비활성화.

            TutorialMsg_PopupUI ui = _ui.OpenPopup<TutorialMsg_PopupUI>();
            ui.curTutorialText = Main.Get<DataManager>().Tutorial["T0"].Description;
            _dayArrowImg.gameObject.SetActive(true);
            dayArrowTransform.anchoredPosition = new Vector3(-770f, -276f, 0f); // 상점 가리키는 화살표.
            tweener = dayArrowTransform.DOAnchorPosY(-306f, animationDuration).SetLoops(-1, LoopType.Yoyo);
        }
        roomDirBtsnUI = _ui.CreateSubitem<RoomDirBtnsUI>();
        roomDirBtsnUI.Owner = this;
        roomDirBtsnUI.gameObject.SetActive(false);
        UpdateHpUI(0);
    }

    public void CreateClickUnitUI(Character unit)
    {
        if(inventory_PopupUI != null)
        {
            if (inventory_PopupUI.GetCurRoomInven())
            {
                inventory_PopupUI.ClickUnitBtnAction();
            }

            foreach(Transform obj in inventory_PopupUI.inventUnitContent.transform)
            {
                InventUnit_ContentsBtnUI unitUI = obj.GetComponent<InventUnit_ContentsBtnUI>();
                if (unitUI.UnitData == unit)
                {
                    unitUI.Initialized();
                    unitUI.ImageClick();
                }
            }
        }
        else
        {
            ReMoveUnitUI();

            _clickUnitUI = _ui.CreateSubitem<InventUnitDescri_PopupUI>("InventUnitDescri_PopupUI", null, Literals.UI_POPUP_PATH);
            _ui.SetCanvasInfo(_clickUnitUI.gameObject, true);
            unit.Status.OnStatusChange += _clickUnitUI.SetInfo;
            _clickUnitUI.UnitData = unit;
        }
    }

    public void ReMoveUnitUI()
    {
        if (_clickUnitUI != null)
        {
            _clickUnitUI.UnitData.Status.OnStatusChange -= _clickUnitUI.SetInfo;
            _ui.DestroySubItem(_clickUnitUI.gameObject);
        }
    }

    #region UiBind

    private void SetButton()
    {
        SetUI<Button>();

        shopButton = GetUI<Button>("ShopButton");
        placingButton = GetUI<Button>("PlacingButton");
        _inventoryButton = GetUI<Button>("InventoryButton");
        _settingButton = GetUI<Button>("SettingButton");
        _stageStartButton = GetUI<Button>("StageStartButton");
        backButton = GetUI<Button>("BackButton");
        unitButton = GetUI<Button>("UnitBtn");
        roomButton = GetUI<Button>("RoomBtn");
        _speed1Button = GetUI<Button>("PlayButton");
        _speed2Button = GetUI<Button>("X2SpeedButton");
        _speed3Button = GetUI<Button>("X3SpeedButton");
        _stageStartYesButton = GetUI<Button>("YesBtn");
        _stageStartNoButton = GetUI<Button>("NoBtn");
        _nextStageInfoButton = GetUI<Button>("NextStageInfoButton");

        SetUICallback(shopButton.gameObject, EUIEventState.Click, ClickShopBtn);
        SetUICallback(_inventoryButton.gameObject, EUIEventState.Click, ClickInventoryBtn);
        SetUICallback(_settingButton.gameObject, EUIEventState.Click, ClickSettingBtn);

        SetUICallback(_stageStartButton.gameObject, EUIEventState.Click, ClickStageStartOpenBtn);
        SetUICallback(_stageStartYesButton.gameObject, EUIEventState.Click, ClickStageStartBtn);
        SetUICallback(_stageStartNoButton.gameObject, EUIEventState.Click, ClickStageStartNoBtn);

        SetUICallback(placingButton.gameObject, EUIEventState.Click, ClickPlacingBtn);
        SetUICallback(backButton.gameObject, EUIEventState.Click, ClickBackBtn);
        SetUICallback(unitButton.gameObject, EUIEventState.Click, ClickUnitBtn);
        SetUICallback(roomButton.gameObject, EUIEventState.Click, ClickRoomBtn);

        SetUICallback(_speed1Button.gameObject, EUIEventState.Click, ClickSpeed1Btn);
        SetUICallback(_speed2Button.gameObject, EUIEventState.Click, ClickSpeed2Btn);
        SetUICallback(_speed3Button.gameObject, EUIEventState.Click, ClickSpeed3Btn);

        SetUICallback(_nextStageInfoButton.gameObject, EUIEventState.Click, ClickNextStageInfoButtonBtn);
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
        _dayArrowImg = GetUI<Image>("DayArrowImg");
        _startYesNoPanel = GetUI<Image>("StartYesNoButton");

        _categoryPanel.gameObject.SetActive(false);
        _dayArrowImg.gameObject.SetActive(false);
        _startYesNoPanel.gameObject.SetActive(false);
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
        _upMoveUIList.Add(_nextStageInfoButton.GetComponent<RectTransform>());

        _categoryTransform = _categoryPanel.GetComponent<RectTransform>();
        _placingPanelTransform = _placingPanel.GetComponent<RectTransform>();
        _backBtnTransform = backButton.GetComponent<RectTransform>();
        _nightTransform = _speed1Button.transform.parent.GetComponent<RectTransform>();
        dayArrowTransform = _dayArrowImg.GetComponent<RectTransform>();
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
    private void ClickStageStartOpenBtn(PointerEventData eventData)
    {
        ReMoveUnitUI();
        _startYesNoPanel.gameObject.SetActive(true);

        if (gameManager.isTutorial) // 튜토리얼 중이라면
        {
            Main.Get<UIManager>().ClosePopup(); // 튜토리얼 팝업창 닫기
            if (tweener.IsActive())
            {
                tweener.Kill(); // Battle 버튼 가리키던 화살표 Kill.
            }
            _dayArrowImg.gameObject.SetActive(false); // day 화살표 비활성화
            shopButton.gameObject.SetActive(true);
            placingButton.gameObject.SetActive(true);
            _inventoryButton.gameObject.SetActive(true);
            roomButton.gameObject.SetActive(true);
            unitButton.gameObject.SetActive(true);

            roomDirBtsnUI.RightTopButton.gameObject.SetActive(true); // 열기닫기 버튼 SetActive 건드려놨던거 원상복구 시키기.
            roomDirBtsnUI.RightBottomButton.gameObject.SetActive(true);
            roomDirBtsnUI.LeftTopButton.gameObject.SetActive(true);
            roomDirBtsnUI.LeftBottomButton.gameObject.SetActive(true);
        }
    }

    private void ClickStageStartNoBtn(PointerEventData eventData)
    {
        _startYesNoPanel.gameObject.SetActive(false);
    }

    private void ClickStageStartBtn(PointerEventData eventData)
    {
        _startYesNoPanel.gameObject.SetActive(false);

        if (Main.Get<GameManager>().CurStage >= 35)
        {
            Error_PopupUI ui = _ui.OpenPopup<Error_PopupUI>();
            ui.curErrorText = "개발중입니다.";
            return;
        }

        if (gameManager.isHomeSet)
        {
            Stack<Vector2> newPath;
            Vector2 startPos = tileManager.GetRoom(1, 0).transform.position;
            startPos.y += 1.5f;
            Vector2 endPos = gameManager.HomeRoom.transform.position;
            endPos.y += 1.5f;

            if (tileManager.FindPath(startPos, endPos, out newPath))
            {
                stageManager.StartStage();
            }
            else
            {
                Error_PopupUI ui = _ui.OpenPopup<Error_PopupUI>();
                ui.curErrorText = "홈타입의 방으로 가는 길이 없습니다.";
            }
        }
        else
        {
            Error_PopupUI ui = _ui.OpenPopup<Error_PopupUI>();
            ui.curErrorText = "홈타입의 방을 설치해 주세요.";
        }
    }

    private void ClickUnitBtn(PointerEventData eventData)
    {

        if (gameManager.isTutorial)
        {
            unitButton.gameObject.SetActive(false);
            OpenPoketBlock(true);

            if (tweener.IsActive())
            {
                tweener.Kill(); // unit 버튼 가리키는 화살표 Kill.
            }
            dayArrowTransform.anchoredPosition = new Vector3(840f, 351f, 0f); // 보유 unit 가리키는 화살표.
            dayArrowTransform.Rotate(0f, 0f, -90f);
            tweener = dayArrowTransform.DOAnchorPosX(810f, animationDuration).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            OpenPoketBlock(true);
        }
    }

    private void ClickRoomBtn(PointerEventData eventData)
    {
        if (gameManager.isTutorial) // 튜토리얼 중이라면
        {
            roomButton.gameObject.SetActive(false);

            if (tileManager.SelectRoom == tileManager.GetRoom(1, 1))
            {
                if (!gameManager.isHomeSet)
                {
                    Main.Get<UIManager>().ClosePopup();
                    TutorialMsg_PopupUI ui = Main.Get<UIManager>().OpenPopup<TutorialMsg_PopupUI>();
                    ui.curTutorialText = Main.Get<DataManager>().Tutorial["T11"].Description;

                    if (_pocketBlock == null)
                    {
                        OpenPoketBlock(false);
                        if (tweener.IsActive())
                        {
                            tweener.Kill(); // Room 가리키던 화살표 Kill.
                        }
                        dayArrowTransform.anchoredPosition = new Vector3(840f, 387f, 0f); // Home Room 가리키는 화살표
                        tweener = dayArrowTransform.DOAnchorPosX(810f, animationDuration).SetLoops(-1, LoopType.Yoyo);

                        dayArrowTransform.Rotate(0f, 0f, -90f);
                    }
                }
            }
            else if (tileManager.SelectRoom == tileManager.GetRoom(1, 0))
            {
                OpenPoketBlock(false);
                if (tweener.IsActive())
                {
                    tweener.Kill(); // Room 가리키던 화살표 Kill.
                }
                _dayArrowImg.gameObject.SetActive(true);
                dayArrowTransform.anchoredPosition = new Vector3(840f, 265f, 0f);// Room[1] 요소 가리키는 화살표
                dayArrowTransform.Rotate(0f, 0f, -90f);
                tweener = dayArrowTransform.DOAnchorPosX(810f, animationDuration).SetLoops(-1, LoopType.Yoyo);

                TutorialMsg_PopupUI ui1 = Main.Get<UIManager>().OpenPopup<TutorialMsg_PopupUI>();
                ui1.curTutorialText = Main.Get<DataManager>().Tutorial["T13"].Description;
            }
        }
        else
        {
            OpenPoketBlock(false);
        }
    }

    private void ClickBackBtn(PointerEventData eventData)
    {
        ReMoveUnitUI();

        if (_btnActions.Count >= 1 && !isUIAnimating)
        {
            _btnActions.Pop().BtnActions.Invoke();
        }

        if (gameManager.isTutorial) // 튜토리얼 중이라면,
        {
            if (gameManager.isPlacingTutorialClear) // 배치 튜토리얼 완료 후 뒤로가기 버튼 누르면
            {
                placingButton.gameObject.SetActive(false); // 배치모드 버튼 비활성화
                _stageStartButton.gameObject.SetActive(true); // StageStart 버튼 활성화

                if (_btnActions.Count == 0) // 끝까지 뒤로갔을 때
                {
                    if (tweener.IsActive())
                    {
                        tweener.Kill(); // 배치모드 뒤로가기 버튼 가리키던 화살표 Kill

                        dayArrowTransform.anchoredPosition = new Vector3(730f, -245f, 0f); // Battle 버튼 위치잡고
                        tweener = dayArrowTransform.DOAnchorPosY(-275f, animationDuration).SetLoops(-1, LoopType.Yoyo); // Battle 버튼 DOTween 만들기
                        dayArrowTransform.Rotate(0f, 0f, 90f); // 90 도 돌리고

                        TutorialMsg_PopupUI ui = Main.Get<UIManager>().OpenPopup<TutorialMsg_PopupUI>(); // 튜토리얼 팝업
                        ui.curTutorialText = Main.Get<DataManager>().Tutorial["T17"].Description;
                    }
                }
            }
        }
    }

    private void ClickPlacingBtn(PointerEventData eventData)
    {
        ReMoveUnitUI();
        _ui.CloseAllPopup();

        ClickPlacing();
        _btnActions.Push(new UIState(ClickPlacing, EUIstate.Main));

        if (gameManager.isTutorial) // 튜토리얼 중이라면
        {
            tweener.Kill(); // 배치모드 가리키고 있던 화살표 kill.
            _dayArrowImg.gameObject.SetActive(false);
            tileManager.GetRoom(1, 1).StartFlashing();

            backButton.gameObject.SetActive(false);
            TutorialMsg_PopupUI ui = Main.Get<UIManager>().OpenPopup<TutorialMsg_PopupUI>(); // 가운데 Ground 눌러
            ui.curTutorialText = Main.Get<DataManager>().Tutorial["T9"].Description;
            ui.isCloseBtnActive = true;
            ui.isBackgroundActive = true;
        }
    }

    private void ClickShopBtn(PointerEventData eventData)
    {
        ReMoveUnitUI();
        _ui.CloseAllPopup();

        maincamera.Rock = true;
        shop_PopupUI = _ui.OpenPopup<Shop_PopupUI>("Shop_PopupUI");
        shop_PopupUI.Owner = this;

        if (gameManager.isTutorial) // 튜토리얼 중이라면
        {
            tweener.Kill(); // 상점 가리키고 있던 화살표 kill.
            _dayArrowImg.gameObject.SetActive(false);

            TutorialMsg_PopupUI tutorialUI = _ui.OpenPopup<TutorialMsg_PopupUI>();
            tutorialUI.curTutorialText = Main.Get<DataManager>().Tutorial["T1"].Description;
            tutorialUI.isCloseBtnActive = true;
            tutorialUI.isBackgroundActive = true;

            shopButton.gameObject.SetActive(false); // 상점 튜토리얼 완료하면 상점 버튼 inactive.
            _inventoryButton.gameObject.SetActive(true); // 상점 튜토리얼 다음 순서인 인벤토리 버튼 active.
            _dayArrowImg.gameObject.SetActive(true);
            dayArrowTransform.anchoredPosition = new Vector3(-241f, -276f, 0f); // 인벤토리 가리키는 화살표.
            tweener = dayArrowTransform.DOAnchorPosY(-306f, animationDuration).SetLoops(-1, LoopType.Yoyo);
        }
    }

    private void ClickSpeed1Btn(PointerEventData eventData)
    {
        Time.timeScale = 1.5f;
        _speed1Button.gameObject.SetActive(false);
        _speed2Button.gameObject.SetActive(true);
    }

    private void ClickSpeed2Btn(PointerEventData eventData)
    {
        Time.timeScale = 2.0f;
        _speed2Button.gameObject.SetActive(false);
        _speed3Button.gameObject.SetActive(true);
    }
    private void ClickSpeed3Btn(PointerEventData eventData)
    {
        Time.timeScale = 1.0f;
        _speed3Button.gameObject.SetActive(false);
        _speed1Button.gameObject.SetActive(true);
    }

    private void ClickNextStageInfoButtonBtn(PointerEventData eventData)
    {
        _ui.OpenPopup<NextStageInfo_PopupUI>("NextStageInfo_PopupUI");
    }

    private void SetTimeScale(int stage)
    {
        if (_speed1Button.gameObject.activeSelf)
        {
            Time.timeScale = 1.0f;
        }
        else if (_speed2Button.gameObject.activeSelf)
        {
            Time.timeScale = 1.5f;
        }
        else if (_speed3Button.gameObject.activeSelf)
        {
            Time.timeScale = 2.0f;
        }
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

        if (gameManager.isTutorial) // 튜토리얼 중이라면.
        {
            _ui.ClosePopup(); // 열려있던 튜토리얼msg 팝업 먼저 끄기.
        }

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
            ReMoveUnitUI();
            _ui.CloseAllPopup();
            inventory_PopupUI = _ui.OpenPopup<Inventory_PopupUI>("Inventory_PopupUI");
            inventory_PopupUI.Owner = this;
            isInventOpen = true;
        }

        if (gameManager.isTutorial) // 튜토리얼 중이라면.
        {
            _inventoryButton.gameObject.SetActive(false); // 인벤토리 버튼 비활성화

            tweener.Kill(); // 인벤토리 버튼 가리키턴 화살표 kill.
            _dayArrowImg.gameObject.SetActive(false);

            TutorialMsg_PopupUI tutorialUI = _ui.OpenPopup<TutorialMsg_PopupUI>();
            tutorialUI.curTutorialText = Main.Get<DataManager>().Tutorial["T4"].Description;
            inventory_PopupUI.tutorialMsg_PopupUI = tutorialUI;
            tutorialUI.isBackgroundActive = true;
            tutorialUI.isCloseBtnActive = true;
        }
    }

    private void ClickSettingBtn(PointerEventData eventData)
    {
        maincamera.Rock = true;
        Main.Get<UIManager>().OpenPopup<Setting_PopupUI>();
    }

    private void OpenPoketBlock(bool isUint)
    {
        if (_pocketBlock == null)
        {
            _pocketBlock = _ui.OpenPopup<PocketBlock_PopupUI>();
            _pocketBlock.Owner = this;
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
        roomDirBtsnUI.SetPosition(tileManager.SelectRoom.transform.position);
        FocusCamera(); //방 클릭시 줌인하는 기능

        if (_btnActions.Peek().UIStagte != EUIstate.ChangeTileSelect)
        {
            _btnActions.Push(new UIState(SetTileBatUI, EUIstate.ChangeTileSelect));
            SetTileBatUI();
        }

        if (gameManager.isTutorial)
        {
            if (!gameManager.isHomeSet)
            {
                TutorialMsg_PopupUI ui = _ui.OpenPopup<TutorialMsg_PopupUI>();
                ui.curTutorialText = Main.Get<DataManager>().Tutorial["T10"].Description;
                backButton.gameObject.SetActive(false);
                _dayArrowImg.gameObject.SetActive(true);
                dayArrowTransform.anchoredPosition = new Vector3(860f, 230f, 0f); // Room 버튼 가리키는 화살표
                tweener = dayArrowTransform.DOAnchorPosY(200f, animationDuration).SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                roomButton.gameObject.SetActive(true);
                if (tweener.IsActive())
                {
                    tweener.Kill();
                }
                _dayArrowImg.gameObject.SetActive(true);
                dayArrowTransform.anchoredPosition = new Vector3(860f, 230f, 0f); // Room 버튼 가리키는 화살표
                tweener = dayArrowTransform.DOAnchorPosY(200f, animationDuration).SetLoops(-1, LoopType.Yoyo);
            }
        }
    }

    public void SetTileBatUI()
    {
        if (isUIAnimating)
            return;

        StartCoroutine(ButtonRock());
        MovePosYUI(_placingPanelTransform, 220f);
        MovePosXUI(_categoryTransform, 200f, () =>
        {
            if (_btnActions.Peek().UIStagte == EUIstate.ChangeTileSelect)
            {
                if (gameManager.isTutorial && !gameManager.PlayerRooms[1].IsEquiped) // 튜토리얼 중인데 배치룸 배치 안 했으면
                {
                    roomDirBtsnUI.gameObject.SetActive(false);

                }
                else if (!gameManager.isTutorial)                                   // 튜토리얼이 아니라면
                {
                    roomDirBtsnUI.gameObject.SetActive(true);
                }
            }
        });

        if (_btnActions.Peek().UIStagte != EUIstate.ChangeTileSelect)
        {
            tileManager.SelectRoom.StopFlashing();
            tileManager.InactiveBatSlot();
            tileManager.SetSelectRoom(null);
            _ui.CloseAllPopup();
            Camera.main.DOOrthoSize(5.0f, animationDuration);
            maincamera.Rock = false;
            roomDirBtsnUI.gameObject.SetActive(false);
        }
    }

    #endregion

    #region UIDOTween

    private void MovePosYUI(RectTransform ui, float offset, Action moveEndAction = null)
    {
        if (!ui.gameObject.activeSelf)
        {
            ui.gameObject.SetActive(true);
            ui.DOAnchorPosY(ui.anchoredPosition.y + offset, animationDuration).OnComplete(() =>
            {
                moveEndAction?.Invoke();
            });
        }
        else
        {
            ui.DOAnchorPosY(ui.anchoredPosition.y - offset, animationDuration).OnComplete(() =>
            {
                ui.gameObject.SetActive(false);
                moveEndAction?.Invoke();
            });
        }
    }

    private void MovePosYUI(List<RectTransform> ui, float offset, Action moveEndAction = null)
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
                    moveEndAction?.Invoke();
                });
            }
        }
        else
        {
            foreach (var rect in ui)
            {
                rect.gameObject.SetActive(true);
                rect.DOAnchorPosY(rect.anchoredPosition.y - offset, animationDuration).OnComplete(() =>
                {
                    moveEndAction?.Invoke();
                });
            }
        }
    }

    private void MovePosXUI(RectTransform ui, float offset, Action moveEndAction = null)
    {
        if (!ui.gameObject.activeSelf)
        {
            ui.gameObject.SetActive(true);
            ui.DOAnchorPosX(ui.anchoredPosition.x - offset, animationDuration).OnComplete(() =>
            {
                moveEndAction?.Invoke();
            });
        }
        else
        {
            ui.DOAnchorPosX(ui.anchoredPosition.x + offset, animationDuration).OnComplete(() =>
            {
                ui.gameObject.SetActive(false);
                moveEndAction?.Invoke();
            });
        }
    }

    private void FocusCamera()
    {
        Vector3 pos = new Vector3(tileManager.SelectRoom.transform.position.x,
            tileManager.SelectRoom.transform.position.y + 1.8f, Camera.main.transform.position.z);
        Camera.main.transform.DOMove(pos, animationDuration);
        //Camera.main.DOOrthoSize(2.5f, animationDuration); //줌인기능
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