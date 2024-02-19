using System;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GachaResult_PopupUI : BaseUI
{
    private Transform gachaResultContent;
    private Button closeButton;

    // Data
    public List<CharacterData> GachaUnitData { get; set; }
    public List<RoomData> GachaRoomData { get; set; }
    public List<ItemData> GachaItemData { get; set; }
    public List<ItemData> ShopGroundData { get; set; }

    protected override void Init()
    {
        SetUI<Transform>();
        SetUI<Button>();

        gachaResultContent = GetUI<Transform>("GachaResult_Content");

        closeButton = GetUI<Button>("GachaResultCloseBtn");
        SetUICallback(closeButton.gameObject, EUIEventState.Click, ClickCloseBtn);

        SetResultImgUInfo();
    }

    private void ClickCloseBtn(PointerEventData eventData)
    {
        if (GachaUnitData != null) //인벤토리로 옮기려는 데이터가 유닛 일 때
        {
            for (int i = 0; i < GachaUnitData.Count; i++)
            {
                SaveUnitInInventory(GachaUnitData[i]);
            }
        }
        else if (GachaRoomData != null) // Room
        {
            for (int i = 0; i < GachaRoomData.Count; i++)
            {
                SaveRoomInInventory(GachaRoomData[i]);
            }
        }
        else if (GachaItemData != null) // Item
        {
            for (int i = 0; i < GachaItemData.Count; i++)
            {
                SaveItemInInventory(GachaItemData[i]);
            }
        }

        Main.Get<UIManager>().ClosePopup();
    }

    private void SetResultImgUInfo()
    {
        if (GachaUnitData != null)
        {
            for (int i = 0; i < GachaUnitData.Count; i++)
            {
                GachaResultImgUI gachaResultImg = Main.Get<UIManager>().CreateSubitem<GachaResultImgUI>("GachaResultImgUI", gachaResultContent);
                gachaResultImg.GachaUnitData = GachaUnitData[i];
            }
        }
        else if (GachaRoomData != null)
        {
            for (int i = 0; i < GachaRoomData.Count; i++)
            {
                GachaResultImgUI gachaResultImg = Main.Get<UIManager>().CreateSubitem<GachaResultImgUI>("GachaResultImgUI", gachaResultContent);
                gachaResultImg.GachaRoomData = GachaRoomData[i];
            }
        }
        else if (GachaItemData != null)
        {
            for (int i = 0; i < GachaItemData.Count; i++)
            {
                GachaResultImgUI gachaResultImg = Main.Get<UIManager>().CreateSubitem<GachaResultImgUI>("GachaResultImgUI", gachaResultContent);
                gachaResultImg.GachaItemData = GachaItemData[i];
            }
        }
    }

    private void SaveUnitInInventory(CharacterData data)
    {
        Character newChar = new Character(data);
        Main.Get<GameManager>().playerUnits.Add(newChar);
    }

    private void SaveRoomInInventory(RoomData data)
    {
        Room newRoom = new Room(data);
        Main.Get<GameManager>().PlayerRooms.Add(newRoom);
    }

    private void SaveItemInInventory(ItemData data)
    {
        Item newItem = Main.Get<DataManager>().ItemCDO[data.Key].Clone();
        newItem.Init(data);
        Main.Get<GameManager>().PlayerItems.Add(newItem);
        Debug.Log(newItem.GetType().Name);
    }
}
