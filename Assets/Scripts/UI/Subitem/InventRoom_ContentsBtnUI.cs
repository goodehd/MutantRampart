using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventRoom_ContentsBtnUI : BaseUI
{
    private Image _roomContentsImg;
    private Button _roomContentsBtn;
    private Image _equipCheckImg;

    public ThisRoom RoomData { get; set; }

    //public Inventory_PopupUI Owner { get; set; }

    protected override void Init()
    {
        SetUI<Image>();
        SetUI<Button>();

        _roomContentsImg = GetUI<Image>("InventRoom_ContentsBtnUI");
        _roomContentsBtn = GetUI<Button>("InventRoom_ContentsBtnUI");
        _equipCheckImg = GetUI<Image>("InventRoomEquipCheckImg");

        SetUICallback(_roomContentsBtn.gameObject, EUIEventState.Click, ClickRoomContentBtn);

        SetInfo();
    }

    private void SetInfo()
    {
        _roomContentsImg.sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.ROOM_SPRITES_PATH}{RoomData.Data.Key}");
        //_equipCheckImg = 
        
    }

    private void ClickRoomContentBtn(PointerEventData data)
    {
        // 버튼 누를때마다 창이 여러개 뜬다 !
        Main.Get<UIManager>().OpenPopup<InventRoomDescri_PopupUI>("InventRoomDescri_PopupUI").RoomData = RoomData;
    }
    
}
