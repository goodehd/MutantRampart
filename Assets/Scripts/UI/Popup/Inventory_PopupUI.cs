using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory_PopupUI : BaseUI
{
    private GameManager gameManager;

    //private Button _backButton;
    private Button _roomButton;
    private Button _unitButton;
    public Button closeButton { get; set; }
    public Button upgradeButton { get; set; }

    private Button _sortNameButton;
    private Button _sortLeveButton;

    private TextMeshProUGUI _sortNameText;
    private TextMeshProUGUI _sortLevelText;

    private ScrollRect _inventRoomScrollView;
    private ScrollRect _inventUnitScrollView;

    private Transform _inventRoomContent;
    private Transform _inventUnitContent;

    public Image inventArrowImg { get; set; }

    public RectTransform inventArrowTransform { get; set; }

    public float animationDuration = 0.3f;

    public Tweener tweener { get; set; }
    public DayMain_SceneUI Owner { get; set; }
    public InventRoomDescri_PopupUI inventRoomDescri_PopupUI;
    public InventUnitDescri_PopupUI inventUnitDescri_PopupUI;
    public InventUpgrade_PopupUI inventUpgrade_PopupUI;
    public TutorialMsg_PopupUI tutorialMsg_PopupUI;

    private bool _nameAscend;
    private bool _levelAscend;

    protected override void Init()
    {
        base.Init();

        SetUI<Button>();
        SetUI<TextMeshProUGUI>();
        SetUI<Transform>();
        SetUI<ScrollRect>();
        SetUI<Image>();

        gameManager = Main.Get<GameManager>();

        //_backButton = GetUI<Button>("InventBackBtn");
        _roomButton = GetUI<Button>("InventRoomBtn");
        _unitButton = GetUI<Button>("InventUnitBtn");
        closeButton = GetUI<Button>("InventoryCloseBtn");
        upgradeButton = GetUI<Button>("InventoryUpgradeBtn");
        _sortNameButton = GetUI<Button>("SortNameBtn");
        _sortLeveButton = GetUI<Button>("SortLevelBtn");

        _sortNameText = GetUI<TextMeshProUGUI>("SortNameText");
        _sortLevelText = GetUI<TextMeshProUGUI>("SortLevelText");

        //SetUICallback(_backButton.gameObject, EUIEventState.Click, ClickBackBtn);
        SetUICallback(_roomButton.gameObject, EUIEventState.Click, ClickRoomBtn);
        SetUICallback(_unitButton.gameObject, EUIEventState.Click, ClickUnitBtn);
        SetUICallback(closeButton.gameObject, EUIEventState.Click, ClickCloseBtn);
        SetUICallback(upgradeButton.gameObject, EUIEventState.Click, ClickUpgradeBtn);
        SetUICallback(_sortNameButton.gameObject, EUIEventState.Click, ClickSortNameBtn);
        SetUICallback(_sortLeveButton.gameObject, EUIEventState.Click, ClickSortLevelBtn);


        _inventRoomScrollView = GetUI<ScrollRect>("InventRoom_Scroll View");
        _inventUnitScrollView = GetUI<ScrollRect>("InventUnit_Scroll View");

        _inventRoomContent = GetUI<Transform>("InventRoom_Content");
        _inventUnitContent = GetUI<Transform>("InventUnit_Content");

        inventArrowImg = GetUI<Image>("InventArrowImg");

        inventArrowTransform = inventArrowImg.GetComponent<RectTransform>();

        Owner.inventory_PopupUI = this;

        SetRoomInventory();
        SetUnitInventory();

        if (!upgradeButton.gameObject.activeSelf)
        {
            upgradeButton.gameObject.SetActive(true);
        }

        if (gameManager.isTutorial)
        {
            inventArrowImg.gameObject.SetActive(true);
            tweener = inventArrowTransform.DOAnchorPosY(-330f, animationDuration).SetLoops(-1, LoopType.Yoyo); // 인벤토리 업그레이드 버튼 가리키는 화살표 DOTween.

            closeButton.gameObject.SetActive(false); // 일단 인벤토리 닫기 버튼 inactive 해두고, 룸, 유닛 업그레이드 완료하면 active 해주기
            InActiveSortButton();
        }

        _nameAscend = false;
        _levelAscend = false;
    }

    private void ClickSortLevelBtn(PointerEventData data)
    {
        if(inventUnitDescri_PopupUI != null)
        {
            _ui.ClosePopup();
        }

        if (inventRoomDescri_PopupUI != null)
        {
            _ui.ClosePopup();
        }

        if (_inventUnitScrollView.gameObject.activeSelf)
        {
            gameManager.SortUnitLevel(_levelAscend);
            _levelAscend = !_levelAscend;
            if (_levelAscend)
            {
                _sortLevelText.text = "등급순▼";
            }
            else
            {
                _sortLevelText.text = "등급순▲";
            }
            SetUnitInventory();
        }

        if (_inventRoomScrollView.gameObject.activeSelf)
        {
            gameManager.SortRoomLevel(_levelAscend);
            _levelAscend = !_levelAscend;
            if (_levelAscend)
            {
                _sortLevelText.text = "등급순▼";
            }
            else
            {
                _sortLevelText.text = "등급순▲";
            }
            SetRoomInventory();
        }
    }

    private void ClickSortNameBtn(PointerEventData data)
    {
        if (inventUnitDescri_PopupUI != null)
        {
            _ui.ClosePopup();
        }

        if (inventRoomDescri_PopupUI != null)
        {
            _ui.ClosePopup();
        }

        if (_inventUnitScrollView.gameObject.activeSelf)
        {
            gameManager.SortUnitName(_nameAscend);
            _nameAscend = !_nameAscend;
            if (_nameAscend)
            {
                _sortNameText.text = "이름순▼";
            }
            else
            {
                _sortNameText.text = "이름순▲";
            }
            SetUnitInventory();
        }

        if (_inventRoomScrollView.gameObject.activeSelf)
        {
            gameManager.SortRoomName(_levelAscend);
            _levelAscend = !_levelAscend;
            if (_levelAscend)
            {
                _sortNameText.text = "이름순▼";
            }
            else
            {
                _sortNameText.text = "이름순▲";
            }
            SetRoomInventory();
        }
    }

    public void SetRoomInventory()
    {
        // room
        List<Room> playerRooms = gameManager.PlayerRooms;
        foreach (Transform item in _inventRoomContent.transform) // 초기화 관련 ?
        {
            Destroy(item.gameObject);
        }
        for (int i = 0; i < playerRooms.Count; i++)
        {
            InventRoom_ContentsBtnUI inventRoomItems = Main.Get<UIManager>().CreateSubitem<InventRoom_ContentsBtnUI>("InventRoom_ContentsBtnUI", _inventRoomContent);
            inventRoomItems.RoomData = playerRooms[i];
            inventRoomItems.Owner = this;
        }
    }

    public void SetUnitInventory()
    {
        // unit
        List<Character> playerUnits = gameManager.PlayerUnits;
        foreach (Transform item in _inventUnitContent.transform)
        {
            Destroy(item.gameObject);
        }
        for (int i = 0; i < playerUnits.Count; i++)
        {
            InventUnit_ContentsBtnUI inventUnitItems = Main.Get<UIManager>().CreateSubitem<InventUnit_ContentsBtnUI>("InventUnit_ContentsBtnUI", _inventUnitContent);
            inventUnitItems.UnitData = playerUnits[i];
            inventUnitItems.Owner = this;
        }
    }


    //private void ClickBackBtn(PointerEventData EventData)
    //{
    //    Main.Get<UIManager>().CloseAllPopup();
    //    Owner.isInventOpen = false;
    //}

    private void ClickRoomBtn(PointerEventData EventData)
    {
        if (gameManager.isTutorial) // 튜토리얼 중이라면
        {
            if (gameManager.PlayerRooms.Count == 2) return; // 보유 room 이 2 면 버튼 작동 안 되게끔.
        }

        _inventRoomScrollView.gameObject.SetActive(true);
        _inventUnitScrollView.gameObject.SetActive(false);

        if (inventUnitDescri_PopupUI != null) // unit description 창이 켜져있다면
        {
            inventUnitDescri_PopupUI.Owner._selectCheckImg.gameObject.SetActive(false);
            Main.Get<UIManager>().ClosePopup(); // stack 으로 popup 이 쌓임. -> 후입선출 되는 흐름 ! 그래서 ClosePopup 하게 되면 제일 최근에 생성된 UnitDescri_PopupUI 가 닫히는 것이다 !
            inventUnitDescri_PopupUI = null;
        }

        if (!gameManager.isTutorial)
        {
            if (inventUpgrade_PopupUI != null) // 업그레이드창이 켜져있다면
            {
                Main.Get<UIManager>().ClosePopup();
                inventUpgrade_PopupUI = null;
            }
        }
    }

    private void ClickUnitBtn(PointerEventData EventData)
    {
        if (gameManager.isTutorial) // 튜토리얼 중이라면
        {
            if (gameManager.PlayerRooms.Count > 2) return; // room 업그레이드 전에 유닛 버튼 작동 안 되게끔.

            if (gameManager.PlayerRooms.Count == 2 && gameManager.PlayerUnits.Count == 1) return;

            upgradeButton.gameObject.SetActive(true);
            if (tweener.IsActive())
            {
                tweener.Kill();
            }
            inventArrowImg.gameObject.SetActive(false);
        }

        _inventRoomScrollView.gameObject.SetActive(false);
        _inventUnitScrollView.gameObject.SetActive(true);

        if (inventRoomDescri_PopupUI != null) // room description 설명창이 열려 있다면
        {
            inventRoomDescri_PopupUI.Owner._selectCheckImg.gameObject.SetActive(false);
            Main.Get<UIManager>().ClosePopup();
            inventRoomDescri_PopupUI = null;
        }

        if (inventUpgrade_PopupUI != null) // 업그레이드창이 켜져있다면
        {
            Main.Get<UIManager>().ClosePopup();
            inventUpgrade_PopupUI = null;
        }
    }

    private void ClickCloseBtn(PointerEventData EventData)
    {
        Main.Get<UIManager>().CloseAllPopup();
        Owner.isInventOpen = false;
        Camera.main.GetComponent<CameraMovement>().Rock = false;

        if (gameManager.isTutorial) // 인벤토리 닫기 버튼 눌렀을 때
        {
            tweener.Kill(); // 인벤토리 닫기 버튼 가리키던 dotween kill 하고
            inventArrowImg.gameObject.SetActive(false); // 인벤토리 내 화살표 inactive 하고
            upgradeButton.gameObject.SetActive(true); // 인벤토리 내 업그레이드 버튼 활성화.
            Owner.placingButton.gameObject.SetActive(true); // 배치모드 버튼 활성화하고.
            Owner._dayArrowImg.gameObject.SetActive(true); // daymain 의 화살표 활성화.
            Owner.dayArrowTransform.anchoredPosition = new Vector3(-500f, -276f, 0f); // daymain 의 화살표가 배치모드 향하게 하고
            Owner.tweener = Owner.dayArrowTransform.DOAnchorPosY(-306f, animationDuration).SetLoops(-1, LoopType.Yoyo); // daymain 화살표 dotween 걸어주고

            TutorialMsg_PopupUI ui = Main.Get<UIManager>().OpenPopup<TutorialMsg_PopupUI>(); // tutorialpopup - 배치모드 관련해서 튜토리얼팝업 만들어주고
            ui.curTutorialText = "자, 이제 마지막으로 <color=#E9D038><b>배치모드</b></color>에서\nUnit 과 Room 을 배치해봅시다!";
        }
    }

    private void ClickUpgradeBtn(PointerEventData EventData)
    {
        if (inventUnitDescri_PopupUI != null) // unit description 창이 켜져있다면
        {
            inventUnitDescri_PopupUI.Owner._selectCheckImg.gameObject.SetActive(false);
            Main.Get<UIManager>().ClosePopup();
            inventUnitDescri_PopupUI = null;
        }

        if (inventRoomDescri_PopupUI != null) // room description 설명창이 열려 있다면
        {
            inventRoomDescri_PopupUI.Owner._selectCheckImg.gameObject.SetActive(false);
            Main.Get<UIManager>().ClosePopup();
            inventRoomDescri_PopupUI = null;
        }

        if (inventUpgrade_PopupUI == null) // Upgrade 팝업창이 null 일 때만 업그레이드 버튼 누르면 Upgrade 팝업창 뜨도록.
        {
            InventUpgrade_PopupUI ui = Main.Get<UIManager>().OpenPopup<InventUpgrade_PopupUI>("InventUpgrade_PopupUI");
            ui.Owner = this;
        }

        if (gameManager.isTutorial) // 튜토리얼 중이라면
        {
            if (gameManager.PlayerRooms.Count == 4) // Rooom 에서 Upgrade 버튼 비활성화 할 때
            {
                tweener.Kill(); // 인벤토리 업그레이드 버튼 가리키는 화살표 kill.
                inventArrowTransform.anchoredPosition = new Vector3(660f, -30f, 0f); // 보유 Room 가리키는 화살표
                inventArrowTransform.Rotate(0f, 0f, 180f);
                tweener = inventArrowTransform.DOAnchorPosY(0f, animationDuration).SetLoops(-1, LoopType.Yoyo);
                upgradeButton.gameObject.SetActive(false);
            }
        }
    }

    public void InActiveSortButton()
    {
        _sortLeveButton.gameObject.SetActive(false);
        _sortNameButton.gameObject.SetActive(false);
    }
}


