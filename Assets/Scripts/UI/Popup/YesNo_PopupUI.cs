using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class YesNo_PopupUI : BaseUI
{
    private TMP_Text _askingText;
    private Button _yesButton;
    private Button _noButton;

    // Data
    public CharacterData ShopUnitData { get; set; }
    public RoomData ShopRoomData { get; set; }
    public ItemData ShopItemData { get; set; }
    public ItemData ShopGroundItemData { get; set; }

    public string curAskingText { get; set; }

    protected override void Init()
    {
        SetUI<TMP_Text>();
        SetUI<Button>();

        _askingText = GetUI<TMP_Text>("YesNoPopupTxt");
        _yesButton = GetUI<Button>("YesBtn");
        _noButton = GetUI<Button>("NoBtn");

        _askingText.text = curAskingText;

        SetUICallback(_yesButton.gameObject, EUIEventState.Click, ClickYesBtn);
        SetUICallback(_noButton.gameObject, EUIEventState.Click, ClickNoBtn);

    }

    private void ClickYesBtn(PointerEventData eventData)
    {
        if (ShopUnitData != null) // 구분 - 구매하려는 데이터가 유닛 일 때
        {
            BuyUnitItem(ShopUnitData);
        }
        else if (ShopRoomData != null) // 구분 - 구매하려는 데이터가 Room 일 때
        {
            BuyRoomItem(ShopRoomData);
        }
        else if (ShopItemData != null) // 구분 - 구매하려는 데이터가 Item 일 때
        {
            BuyItemItem(ShopItemData);
        }
        else if (ShopGroundItemData != null)
        {
            BuyGroundItem(ShopGroundItemData);
        }

        Main.Get<UIManager>().ClosePopup();

    }

    private void ClickNoBtn(PointerEventData eventData)
    {
        Main.Get<UIManager>().ClosePopup();
    }


    //유닛구매
    private void BuyUnitItem(CharacterData data)
    {
        if (Main.Get<GameManager>()._playerMoney >= data.Price)
        {
            Main.Get<GameManager>().ChangeMoney(-data.Price);
            Character newChar = new Character(data);
            newChar.Init(data);
            Main.Get<GameManager>().playerUnits.Add(newChar);
            Debug.Log("구매완료했습니다.");
            Debug.Log($"잔액 : {Main.Get<GameManager>()._playerMoney}");
            //Debug.Log($"playerUnit인벤 : {Main.Get<GameManager>().playerUnits}{data.Key}");
        }
        else // 보유 금액 부족 시
        {
            Error_PopupUI ui = Main.Get<UIManager>().OpenPopup<Error_PopupUI>("Error_PopupUI"); // 돈이 아이템 금액보다 적으면 돈부족 경고창 띄우기
            ui.curErrorText = "돈이 부족해서 구매할 수 없습니다.";
            Debug.Log("돈이 부족해서 구매할 수 없습니다.");
        }
    }

    //Room구매
    private void BuyRoomItem(RoomData data)
    {
        if (Main.Get<GameManager>()._playerMoney >= data.Price)
        {
            Main.Get<GameManager>().ChangeMoney(-data.Price);
            Room newRoom = new Room();
            newRoom.Init(data);
            Main.Get<GameManager>().PlayerRooms.Add(newRoom);
            Debug.Log("구매완료했습니다.");
            Debug.Log($"잔액 : {Main.Get<GameManager>()._playerMoney}");
        }
        else
        {
            Error_PopupUI ui = Main.Get<UIManager>().OpenPopup<Error_PopupUI>("Error_PopupUI"); // 돈이 아이템 금액보다 적으면 돈부족 경고창 띄우기
            ui.curErrorText = "돈이 부족해서 구매할 수 없습니다.";
            Debug.Log("돈이 부족해서 구매할 수 없습니다.");
        }
    }

    //Item구매
    private void BuyItemItem(ItemData data)
    {
        if (Main.Get<GameManager>()._playerMoney >= data.Price)
        {
            Main.Get<GameManager>().ChangeMoney(-data.Price);
            Item newItem = new Item();
            newItem.Init(data);
            Main.Get<GameManager>().PlayerItems.Add(newItem);
            Debug.Log("구매완료했습니다.");
            Debug.Log($"잔액 : {Main.Get<GameManager>()._playerMoney}");
        }
        else
        {
            Error_PopupUI ui = Main.Get<UIManager>().OpenPopup<Error_PopupUI>("Error_PopupUI"); // 돈이 아이템 금액보다 적으면 돈부족 경고창 띄우기
            ui.curErrorText = "돈이 부족해서 구매할 수 없습니다.";
            Debug.Log("돈이 부족해서 구매할 수 없습니다.");
        }
    }

    private void BuyGroundItem(ItemData data)
    {
        if (Main.Get<GameManager>()._playerMoney >= data.Price)
        {
            if(data.Key == "ExpandMapRow")
            {
                Main.Get<TileManager>().ExpandMapRow();
            }
            else if(data.Key == "ExpandMapCol")
            {
                Main.Get<TileManager>().ExpandMapCol();
            }
            Main.Get<GameManager>().ChangeMoney(-data.Price);
            Debug.Log("구매완료했습니다.");
            Debug.Log($"잔액 : {Main.Get<GameManager>()._playerMoney}");
        }
        else
        {
            Error_PopupUI ui = Main.Get<UIManager>().OpenPopup<Error_PopupUI>("Error_PopupUI"); // 돈이 아이템 금액보다 적으면 돈부족 경고창 띄우기
            ui.curErrorText = "돈이 부족해서 구매할 수 없습니다.";
            Debug.Log("돈이 부족해서 구매할 수 없습니다.");
        }
    }
}
