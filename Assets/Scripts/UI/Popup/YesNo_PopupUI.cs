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
    public Shop_PopupUI Shop_PopupUI { get; set; }

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
        Main.Get<UIManager>().ClosePopup();

        if (ShopGroundItemData != null) // 구분 - 구매하려는 데이터가 Ground 일 때
        {
            BuyGroundItem(ShopGroundItemData);
        }

        Shop_PopupUI.UpdateGroundPriceText();
    }

    private void ClickNoBtn(PointerEventData eventData)
    {
        Main.Get<UIManager>().ClosePopup();
    }
    //Ground구매
    private void BuyGroundItem(ItemData data)
    {
        if (Main.Get<GameManager>().PlayerMoney >= data.Price)
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
            Debug.Log($"잔액 : {Main.Get<GameManager>().PlayerMoney}");
        }
        else
        {
            Error_PopupUI ui = Main.Get<UIManager>().OpenPopup<Error_PopupUI>("Error_PopupUI");
            ui.curErrorText = "돈이 부족해서 구매할 수 없습니다.";
            Debug.Log("돈이 부족해서 구매할 수 없습니다.");
        }
    }
}
