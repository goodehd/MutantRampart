using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Sell_PopupUI : BaseUI
{
    private TMP_Text _priceText;
    private Button _yesButton;
    private Button _noButton;
    private int price;
    public Inventory_PopupUI Owner { get; set; }
    // Data
    public Character ShopUnitData { get; set; }
    public Room ShopRoomData { get; set; }

    protected override void Init()
    {
        SetUI<TMP_Text>();
        SetUI<Button>();

        _priceText = GetUI<TMP_Text>("PriceTxt");
        _yesButton = GetUI<Button>("YesBtn");
        _noButton = GetUI<Button>("NoBtn");


        SetUICallback(_yesButton.gameObject, EUIEventState.Click, ClickYesBtn);
        SetUICallback(_noButton.gameObject, EUIEventState.Click, ClickNoBtn);

        SetPrice();

    }

    private void ClickYesBtn(PointerEventData eventData)
    {
        Main.Get<UIManager>().ClosePopup();
        Main.Get<UIManager>().ClosePopup();
        if (ShopUnitData != null)
        {
            Main.Get<GameManager>().RemoveUnit(ShopUnitData);
            Owner.SetUnitInventory();
        }
        if (ShopRoomData != null)
        {
            Main.Get<GameManager>().RemoveRoom(ShopRoomData);
            Owner.SetRoomInventory();
            Owner.SetUnitInventory();
        }
        Main.Get<GameManager>().ChangeMoney(price);
    }

    private void ClickNoBtn(PointerEventData eventData)
    {
        Main.Get<UIManager>().ClosePopup();
    }

    private void SetPrice()
    {

        if (ShopUnitData != null)
        {
            price = ShopUnitData.Data.Price;    
        }
        if (ShopRoomData != null)
        {
            price = ShopRoomData.Data.Price;
        }

        _priceText.text = $"판매금액 : {price} gold";
    }
}
