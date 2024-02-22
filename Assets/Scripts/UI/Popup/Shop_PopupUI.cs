using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop_PopupUI : BaseUI
{
    private GameManager gameManager;

    public Button backButton { get; set; }
    private Button _unitButton;
    private Button _roomButton;
    private Button _groundButton;
    private Button _itemButton;
    private Button _gachaUnit1Button;
    private Button _gachaUnit3Button;
    private Button _gachaRoom1Button;
    private Button _gachaRoom3Button;
    private Button _groundRowButton;
    private Button _groundColButton;
    private Button _gachaItem1Button;
    private Button _gachaItem3Button;

    private Transform _unitBtnBox;
    private Transform _roomBtnBox;
    private Transform _groundBtnBox;
    private Transform _itemBtnBox;

    private TMP_Text _playerMoneyText;
    private TMP_Text _groundRowPriceText;
    private TMP_Text _groundColPriceText;

    public Image shopArrowImg { get; set; }

    public RectTransform shopArrowTransform { get; set; }

    public Tweener tweener { get; set; }

    public float animationDuration = 0.3f;

    public DayMain_SceneUI Owner { get; set; }

    public bool isShopTutorialClear { get; set; } = false;


    // 가챠 판매 아이템s
    public List<CharacterData> GachaUnitItems { get; private set; } = new List<CharacterData>();
    public List<RoomData> GachaRoomItems { get; private set; } = new List<RoomData>();
    public List<ItemData> GachaItemItems { get; private set; } = new List<ItemData>();

    // Ground 확장 판매
    public List<ItemData> ShopGroundItems { get; private set; } = new List<ItemData>();

    // 담을 주머니
    public List<CharacterData> _myGachaUnits { get; private set; } = new List<CharacterData>();
    public List<RoomData> _myGachaRooms { get; private set; } = new List<RoomData>();
    public List<ItemData> _myGachaItems { get; private set; } = new List<ItemData>();

    protected override void Init()
    {
        SetUI<Button>();
        SetUI<Transform>();
        SetUI<TMP_Text>();
        SetUI<Image>();

        gameManager = Main.Get<GameManager>();

        backButton = GetUI<Button>("GachaBackBtn");
        _unitButton = GetUI<Button>("GachaShopUnitBtn");
        _roomButton = GetUI<Button>("GachaShopRoomBtn");
        _groundButton = GetUI<Button>("GachaShopGroundBtn");
        _itemButton = GetUI<Button>("GachaShopItemBtn");

        SetUICallback(backButton.gameObject, EUIEventState.Click, ClickCloseBtn);
        SetUICallback(_unitButton.gameObject, EUIEventState.Click, ClickUnitBtn);
        SetUICallback(_roomButton.gameObject, EUIEventState.Click, ClickRoomBtn);
        SetUICallback(_groundButton.gameObject, EUIEventState.Click, ClickGroundBtn);
        SetUICallback(_itemButton.gameObject, EUIEventState.Click, ClickItemBtn);

        _gachaUnit1Button = GetUI<Button>("GachaUnit1Btn");
        _gachaUnit3Button = GetUI<Button>("GachaUnit3Btn");
        _gachaRoom1Button = GetUI<Button>("GachaRoom1Btn");
        _gachaRoom3Button = GetUI<Button>("GachaRoom3Btn");
        _groundRowButton = GetUI<Button>("GroundRowBtn");
        _groundColButton = GetUI<Button>("GroundColBtn");
        _gachaItem1Button = GetUI<Button>("GachaItem1Btn");
        _gachaItem3Button = GetUI<Button>("GachaItem3Btn");

        SetUICallback(_gachaUnit1Button.gameObject, EUIEventState.Click, ClickUnit1Btn);
        SetUICallback(_gachaUnit3Button.gameObject, EUIEventState.Click, ClickUnit3Btn);
        SetUICallback(_gachaRoom1Button.gameObject, EUIEventState.Click, ClickRoom1Btn);
        SetUICallback(_gachaRoom3Button.gameObject, EUIEventState.Click, ClickRoom3Btn);
        SetUICallback(_groundRowButton.gameObject, EUIEventState.Click, ClickExpandRowBtn);
        SetUICallback(_groundColButton.gameObject, EUIEventState.Click, ClickExpandColBtn);
        SetUICallback(_gachaItem1Button.gameObject, EUIEventState.Click, ClickItem1Btn);
        SetUICallback(_gachaItem3Button.gameObject, EUIEventState.Click, ClickItem3Btn);

        _unitBtnBox = GetUI<Transform>("GachaUnitBtnBox");
        _roomBtnBox = GetUI<Transform>("GachaRoomBtnBox");
        _groundBtnBox = GetUI<Transform>("GachaGroundBox");
        _itemBtnBox = GetUI<Transform>("GachaItemBtnBox");

        _playerMoneyText = GetUI<TMP_Text>("GachaPlayerMoneyTxt");
        _playerMoneyText.text = Main.Get<GameManager>().PlayerMoney.ToString();

        _groundRowPriceText = GetUI<TMP_Text>("GroundRowPriceTxt");
        _groundColPriceText = GetUI<TMP_Text>("GroundColPriceTxt");
        UpdateGroundPriceText();

        shopArrowImg = GetUI<Image>("ShopArrowImg");

        shopArrowTransform = shopArrowImg.GetComponent<RectTransform>();

        Main.Get<GameManager>().OnChangeMoney += UpdateMoneyText;

        #region 상점 판매 아이템 추가
        // 가챠 판매 아이템 추가
        if (gameManager.isTutorial) // 튜토리얼 중이라면.
        {
            GachaUnitItems.Add(Main.Get<DataManager>().Character["Warrior"]);
            GachaRoomItems.Add(Main.Get<DataManager>().Room["Forest"]);
        }
        else
        {
            GachaUnitItems.Add(Main.Get<DataManager>().Character["Gun"]);
            GachaUnitItems.Add(Main.Get<DataManager>().Character["Jotem"]);
            GachaUnitItems.Add(Main.Get<DataManager>().Character["Warrior"]);
            GachaUnitItems.Add(Main.Get<DataManager>().Character["Cleric"]);
            GachaUnitItems.Add(Main.Get<DataManager>().Character["Knight"]);
            GachaUnitItems.Add(Main.Get<DataManager>().Character["Kunoichi"]);
            GachaUnitItems.Add(Main.Get<DataManager>().Character["Priest"]);
            GachaUnitItems.Add(Main.Get<DataManager>().Character["Shaman"]);

            GachaRoomItems.Add(Main.Get<DataManager>().Room["Forest"]);
            GachaRoomItems.Add(Main.Get<DataManager>().Room["Igloo"]);
            GachaRoomItems.Add(Main.Get<DataManager>().Room["Lava"]);
            GachaRoomItems.Add(Main.Get<DataManager>().Room["Livingroom"]);
            //GachaRoomItems.Add(Main.Get<DataManager>().Room["Molar"]);
            GachaRoomItems.Add(Main.Get<DataManager>().Room["Snow"]);
            GachaRoomItems.Add(Main.Get<DataManager>().Room["Temple"]);

            GachaItemItems.Add(Main.Get<DataManager>().Item["Feather"]);
            GachaItemItems.Add(Main.Get<DataManager>().Item["TrainingEgg"]);
            GachaItemItems.Add(Main.Get<DataManager>().Item["FrozenTuna"]);
            GachaItemItems.Add(Main.Get<DataManager>().Item["RedBook"]);
            GachaItemItems.Add(Main.Get<DataManager>().Item["BlueBook"]);
            GachaItemItems.Add(Main.Get<DataManager>().Item["GoldenCoin"]);
            GachaItemItems.Add(Main.Get<DataManager>().Item["SilverCoin"]);
            //GachaItemItems.Add(Main.Get<DataManager>().Item["StrangeCandy"]);
            //GachaItemItems.Add(Main.Get<DataManager>().Item["Meat"]);
            GachaItemItems.Add(Main.Get<DataManager>().Item["SilverBar"]);
            GachaItemItems.Add(Main.Get<DataManager>().Item["GoldBar"]);
            // Ground 확장
            ShopGroundItems.Add(Main.Get<DataManager>().Item["ExpandMapRow"]);
            ShopGroundItems.Add(Main.Get<DataManager>().Item["ExpandMapCol"]);
        }

        #endregion

        Owner.shop_PopupUI = this;

        if (gameManager.isTutorial) // 튜토리얼 중이라면.
        {
            backButton.gameObject.SetActive(false);
            _groundButton.gameObject.SetActive(false);
            _itemButton.gameObject.SetActive(false);
            shopArrowImg.gameObject.SetActive(true);
            shopArrowTransform.anchoredPosition = new Vector3(244f, 376f, 0f); // 상점 내 unit 카테고리 가리키는 화살표.
            tweener = shopArrowTransform.DOAnchorPosY(346f, animationDuration).SetLoops(-1, LoopType.Yoyo);
        }
    }

    public void UpdateGroundPriceText()
    {
        int x;
        int y;
        Main.Get<TileManager>().GetMapSize(out x, out y);
        int RowPrice = (Main.Get<DataManager>().Item["ExpandMapRow"].Price) * (y - 2);
        int ColPrice = (Main.Get<DataManager>().Item["ExpandMapCol"].Price) * (x - 2);
        _groundRowPriceText.text = RowPrice.ToString();
        _groundColPriceText.text = ColPrice.ToString();
    }

    private void UpdateMoneyText(int amount)
    {
        _playerMoneyText.text = amount.ToString();
    }

    private void ClickCloseBtn(PointerEventData eventData)
    {
        Main.Get<UIManager>().ClosePopup();

        if (gameManager.isTutorial)
        {
            tweener.Kill(); // 상점 뒤로가기 버튼 가리키는 화살표 Kill.

            Owner.tutorialMsg_PopupUI = Main.Get<UIManager>().OpenPopup<TutorialMsg_PopupUI>();
            Owner.tutorialMsg_PopupUI.curTutorialText = "<color=#E9D038><b>인벤토리</b></color>에서는\n보유하고 있는 Unit 과 Room 에 대한 정보를 확인할 수 있어요.";
        }

        Camera.main.GetComponent<CameraMovement>().Rock = false;

    }

    private void ClickUnitBtn(PointerEventData eventData) // Unit Box 활성화
    {
        if (gameManager.isTutorial)
        {
            if (gameManager.PlayerUnits.Count >= 3)
            {
                return;
            }
            if (gameManager.PlayerUnits.Count == 0)
            {

                tweener.Kill(); // unit 가리키던 화살표 Kill.
            }
            shopArrowImg.gameObject.SetActive(false);
        }
        _unitBtnBox.gameObject.SetActive(true);
        _roomBtnBox.gameObject.SetActive(false);
        _groundBtnBox.gameObject.SetActive(false);
        _itemBtnBox.gameObject.SetActive(false);
    }

    private void ClickRoomBtn(PointerEventData eventData) // Room Box 활성화
    {
        if (gameManager.isTutorial) // 튜토리얼 중이라면 
        {
            if (gameManager.PlayerUnits.Count < 3 || gameManager.PlayerRooms.Count >= 4) return;

            if (gameManager.PlayerUnits.Count >= 3) // 먼저 유닛 3회뽑기 완료했을 때만 Room 버튼 작동되도록.
            {
                shopArrowImg.gameObject.SetActive(false);
                _unitBtnBox.gameObject.SetActive(false);
                _roomBtnBox.gameObject.SetActive(true);
                return;
            }
        }

        _unitBtnBox.gameObject.SetActive(false);
        _roomBtnBox.gameObject.SetActive(true);
        _groundBtnBox.gameObject.SetActive(false);
        _itemBtnBox.gameObject.SetActive(false);
    }

    private void ClickGroundBtn(PointerEventData eventData) // Ground Box 활성화
    {
        _unitBtnBox.gameObject.SetActive(false);
        _roomBtnBox.gameObject.SetActive(false);
        _groundBtnBox.gameObject.SetActive(true);
        _itemBtnBox.gameObject.SetActive(false);
    }

    private void ClickItemBtn(PointerEventData eventData) // Item Box 활성화
    {
        _unitBtnBox.gameObject.SetActive(false);
        _roomBtnBox.gameObject.SetActive(false);
        _groundBtnBox.gameObject.SetActive(false);
        _itemBtnBox.gameObject.SetActive(true);
    }

    private void ClickUnit1Btn(PointerEventData eventData)
    {
        if (gameManager.isTutorial) // 튜토리얼 중이라면
        {
            if (gameManager.PlayerUnits.Count >= 3)
            {
                return;
            }

            ClickGachaUnit(1);
        }
        else
        {
            ClickGachaUnit(1);
        }
    }

    private void ClickUnit3Btn(PointerEventData eventData)
    {
        if (gameManager.isTutorial) // 튜토리얼 중이라면
        {
            if (gameManager.PlayerUnits.Count >= 1)
            {
                return;
            }

            ClickGachaUnit(3);
        }
        else
        {
            ClickGachaUnit(3);
        }
    }

    private void ClickRoom1Btn(PointerEventData eventData)
    {
        if (gameManager.isTutorial) // 튜토리얼 중이라면
        {
            if (gameManager.PlayerRooms.Count >= 4)
            {
                return;
            }

            ClickGachaRoom(1);
        }
        else
        {
            ClickGachaRoom(1);
        }
    }

    private void ClickRoom3Btn(PointerEventData eventData)
    {
        if (gameManager.isTutorial) // 튜토리얼 중이라면
        {
            if (gameManager.PlayerRooms.Count >= 2)
            {
                return;
            }

            ClickGachaRoom(3);
        }
        else
        {
            ClickGachaRoom(3);
        }
    }

    private void ClickExpandRowBtn(PointerEventData eventData)
    {
        YesNo_PopupUI ui = Main.Get<UIManager>().OpenPopup<YesNo_PopupUI>("YesNo_PopupUI");
        ui.curAskingText = "구매하시겠습니까 ?";
        ui.ShopGroundItemData = ShopGroundItems[0]; // 0 이 ExpandMapRow
        ui.Shop_PopupUI = this;
    }

    private void ClickExpandColBtn(PointerEventData eventData)
    {
        YesNo_PopupUI ui = Main.Get<UIManager>().OpenPopup<YesNo_PopupUI>("YesNo_PopupUI");
        ui.curAskingText = "구매하시겠습니까 ?";
        ui.ShopGroundItemData = ShopGroundItems[1]; // 1 이 ExpandMapCol
        ui.Shop_PopupUI= this;
    }

    private void ClickItem1Btn(PointerEventData eventData)
    {
        ClickGachaItem(1);
    }

    private void ClickItem3Btn(PointerEventData eventData)
    {
        ClickGachaItem(3);
    }

    private void ClickGachaUnit(int count)
    {
        if (_myGachaUnits != null) // 이거 안 해주면 버튼 누를때마다 리스트에 계속 쌓여서 1개가 2개가 되고 .. 계속 증가 이슈.
        {
            _myGachaUnits.Clear();
        }

        if (Main.Get<GameManager>().PlayerMoney >= count * 1000) // 금액 계산 로직
        {
            Main.Get<GameManager>().ChangeMoney(-count * 1000);

            for (int i = 0; i < count; i++)
            {
                _myGachaUnits.Add(RandomPickUnit());
            }

            GachaResult_PopupUI ui = Main.Get<UIManager>().OpenPopup<GachaResult_PopupUI>("GachaResult_PopupUI");
            ui.GachaUnitData = _myGachaUnits;
            ui.Owner = this;
        }
        else // 보유 금액 부족 시
        {
            Error_PopupUI errorUI = Main.Get<UIManager>().OpenPopup<Error_PopupUI>("Error_PopupUI");
            errorUI.curErrorText = "돈이 부족해서 구매할 수 없습니다.";
        }
    }

    private CharacterData RandomPickUnit()
    {
        return GachaUnitItems[Random.Range(0, GachaUnitItems.Count)];
    }

    private void ClickGachaRoom(int count)
    {
        if (_myGachaRooms != null)
        {
            _myGachaRooms.Clear();
        }

        if (Main.Get<GameManager>().PlayerMoney >= count * 1000)
        {
            Main.Get<GameManager>().ChangeMoney(-count * 1000);

            for (int i = 0; i < count; i++)
            {
                _myGachaRooms.Add(RandomPickRoom());
            }

            GachaResult_PopupUI ui = Main.Get<UIManager>().OpenPopup<GachaResult_PopupUI>("GachaResult_PopupUI");
            ui.GachaRoomData = _myGachaRooms;
            ui.Owner = this;
        }
        else
        {
            Error_PopupUI errorUI = Main.Get<UIManager>().OpenPopup<Error_PopupUI>("Error_PopupUI");
            errorUI.curErrorText = "돈이 부족해서 구매할 수 없습니다.";
        }
    }

    private RoomData RandomPickRoom()
    {
        return GachaRoomItems[Random.Range(0, GachaRoomItems.Count)];
    }

    private void ClickGachaItem(int count)
    {
        if (_myGachaItems != null)
        {
            _myGachaItems.Clear();
        }

        if (Main.Get<GameManager>().PlayerMoney >= count * 1000)
        {
            Main.Get<GameManager>().ChangeMoney(-count * 1000);


            for (int i = 0; i < count; i++)
            {
                _myGachaItems.Add(RandomPickItem());
            }

            GachaResult_PopupUI ui = Main.Get<UIManager>().OpenPopup<GachaResult_PopupUI>("GachaResult_PopupUI");
            ui.GachaItemData = _myGachaItems;
            ui.Owner = this;
        }
        else
        {
            Error_PopupUI errorUI = Main.Get<UIManager>().OpenPopup<Error_PopupUI>("Error_PopupUI");
            errorUI.curErrorText = "돈이 부족해서 구매할 수 없습니다.";
        }
    }

    private ItemData RandomPickItem()
    {
        return GachaItemItems[Random.Range(0, GachaItemItems.Count)];
    }

}
