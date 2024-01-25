using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class BuyConfirm_PopupUI : BaseUI
{
    private Button _yesButton;
    private Button _noButton;

    // Data
    public CharacterData ShopUnitData { get; set; }
    public RoomData ShopRoomData { get; set; }

    //public bool isUnitItem;
    //public bool isRoomItem;
    //private Room_List Room_List;
    //private Unit_List Unit_List;


    protected override void Init()
    {
        SetUI<Button>();

        _yesButton = GetUI<Button>("YesBtn");
        _noButton = GetUI<Button>("NoBtn");

        SetUICallback(_yesButton.gameObject, EUIEventState.Click, ClickYesBtn);
        SetUICallback(_noButton.gameObject, EUIEventState.Click, ClickNoBtn);

    }

    private void ClickYesBtn(PointerEventData eventData)
    {
        // todo: 구분 - 구매하는 데이터가 유닛 일 때
        //if ()
        //{
        //    BuyUnitItem(ShopUnitData);
        //}
        //else if () // todo : 구분 - 구매하는 데이터가 Room 일 때
        //{
        //    BuyRoomItem(ShopRoomData);
        //}

        Main.Get<UIManager>().ClosePopup();

    }

    private void ClickNoBtn(PointerEventData eventData)
    {
        Main.Get<UIManager>().ClosePopup();
    }


    // 플레이어 골드를 활용해서, ChangeMoney 함수 ...
    //유닛구매
    private void BuyUnitItem(CharacterData data)
    {
        if (Main.Get<GameManager>()._playerMoney >= data.Price)
        {
            Main.Get<GameManager>().ChangeMoney(-data.Price);
            Main.Get<GameManager>().playerUnits.Add(data); // 얕은복사이슈발생할수도
            Debug.Log("구매완료했습니다.");
            Debug.Log($"잔액 : {Main.Get<GameManager>()._playerMoney}");
        }
        else // 보유 금액 부족 시
        {
            Main.Get<UIManager>().OpenPopup<MoneyError_PopupUI>("MoneyError_PopupUI"); // 돈이 아이템 금액보다 적으면 돈부족 경고창 띄우기
            Debug.Log("돈이 부족해서 구매할 수 없습니다.");
        }
    }

    //Room구매
    private void BuyRoomItem(RoomData data)
    {
        if (Main.Get<GameManager>()._playerMoney >= data.Price)
        {
            Main.Get<GameManager>().ChangeMoney(-data.Price);
            Main.Get<GameManager>().PlayerRooms.Add(data);
            Debug.Log("구매완료했습니다.");
            Debug.Log($"잔액 : {Main.Get<GameManager>()._playerMoney}");
        }
        else
        {
            Main.Get<UIManager>().OpenPopup<MoneyError_PopupUI>("MoneyError_PopupUI"); // 돈이 아이템 금액보다 적으면 돈부족 경고창 띄우기
            Debug.Log("돈이 부족해서 구매할 수 없습니다.");
        }
    }
}
