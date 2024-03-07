using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChangeRoom_PopupUI : BaseUI
{
    
    private Transform _content;
    private Button _exitButton;
    
  
    public RoomBehavior SelectRoom;
    public Room SelectChangeRoom;

    

    protected override void Init()
    {
        SetUI<Transform>();
        SetUI<Button>();

        _content = GetUI<Transform>("Content");
        _exitButton = GetUI<Button>("Delete");

        SetUICallback(_exitButton.gameObject, EUIEventState.Click, ExitBtnClick);

        SetMapInventory();
    }

    private void ExitBtnClick(PointerEventData eventData)
    {
        Main.Get<UIManager>().ClosePopup();
    }

    public void SetMapInventory()
    {
        foreach (Transform item in _content.transform)
        {
            Destroy(item.gameObject);
        }

        for (int i = 0; i < Main.Get<GameManager>().PlayerRooms.Count; i++)
        {
            RoomSelectImageUIPanel roomSelectImage = Main.Get<UIManager>().CreateSubitem<RoomSelectImageUIPanel>("RoomSelectImageUI", _content);
            roomSelectImage.Room = Main.Get<GameManager>().PlayerRooms[i];
            //roomSelectImage.Owner = this;
        }
        
    }

    public void EquipRoom(Room changeRoom)
    {
        if (!SelectRoom.RoomInfo.IsEquiped)
        {
            if (!changeRoom.IsEquiped)
            {
                changeRoom.IsEquiped = true;
                //SelectRoom = Main.Get<TileManager>().ChangeRoom(SelectRoom.IndexX, SelectRoom.IndexY, changeRoom);
            }
        }
        else
        {
            if (changeRoom.IsEquiped && changeRoom == SelectRoom.RoomInfo)
            {
                //SelectRoom = Main.Get<TileManager>().ChangeRoomToDefault(SelectRoom.IndexX, SelectRoom.IndexY);
                changeRoom.IsEquiped = false;
            }
        }

        SetMapInventory();
    }

    
}
