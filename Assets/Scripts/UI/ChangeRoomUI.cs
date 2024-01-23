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
    
    public Room SelectRoom;
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
        _selectRoomData = roomData;

    }
    
    public void SetClickRoomData()
    {
        _selectRoomData = Main.Get<DataManager>().roomDatas[RoomName];
        _roomImage.sprite = Main.Get<ResourceManager>().Load<Sprite>(_selectRoomData.SpritePath);
    }

    private void ClickEquipButton(PointerEventData eventData)
    {
        if (_selectRoomData.isEquiped == false)
        {
            ChangeRoom(_selectRoomData.Key);
            _selectRoomData.isEquiped = true;
            Main.Get<GameManager>().PlayerRooms.Remove(_selectRoomData);
        }
        else
        {
            ChangeRoom("Default");
            _selectRoomData.isEquiped = false;
            Main.Get<GameManager>().PlayerRooms.Add(_selectRoomData);
        }
        SetMapInventory();
    }

    private void ExitBtnClick(PointerEventData eventData)
    {
        Main.Get<UIManager>().ClosePopup();
    }

    private void OnDestroy()
    {
        _isOpenUi = false;
        //Camera.main.GetComponent<Camera>().cullingMask = 1;
    }

    private void ChangeRoom(string roomDataName)
    {
        SelectRoom = Main.Get<TileManager>().ChangeRoom(SelectRoom.IndexX, SelectRoom.IndexY, roomDataName);
    }

    private void SetMapInventory()
    {
        List<RoomData> playerRooms = Main.Get<GameManager>().PlayerRooms;
        foreach (Transform item in _content.transform)
        {
            Destroy(item.gameObject);
        }

        for (int i = 0; i < playerRooms.Count; i++)
        {
            if (playerRooms[i].isEquiped)
            {
                continue;
            }
            RoomSelectImage roomSelectImage = Main.Get<UIManager>().CreateSubitem<RoomSelectImage>("RoomSelectImage", _content);
            roomSelectImage.RoomData = playerRooms[i];
            roomSelectImage.Owner = this;
        }
    }
}
