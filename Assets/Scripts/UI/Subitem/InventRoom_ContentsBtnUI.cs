using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventRoom_ContentsBtnUI : BaseUI
{
    private Image _roomContentsImg;
    private Button _roomContentsBtn;
    public Image _equipCheckImg { get; private set; }

    public ThisRoom RoomData { get; set; }

    public bool isRoomContentPressed { get; set; }

    //public Inventory_PopupUI Owner { get; set; }

    protected override void Init()
    {
        SetUI<Image>();
        SetUI<Button>();

        _roomContentsImg = GetUI<Image>("InventRoom_ContentsBtnUI");
        _roomContentsBtn = GetUI<Button>("InventRoom_ContentsBtnUI");
        _equipCheckImg = GetUI<Image>("InventRoomEquipCheckImg");

        SetUICallback(_roomContentsBtn.gameObject, EUIEventState.Click, ClickRoomContentBtn);
        SetUICallback(_roomContentsBtn.gameObject, EUIEventState.Hovered, HoveredUnitContentBtn);
        SetUICallback(_roomContentsBtn.gameObject, EUIEventState.Exit, ExitUnitContentBtn);

        SetInfo();
    }

    private void SetInfo()
    {
        _roomContentsImg.sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.ROOM_SPRITES_PATH}{RoomData.Data.Key}");        
    }

    private void ClickRoomContentBtn(PointerEventData data)
    {
        if (isRoomContentPressed) // 버튼을 눌러 설명창이 기존에 열려있다면 리턴시키고,
        {
            return;
        }
        else if (!isRoomContentPressed) // 버튼을 눌러 설명창이 기존에 안 열려있다면
        {
            _equipCheckImg.gameObject.SetActive(true);
            isRoomContentPressed = true;

            InventRoomDescri_PopupUI ui = Main.Get<UIManager>().OpenPopup<InventRoomDescri_PopupUI>("InventRoomDescri_PopupUI"); // 설명창 열어주고
            ui.RoomData = RoomData; // 데이터 넘겨주고
            ui.Owner = this; // owner 설정해주고

            //Main.Get<UIManager>().OpenPopup<InventRoomDescri_PopupUI>("InventRoomDescri_PopupUI").isOpen = true;

        }
    }

    private void HoveredUnitContentBtn(PointerEventData data)
    {
        _roomContentsImg.color = Color.cyan;
    }

    private void ExitUnitContentBtn(PointerEventData data)
    {
        _roomContentsImg.color = Color.white;
    }

}
