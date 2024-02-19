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
    private Image _dayArrowImg;
    private Image _nightArrowImg;

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
    private Button _rightTopButton;
    private Button _rightBottomButton;
    private Button _leftTopButton;
    private Button _leftBottomButton;

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
    private RectTransform _roomDirTransform;
    private RectTransform _dayArrowTransform;
    private RectTransform _nightArrowTransform;

    private Error_PopupUI _errorPopupUI;

    private PocketBlock_PopupUI _pocketBlock;

    private Stack<UIState> _btnActions = new Stack<UIState>();

    private Tweener tweener;

    private bool isPlaceTutorialClear = false; // 배치모드 튜토리얼 클리어했는지 체크.

    public Inventory_PopupUI inventory_PopupUI;

    public CameraMovement maincamera;

    public Shop_PopupUI shop_PopupUI { get; set; }

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

        if (gameManager.isTutorial) // 튜토리얼 중이라면 팝업메세지 띄우고, 강조화살표 등장.
        {
            _placingButton.gameObject.SetActive(false);
            _inventoryButton.gameObject.SetActive(false);
            _stageStartButton.gameObject.SetActive(false);

            TutorialMsg_PopupUI ui = Main.Get<UIManager>().OpenPopup<TutorialMsg_PopupUI>();
            ui.curTutorialText =
                "적의 침입으로부터 메인 거점인 Home 을 지켜내야 해요!\n지키기 위해서는 침입을 막아줄 Unit 과 Room 이 필요해요.\n\n그럼, 상점에서 제공해드린 재화로\nUnit 과 Room 을 구매해봅시다!";
            _dayArrowImg.gameObject.SetActive(true);
            _dayArrowTransform.anchoredPosition = new Vector3(-770f, -276f, 0f); // 상점 가리키는 화살표.
            tweener = _dayArrowTransform.DOAnchorPosY(-306f, animationDuration).SetLoops(-1, LoopType.Yoyo);
        }
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
        _rightTopButton = GetUI<Button>("RightTop");
        _rightBottomButton = GetUI<Button>("RightBottom");
        _leftTopButton = GetUI<Button>("LeftTop");
        _leftBottomButton = GetUI<Button>("LeftBottom");

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
        SetUICallback(_rightTopButton.gameObject, EUIEventState.Click, ClickRightTopBtn);
        SetUICallback(_rightBottomButton.gameObject, EUIEventState.Click, ClickRightBottomBtn);
        SetUICallback(_leftTopButton.gameObject, EUIEventState.Click, ClickLeftTopBtn);
        SetUICallback(_leftBottomButton.gameObject, EUIEventState.Click, ClickLeftBottomBtn);
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
        _nightArrowImg = GetUI<Image>("NightArrowImg");

        _categoryPanel.gameObject.SetActive(false);
        _dayArrowImg.gameObject.SetActive(false);
        _nightArrowImg.gameObject.SetActive(false);
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
        _roomDirTransform = _rightTopButton.transform.parent.GetComponent<RectTransform>();
        _dayArrowTransform = _dayArrowImg.GetComponent<RectTransform>();
        _nightArrowTransform = _dayArrowImg.GetComponent<RectTransform>();
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
        if (gameManager.isTutorial)
        {
            Main.Get<UIManager>().ClosePopup();
            tweener.Kill(); // Room 버튼 가리키고 있던 화살표 kill.
            TutorialMsg_PopupUI ui = Main.Get<UIManager>().OpenPopup<TutorialMsg_PopupUI>();
            ui.curTutorialText = "룸의 종류는 크게 Home, Bat, Trap 으로 나뉘어져있어요.\n\n우선 맨 위에 있는 Home 은 적으로부터 지켜야 할 제일 중요한 룸이에요.\n그리고 Home 이 배치가 되어있어야 Battle 을 시작할 수 있어요.\n클릭해서 배치해봅시다!";
            _dayArrowTransform.anchoredPosition = new Vector3(840f, 387f, 0f);
            _dayArrowTransform.Rotate(0f, 0f, -90f);
            tweener = _dayArrowTransform.DOAnchorPosX(810f, animationDuration).SetLoops(-1, LoopType.Yoyo);

            if (gameManager.isHomeSet)
            {
                tweener.Kill(); // Home 룸 가리키고 있던 화살표 kill.
                _dayArrowImg.gameObject.SetActive(false);
                TutorialMsg_PopupUI ui1 = Main.Get<UIManager>().OpenPopup<TutorialMsg_PopupUI>();
                ui1.curTutorialText = "그 외로는 유닛을 배치할 수 있는 Bat 타입과,\n유닛 배치는 불가능하지만, 들어온 적에게 함정효과가 발동되는 Trap 타입이 있어요.\n룸을 배치한 후에 동일한 방법으로 유닛도 배치해봅시다.\n전략적인 배치를 기대해볼게요!";
            }
        }
        OpenPoketBlock(false);
    }

    private void ClickBackBtn(PointerEventData eventData)
    {
        if (_btnActions.Count >= 1 && !isUIAnimating)
        {
            _btnActions.Pop().BtnActions.Invoke();
        }

        if (gameManager.isTutorial)
        {
            if (isPlaceTutorialClear)
            {
                tweener.Kill(); // todo : check - 의도는 배치모드 가리키고 있던 화살표 kill.
                _dayArrowImg.gameObject.SetActive(false);
            }
            else
            {
                _dayArrowImg.gameObject.SetActive(true);
                _dayArrowTransform.anchoredPosition = new Vector3(-500f, -276f, 0f); // 배치모드 가리키는 화살표.
                tweener = _dayArrowTransform.DOAnchorPosY(-306f, animationDuration).SetLoops(-1, LoopType.Yoyo);
            }
        }
    }

    private void ClickPlacingBtn(PointerEventData eventData)
    {
        _ui.CloseAllPopup();

        if (gameManager.isTutorial)
        {
            tweener.Kill(); // 배치모드 가리키고 있던 화살표 kill.
            _dayArrowImg.gameObject.SetActive(false);
        }

        ClickPlacing();
        _btnActions.Push(new UIState(ClickPlacing, EUIstate.Main));
    }

    private void ClickShopBtn(PointerEventData eventData)
    {
        _ui.CloseAllPopup();

        maincamera.Rock = true;
        shop_PopupUI = _ui.OpenPopup<Shop_PopupUI>("Shop_PopupUI");
        shop_PopupUI.Owner = this;

        if (gameManager.isTutorial) // 튜토리얼 중이라면
        {
            tweener.Kill(); // 상점 가리키고 있던 화살표 kill.
            _dayArrowImg.gameObject.SetActive(false);

            if (gameManager.playerUnits.Count >= 3 && gameManager.PlayerRooms.Count >= 6) // 상점 튜토리얼 완료했다면, 상점 내에 뒤로가기 버튼 항상 활성화.
            {
                shop_PopupUI.isShopTutorialClear = true;
            }

            if (!(gameManager.playerUnits.Count >= 3 && gameManager.PlayerRooms.Count >= 6))
            {
                TutorialMsg_PopupUI tutorialUI = _ui.OpenPopup<TutorialMsg_PopupUI>();
                tutorialUI.curTutorialText = "카테고리에서 Unit 과 Room 을 클릭해 구매를 진행해봅시다!\n(Unit 은 3회 이상, Room 은 6회 이상 뽑기를 진행해주세요.)\n\n튜토리얼 후에는 Room 을 배치하는 Ground 와\n유닛의 능력치를 올려주는 Item 도 구매할 수 있어요!";
                tutorialUI.isCloseBtnActive = true;
                //StartCoroutine(ClosePopupUI());
            }

            _placingButton.gameObject.SetActive(true);
            _dayArrowImg.gameObject.SetActive(true);
            _dayArrowTransform.anchoredPosition = new Vector3(-500f, -276f, 0f); // 배치모드 가리키는 화살표.
            tweener = _dayArrowTransform.DOAnchorPosY(-306f, animationDuration).SetLoops(-1, LoopType.Yoyo);
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
        Time.timeScale = 1.0f;
        _speed1Button.gameObject.SetActive(true);
        _speed2Button.gameObject.SetActive(false);
    }

    private void ClickRightTopBtn(PointerEventData eventData)
    {
        RoomBehavior room = tileManager.SelectRoom;
        tileManager.SetRoomDir(room, ERoomDir.RightTop, !room.IsDoorOpen(ERoomDir.RightTop));
    }

    private void ClickLeftTopBtn(PointerEventData eventData)
    {
        RoomBehavior room = tileManager.SelectRoom;
        tileManager.SetRoomDir(room, ERoomDir.LeftTop, !room.IsDoorOpen(ERoomDir.LeftTop));
    }

    private void ClickRightBottomBtn(PointerEventData eventData)
    {
        RoomBehavior room = tileManager.SelectRoom;
        tileManager.SetRoomDir(room, ERoomDir.RightBottom, !room.IsDoorOpen(ERoomDir.RightBottom));
    }

    private void ClickLeftBottomBtn(PointerEventData eventData)
    {
        RoomBehavior room = tileManager.SelectRoom;
        tileManager.SetRoomDir(room, ERoomDir.LeftBottom, !room.IsDoorOpen(ERoomDir.LeftBottom));
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

        if (gameManager.isTutorial)
        {
            TutorialMsg_PopupUI ui = _ui.OpenPopup<TutorialMsg_PopupUI>();
            ui.curTutorialText = "먼저, 룸을 배치해볼게요. 룸 버튼을 눌러주세요.";
            _dayArrowImg.gameObject.SetActive(true);
            _dayArrowTransform.anchoredPosition = new Vector3(860f, 230f, 0f);
            tweener = _dayArrowTransform.DOAnchorPosY(200f, animationDuration).SetLoops(-1, LoopType.Yoyo);
        }

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
        MovePosXUI(_categoryTransform, 200f, () => 
        {
            if (_btnActions.Peek().UIStagte == EUIstate.ChangeTileSelect)
            { 
                _roomDirTransform.gameObject.SetActive(true);
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
            _roomDirTransform.gameObject.SetActive(false);
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
        Camera.main.DOOrthoSize(2.5f, animationDuration);
    }

    private IEnumerator ButtonRock()
    {
        isUIAnimating = true;
        var sec = new WaitForSeconds(animationDuration);
        yield return sec;
        isUIAnimating = false;
    }
    //private IEnumerator ClosePopupUI()
    //{
    //    yield return new WaitForSeconds(5f);

    //    _ui.ClosePopup();
    //}
    #endregion
}