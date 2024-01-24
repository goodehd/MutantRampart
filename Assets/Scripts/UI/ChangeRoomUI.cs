using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChangeRoomUI : BaseUI
{
    private RoomData _selectRoomData;
    private TextMeshProUGUI _roomName;
    private TextMeshProUGUI _roomType;
    private TextMeshProUGUI _roomInstruction;
    private TextMeshProUGUI _equipButtonText;
    private Image _roomImage;
    private Button _equipButton;
    private Button _exitButton;
    private Transform _content;
    private bool _isOpenUi = false;
   

    public bool isSelectChangeRoom;
    public Room SelectRoom;
    public RoomData ChangeRoomData;
    public string RoomName;



    private void Awake()
    {
        _isOpenUi = true;
        //Camera.main.GetComponent<Camera>().cullingMask = 6;
    }

    protected override void Init()
    {
        SetUI<TextMeshProUGUI>();
        SetUI<Image>();
        SetUI<Transform>();
        SetUI<Button>();

        _roomName = GetUI<TextMeshProUGUI>("NameText");
        _roomType = GetUI<TextMeshProUGUI>("TypeText");
        _roomInstruction = GetUI<TextMeshProUGUI>("InstructionText");
        _equipButtonText = GetUI<TextMeshProUGUI>("Equip");
        _roomImage = GetUI<Image>("RoomImagesprite");
        _content = GetUI<Transform>("Content");
        _equipButton = GetUI<Button>("EquipButton");
        _exitButton = GetUI<Button>("Delete");

        SetUICallback(_equipButton.gameObject, EUIEventState.Click, ClickEquipButton);
        SetUICallback(_exitButton.gameObject, EUIEventState.Click, ExitBtnClick);

        SetMapInventory();
        SetClickRoomData();
        SetSelectRoomInfo(_selectRoomData, _roomImage.sprite);
    }
    public void SetSelectRoomInfo(RoomData roomData, Sprite sprite)
    {
        _roomName.text = $"이름 : {roomData.Key}";
        _roomType.text = $"타입 : {roomData.Type.ToString()}";
        _roomInstruction.text = $"설명 : {roomData.Instruction}";
        _roomImage.sprite = sprite;
    }
    
    public void SetClickRoomData()
    {
        _selectRoomData = Main.Get<DataManager>().roomDatas[RoomName];
        _roomImage.sprite = Main.Get<ResourceManager>().Load<Sprite>(_selectRoomData.SpritePath);
    }

    private void ClickEquipButton(PointerEventData eventData)
    {
        if (RoomName == "Default")
        {
            if(isSelectChangeRoom)  // 보유하고있는 방을 클릭했을떄
            {
                ChangeRoom(ChangeRoomData.Key);
                Main.Get<GameManager>().PlayerRooms.Remove(ChangeRoomData);
                Main.Get<UIManager>().ClosePopup();
                Debug.Log("룸 장착");
            }
            else
            {
                Main.Get<UIManager>().ClosePopup();
            }
        }
        else
        {
            if (isSelectChangeRoom) // 보유하고있는 방을 클릭했을떄
            {
                ChangeRoom(ChangeRoomData.Key);
                Main.Get<GameManager>().PlayerRooms.Remove(ChangeRoomData);
                Main.Get<GameManager>().PlayerRooms.Add(Main.Get<DataManager>().roomDatas[RoomName]);
                Main.Get<UIManager>().ClosePopup();
                Debug.Log("룸 변경");
            }
            else
            {
                ChangeRoom("Default");
                Main.Get<GameManager>().PlayerRooms.Add(Main.Get<DataManager>().roomDatas[RoomName]);
                Main.Get<UIManager>().ClosePopup();
                Debug.Log("룸 장착 해제");
            }
        }
        SetMapInventory();
    }

    private void ExitBtnClick(PointerEventData eventData)
    {
        Main.Get<UIManager>().ClosePopup();
    }

    private void ChangeRoom(string roomDataName)
    {
        SelectRoom = Main.Get<TileManager>().ChangeRoom(SelectRoom.IndexX, SelectRoom.IndexY, roomDataName);
    }

    private void SetMapInventory()
    {
        foreach (Transform item in _content.transform)
        {
            Destroy(item.gameObject);
        }

        for (int i = 0; i < Main.Get<GameManager>().PlayerRooms.Count; i++)
        {
            /*if (Main.Get<GameManager>().PlayerRooms[i].isEquiped)
            {
                continue;
            }*/
            RoomSelectImage roomSelectImage = Main.Get<UIManager>().CreateSubitem<RoomSelectImage>("RoomSelectImage", _content);
            roomSelectImage.RoomData = Main.Get<GameManager>().PlayerRooms[i];
            roomSelectImage.Owner = this;
        }
    }

}
