using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoomSelectImageUI : BaseUI
{
    private TileManager _tile;
    private DataManager _data;

    private Image _roomImage;
    private Image _isEquipedImage;
    private Button _roomSelectButton;

    public ThisRoom Room;
    public PocketBlock_PopupUI Owner;

    protected override void Init()
    {
        _tile = Main.Get<TileManager>();
        _data = Main.Get<DataManager>();

        SetUI<Image>();
        SetUI<Button>();

        _roomImage = GetUI<Image>("RoomSelectImageUI");
        _isEquipedImage = GetUI<Image>("IsEquipedImage");
        _roomSelectButton = GetUI<Button>("RoomSelectImageUI");

        if (Room.IsEquiped)
        {
            _isEquipedImage.gameObject.SetActive(true);
        }
        _roomImage.sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.ROOM_SPRITES_PATH}{Room.Data.Key}");
        SetUICallback(_roomSelectButton.gameObject, EUIEventState.Click, ChangeRoom);
        SetUICallback(_roomSelectButton.gameObject, EUIEventState.Hovered, RoomImageHovered);
        SetUICallback(_roomSelectButton.gameObject, EUIEventState.Exit, RoomImageExit);

        Room.OnEquipedEvenet += Equiped;
        Room.OnUnEquipedEvenet += UnEquiped;
    }

    private void ChangeRoom(PointerEventData EventData)
    {
        if(_tile.SelectRoom.RoomInfo != Room && Room.IsEquiped)
        {
            return;
        }

        if(_tile.SelectRoom.RoomInfo == Room)
        {
            _tile.ChangeRoom(new ThisRoom(_data.Room["Default"]));
        }
        else
        {
            _tile.ChangeRoom(Room);
        }
    }

    private void Equiped()
    {
        _isEquipedImage.gameObject.SetActive(true);
    }

    private void UnEquiped()
    {
        _isEquipedImage.gameObject.SetActive(false);
    }

    private void RoomImageHovered(PointerEventData eventData)
    {
        Owner.SetRoomInfo(Room);
        Owner._roomDescription.gameObject.SetActive(true);
    }

    private void RoomImageExit(PointerEventData EventData)
    {
        Owner._roomDescription.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        Room.OnEquipedEvenet -= Equiped;
        Room.OnUnEquipedEvenet -= UnEquiped;
    }
}
