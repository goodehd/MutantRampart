using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventRoomDescri_PopupUI : BaseUI
{
    private Button _closeBtn;
    private Button _upgradeBtn;
    private Button _deleteBtn;

    private TMP_Text _roomName;
    private TMP_Text _roomType;
    private TMP_Text _roomDescription;

    private Image _inventRoomImg;

    public ThisRoom RoomData { get; set; }

    public InventRoom_ContentsBtnUI Owner { get; set; }

    //public bool isOpen { get; set; }

    protected override void Init()
    {
        SetUI<Button>();
        SetUI<TMP_Text>();
        SetUI<Image>();

        _closeBtn = GetUI<Button>("InventRoomCloseBtn");
        _upgradeBtn = GetUI<Button>("InventRoomUpgradeBtn");
        _deleteBtn = GetUI<Button>("InventRoomDeleteBtn");

        SetUICallback(_closeBtn.gameObject, EUIEventState.Click, ClickCloseBtn);
        SetUICallback(_upgradeBtn.gameObject, EUIEventState.Click, ClickUpgradeBtn);
        SetUICallback(_deleteBtn.gameObject, EUIEventState.Click, ClickDeleteBtn);

        _roomName = GetUI<TMP_Text>("InventRoomNameTxt");
        _roomType = GetUI<TMP_Text>("InventRoomTypeTxt");
        _roomDescription = GetUI<TMP_Text>("InventRoomDescriTxt");

        _inventRoomImg = GetUI<Image>("InventRoomImg");

        SetInfo();

    }

    private void SetInfo()
    {
        _roomName.text = RoomData.Data.Key;
        _roomType.text = RoomData.Data.Type.ToString();
        _roomDescription.text = RoomData.Data.Instruction;
        _inventRoomImg.sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.ROOM_SPRITES_PATH}{RoomData.Data.Key}");
    }

    private void ClickCloseBtn(PointerEventData EventData)
    {
        Main.Get<UIManager>().ClosePopup();
        Owner._equipCheckImg.gameObject.SetActive(false);
        Owner.isRoomContentPressed = false;
    }

    private void ClickUpgradeBtn(PointerEventData EventData)
    {
        Main.Get<UIManager>().OpenPopup<Upgrade_PopupUI>("Upgrade_PopupUI");

    }

    private void ClickDeleteBtn(PointerEventData EventData)
    {
        // 인벤토리를 껐다가 다시 키면 없어져있긴 하는데 삭제되는 순간에 바로 인벤토리 업데이트까지는 안 됨..
        Main.Get<GameManager>().PlayerRooms.Remove(RoomData);
    }
}
