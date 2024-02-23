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
    public Image _categoryPanel { get; set; }
    public Image _placingPanel { get; set; }
    private Image _hpPanel;
    public Image _dayArrowImg { get; set; }

    public Button shopButton { get; set; }
    public Button placingButton { get; set; }
    private Button _inventoryButton;
    private Button _stageStartButton;
    private Button _settingButton;
    public Button backButton { get; set; }
    private Button _unitButton;
    private Button _roomButton;
    private Button _speed1Button;
    private Button _speed2Button;
    public Button rightTopButton { get; set; }
    public Button rightBottomButton { get; set; }
    public Button leftTopButton { get; set; }
    public Button leftBottomButton { get; set; }

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
    public RectTransform dayArrowTransform { get; set; }

    private Error_PopupUI _errorPopupUI;

    private PocketBlock_PopupUI _pocketBlock;

    private Stack<UIState> _btnActions = new Stack<UIState>();

    public Tweener tweener { get; set; }

    public Inventory_PopupUI inventory_PopupUI;

    public TutorialMsg_PopupUI tutorialMsg_PopupUI;

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
        stageManager.OnStageStartEvent += SetTimeScale;

        stageManager.OnStageClearEvent += ClickStart;
        stageManager.OnStageClearEvent += UpdateDayCount;

        tileManager.OnSlectRoomEvent += TileBat;

        if (gameManager.isTutorial) // 튜토리얼 중이라면
        {
            placingButton.gameObject.SetActive(false); // 배치모드 버튼 비활성화.
            _unitButton.gameObject.SetActive(false); // 배치모드의 unit 버튼 비활성화.
            backButton.gameObject.SetActive(false); // 배치모드 뒤로가기 버튼 비활성화.
            rightBottomButton.gameObject.SetActive(false); // 배치모드의 열기닫기 버튼 비활성화.
            rightTopButton.gameObject.SetActive(false);
            leftBottomButton.gameObject.SetActive(false);
            leftTopButton.gameObject.SetActive(false);
            _inventoryButton.gameObject.SetActive(false); // 인벤토리 버튼 비활성화.
            _stageStartButton.gameObject.SetActive(false); // Battle 버튼 비활성화.

            TutorialMsg_PopupUI ui = Main.Get<UIManager>().OpenPopup<TutorialMsg_PopupUI>();
            ui.curTutorialText =
                "<b>[튜토리얼]</b>\n\n적의 침입으로부터 메인 거점인 Home 을 지켜내야 해요!\n지키기 위해서는 침입을 막아줄 Unit 과 Room 이 필요해요.\n\n그럼, <color=#E9D038><b>상점</b></color>에서 제공해드린 재화로\nUnit 과 Room 을 구매해봅시다!";
            _dayArrowImg.gameObject.SetActive(true);
            dayArrowTransform.anchoredPosition = new Vector3(-770f, -276f, 0f); // 상점 가리키는 화살표.
            tweener = dayArrowTransform.DOAnchorPosY(-306f, animationDuration).SetLoops(-1, LoopType.Yoyo);
        }
        UpdateHpUI(0);
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
        _unitButton = GetUI<Button>("UnitBtn");
        _roomButton = GetUI<Button>("RoomBtn");
        _speed1Button = GetUI<Button>("PlayButton");
        _speed2Button = GetUI<Button>("X2SpeedButton");
        rightTopButton = GetUI<Button>("RightTop");
        rightBottomButton = GetUI<Button>("RightBottom");
        leftTopButton = GetUI<Button>("LeftTop");
        leftBottomButton = GetUI<Button>("LeftBottom");

        SetUICallback(shopButton.gameObject, EUIEventState.Click, ClickShopBtn);
        SetUICallback(_inventoryButton.gameObject, EUIEventState.Click, ClickInventoryBtn);
        SetUICallback(_settingButton.gameObject, EUIEventState.Click, ClickSettingBtn);
        SetUICallback(_stageStartButton.gameObject, EUIEventState.Click, ClickStageStartBtn);
        SetUICallback(placingButton.gameObject, EUIEventState.Click, ClickPlacingBtn);
        SetUICallback(backButton.gameObject, EUIEventState.Click, ClickBackBtn);
        SetUICallback(_unitButton.gameObject, EUIEventState.Click, ClickUnitBtn);
        SetUICallback(_roomButton.gameObject, EUIEventState.Click, ClickRoomBtn);
        SetUICallback(_speed1Button.gameObject, EUIEventState.Click, ClickSpeed1Btn);
        SetUICallback(_speed2Button.gameObject, EUIEventState.Click, ClickSpeed2Btn);
        SetUICallback(rightTopButton.gameObject, EUIEventState.Click, ClickRightTopBtn);
        SetUICallback(rightBottomButton.gameObject, EUIEventState.Click, ClickRightBottomBtn);
        SetUICallback(leftTopButton.gameObject, EUIEventState.Click, ClickLeftTopBtn);
        SetUICallback(leftBottomButton.gameObject, EUIEventState.Click, ClickLeftBottomBtn);
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

        _categoryPanel.gameObject.SetActive(false);
        _dayArrowImg.gameObject.SetActive(false);
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
        _backBtnTransform = backButton.GetComponent<RectTransform>();
        _nightTransform = _speed1Button.transform.parent.GetComponent<RectTransform>();
        _roomDirTransform = rightTopButton.transform.parent.GetComponent<RectTransform>();
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

    private void ClickStageStartBtn(PointerEventData eventData)
    {
        if (Main.Get<GameManager>().CurStage >= 35)
        {
            Error_PopupUI ui = _ui.OpenPopup<Error_PopupUI>();
            ui.curErrorText = "개발중입니다.";
            return;
        }
        if (gameManager.isHomeSet)
        {
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
                _roomButton.gameObject.SetActive(true);
                _unitButton.gameObject.SetActive(true);
                rightTopButton.gameObject.SetActive(true);
                rightBottomButton.gameObject.SetActive(true);
                leftTopButton.gameObject.SetActive(true);
                leftBottomButton.gameObject.SetActive(true);
            }

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
            _unitButton.gameObject.SetActive(false);
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
            _roomButton.gameObject.SetActive(false);

            if (tileManager.SelectRoom == tileManager.GetRoom(1, 1))
            {
                if (!gameManager.isHomeSet)
                {
                    Main.Get<UIManager>().ClosePopup();
                    TutorialMsg_PopupUI ui = Main.Get<UIManager>().OpenPopup<TutorialMsg_PopupUI>();
                    ui.curTutorialText = "Room 의 종류는 크게 Home, Bat, Trap 으로 나뉘어져있어요.\n\n우선 맨 위에 있는 <color=#E9D038><b>Home</b></color> 은\n적으로부터 지켜야 할 제일 중요한 Room 이에요.\n그리고 Home 이 배치가 되어있어야 Battle 을 시작할 수 있어요.\n<color=#E9D038><b>클릭해서 배치해봅시다!</b></color>";

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
                ui1.curTutorialText = "Room 의 종류로는 앞서 설명드린 Home 외에,\n유닛을 배치할 수 있는 <color=#E9D038><b>Bat</b></color> 타입과, \n유닛 배치는 불가능하지만,\n들어온 적에게 함정효과가 발동되는 <color=#E9D038><b>Trap</b></color> 타입이 있어요.\n\n남은 Room 을 클릭해 배치해봅시다!";
            }
        }
        else
        {
            OpenPoketBlock(false);
        }
    }

    private void ClickBackBtn(PointerEventData eventData)
    {
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
                        ui.curTutorialText = "이제 모든 준비가 완료됐으니\n적들의 침입을 막으러 가봅시다!\n<color=#E9D038><b>BATTLE 버튼</b></color>을 눌러주세요!";

                    }
                }
            }
        }
    }

    private void ClickPlacingBtn(PointerEventData eventData)
    {
        _ui.CloseAllPopup();

        ClickPlacing();
        _btnActions.Push(new UIState(ClickPlacing, EUIstate.Main));

        if (gameManager.isTutorial) // 튜토리얼 중이라면
        {
            tweener.Kill(); // 배치모드 가리키고 있던 화살표 kill.
            _dayArrowImg.gameObject.SetActive(false);
            tileManager.GetRoom(1, 1).StartFlashing();
            //_dayArrowTransform.anchoredPosition = new Vector3(); // 가운데 Ground 강조 위치 (근데 유저가 화면을 움직일 수 있어서 애매하네)

            backButton.gameObject.SetActive(false);
            TutorialMsg_PopupUI ui = Main.Get<UIManager>().OpenPopup<TutorialMsg_PopupUI>(); // 가운데 Ground 눌러
            ui.curTutorialText = "<color=#E9D038><b>중앙에 위치한 Ground</b></color> 를 클릭해주세요!";
            ui.isCloseBtnActive = true;
            ui.isBackgroundActive = true;
        }
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

            TutorialMsg_PopupUI tutorialUI = _ui.OpenPopup<TutorialMsg_PopupUI>();
            tutorialUI.curTutorialText = "먼저,\n카테고리에서 <color=#E9D038><b>Unit</b></color> 을 클릭해\n<color=#E9D038><b>3회 뽑기</b></color>를 진행해주세요!";
            tutorialUI.isCloseBtnActive = true;
            tutorialUI.isBackgroundActive = true;
            //StartCoroutine(ClosePopupUI());

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
        Time.timeScale = 1.0f;
        _speed1Button.gameObject.SetActive(true);
        _speed2Button.gameObject.SetActive(false);
    }

    private void ClickRightTopBtn(PointerEventData eventData)
    {
        if (gameManager.isTutorial)
        {
            return;
        }

        RoomBehavior room = tileManager.SelectRoom;
        tileManager.SetRoomDir(room, ERoomDir.RightTop, !room.IsDoorOpen(ERoomDir.RightTop));
    }

    private void ClickLeftTopBtn(PointerEventData eventData)
    {
        RoomBehavior room = tileManager.SelectRoom;
        tileManager.SetRoomDir(room, ERoomDir.LeftTop, !room.IsDoorOpen(ERoomDir.LeftTop));

        if (gameManager.isTutorial)
        {
            CheckBtnForTutorial();

            rightTopButton.gameObject.SetActive(false);
            rightBottomButton.gameObject.SetActive(false);
            leftTopButton.gameObject.SetActive(false);
            leftBottomButton.gameObject.SetActive(false);

            if (tweener.IsActive())
            {
                tweener.Kill(); // 열기닫기 가리키는 화살표 kill.
            }
            dayArrowTransform.anchoredPosition = new Vector3(860f, 60f, 0f); // unit 버튼 가리키는 화살표
            dayArrowTransform.Rotate(0f, 0f, 90f);
            tweener = dayArrowTransform.DOAnchorPosY(30f, animationDuration).SetLoops(-1, LoopType.Yoyo);
        }

    }

    private void ClickRightBottomBtn(PointerEventData eventData)
    {
        if (gameManager.isTutorial)
        {
            return;
        }

        RoomBehavior room = tileManager.SelectRoom;
        tileManager.SetRoomDir(room, ERoomDir.RightBottom, !room.IsDoorOpen(ERoomDir.RightBottom));
    }

    private void ClickLeftBottomBtn(PointerEventData eventData)
    {
        if (gameManager.isTutorial)
        {
            return;
        }

        RoomBehavior room = tileManager.SelectRoom;
        tileManager.SetRoomDir(room, ERoomDir.LeftBottom, !room.IsDoorOpen(ERoomDir.LeftBottom));
    }

    private void CheckBtnForTutorial()
    {
        if (gameManager.isTutorial)
        {
            Main.Get<UIManager>().CloseAllPopup(); // 먼저 떠 있던 튜토리얼 팝업창 및 PocketBlock 닫기

            if (gameManager.PlayerRooms[0].IsEquiped && gameManager.PlayerRooms[1].IsEquiped) // 보유한 Room 모두 장착 중일 때
            {
                if (tutorialMsg_PopupUI == null)
                {
                    tutorialMsg_PopupUI = Main.Get<UIManager>().OpenPopup<TutorialMsg_PopupUI>();
                    tutorialMsg_PopupUI.curTutorialText = "방금 배치한 Room 위에 Unit 도 배치해봅시다.\n\n<color=#FF8888><b>※ Unit은 왼쪽부터 배치가 되고 전투시 가장 왼쪽에 있는 Unit부터 공격대상이됩니다 ※</b></color>";
                }

                if (_roomButton.gameObject.activeSelf) // Room 버튼 활성화되어있다면 비활성화 진행.
                {
                    _roomButton.gameObject.SetActive(false);
                }
                if (!_unitButton.gameObject.activeSelf) // Unit 버튼 비활성화 되어있다면 활성화 진행.
                {
                    _unitButton.gameObject.SetActive(true);
                }
            }
        }
    }

    private void SetTimeScale(int stage)
    {
        if (_speed1Button.gameObject.activeSelf)
        {
            Time.timeScale = 1.0f;
        }
        else
        {
            Time.timeScale = 1.5f;
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
            Main.Get<UIManager>().ClosePopup(); // 열려있던 튜토리얼msg 팝업 먼저 끄기.
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
            tutorialUI.curTutorialText = "그리고 동일한 종류와 레벨의 Unit 이나 Room 이 3개 있으면\n다음 레벨로 업그레이드를 진행할 수도 있어요.\n\n먼저, <color=#E9D038><b>업그레이드 버튼</b></color>을 누른 다음,\n보유한 <color=#E9D038><b>Room</b></color> 을 클릭해서\n업그레이드를 진행해주세요!";
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
        FocusCamera();

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
                ui.curTutorialText = "먼저, Room 을 배치해볼게요. <color=#E9D038><b>Room 버튼</b></color>을 눌러주세요.";
                backButton.gameObject.SetActive(false);
                _dayArrowImg.gameObject.SetActive(true);
                dayArrowTransform.anchoredPosition = new Vector3(860f, 230f, 0f); // Room 버튼 가리키는 화살표
                tweener = dayArrowTransform.DOAnchorPosY(200f, animationDuration).SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                _roomButton.gameObject.SetActive(true);
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
   
    #endregion
}