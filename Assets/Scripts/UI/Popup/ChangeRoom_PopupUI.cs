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
    private RoomData _selectRoomData;
    private TextMeshProUGUI _roomName;
    private TextMeshProUGUI _roomType;
    private TextMeshProUGUI _roomInstruction;
    private Image _roomImage;
    private Button _equipButton;
    private Button _unequipButton;
    private Button _exitButton;
    private Button _setUnitButton;
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
        _roomImage = GetUI<Image>("RoomImagesprite");
        _content = GetUI<Transform>("Content");
        _equipButton = GetUI<Button>("EquipButton");
        _unequipButton = GetUI<Button>("UnEquipButton");
        _exitButton = GetUI<Button>("Delete");
        _setUnitButton = GetUI<Button>("SetUnitButton");

        SetUICallback(_equipButton.gameObject, EUIEventState.Click, ClickEquipButton);
        SetUICallback(_exitButton.gameObject, EUIEventState.Click, ExitBtnClick);
        SetUICallback(_setUnitButton.gameObject, EUIEventState.Click, SetUnitClick);
        SetUICallback(_unequipButton.gameObject, EUIEventState.Click, ClickUnEquipButton);

        SetMapInventory();
        SetClickRoomData();
        SetSelectRoomInfo(_selectRoomData, _roomImage.sprite);

        if(_selectRoomData.Type != EStatusformat.Bat)
        {
            _setUnitButton.gameObject.SetActive(false);
        }
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
        _selectRoomData = SelectRoom.ThisRoomData;
        _roomImage.sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.ROOM_SPRITES_PATH}{_selectRoomData.Key}");
    }

    private void ClickEquipButton(PointerEventData eventData)
    {
        if (RoomName == "Default")
        {
            if(isSelectChangeRoom)  // 보유하고있는 방을 클릭했을떄
            {
                if (ChangeRoomData.isEquiped)
                {
                    Main.Get<TileManager>().ChangeRoom(ChangeRoomData.indexX, ChangeRoomData.indexY, "Default");
                    ChangeRoom(ChangeRoomData.Key);
                    SetSelectRoomInfo(ChangeRoomData, Main.Get<ResourceManager>().Load<Sprite>($"{Literals.ROOM_SPRITES_PATH}{ChangeRoomData.Key}"));
                    SetActiveSetUnitButton();
                }
                else
                {
                    ChangeRoom(ChangeRoomData.Key);
                    ChangeRoomData.isEquiped = true;
                    SetSelectRoomInfo(ChangeRoomData, Main.Get<ResourceManager>().Load<Sprite>($"{Literals.ROOM_SPRITES_PATH}{ChangeRoomData.Key}"));
                    SetActiveSetUnitButton();
                    Debug.Log("룸 장착");
                }
            }
            
        }
        else
        {
            if (isSelectChangeRoom) // 보유하고있는 방을 클릭했을떄
            {
                if(ChangeRoomData.isEquiped)
                {
                    Error_PopupUI errorPopupUI = Main.Get<UIManager>().OpenPopup<Error_PopupUI>();
                    errorPopupUI.curErrorText = "이미 장착중인 룸입니다 !";
                    Debug.Log("이미 장착중인 룸입니다.");
                }
                else
                {
                    ChangeRoom(ChangeRoomData.Key);
                    ChangeRoomData.isEquiped = true;
                    SetSelectRoomInfo(ChangeRoomData, Main.Get<ResourceManager>().Load<Sprite>($"{Literals.ROOM_SPRITES_PATH}{ChangeRoomData.Key}"));
                    SetActiveSetUnitButton();
                    Debug.Log("룸 변경");
                }
            }
            else
            {
                Debug.Log("룸을 선택해주세요");
            }
        }
        SetMapInventory();
    }

    private void ClickUnEquipButton(PointerEventData eventData)
    {
        if (RoomName == "Default")
        {
            if(isSelectChangeRoom)
            {
                if(ChangeRoomData.isEquiped)
                {
                    Main.Get<TileManager>().ChangeRoom(ChangeRoomData.indexX, ChangeRoomData.indexY, "Default");
                    SetSelectRoomInfo(Main.Get<DataManager>().roomDatas["Default"],
                              Main.Get<ResourceManager>().Load<Sprite>($"{Literals.ROOM_SPRITES_PATH}Default"));
                    ChangeRoomData.isEquiped = false;
                    _setUnitButton.gameObject.SetActive(false);
                }
            }
            Error_PopupUI errorPopupUI = Main.Get<UIManager>().OpenPopup<Error_PopupUI>();
            errorPopupUI.curErrorText = "장착 해제할\n룸이 없습니다 !";
            Debug.Log("장착 해제할 룸이 없습니다");
        }
        else
        {
            ChangeRoomData = SelectRoom.ThisRoomData;
            ChangeRoom("Default");
            SetSelectRoomInfo(Main.Get<DataManager>().roomDatas["Default"],
                              Main.Get<ResourceManager>().Load<Sprite>($"{Literals.ROOM_SPRITES_PATH}Default"));
            ChangeRoomData.isEquiped = false;
            _setUnitButton.gameObject.SetActive(false);
        }
        SetMapInventory();
    }

    private void ExitBtnClick(PointerEventData eventData)
    {
        Main.Get<UIManager>().ClosePopup();
    }

    private void ChangeRoom(string roomDataName)
    {
        SelectRoom.ThisRoomData.isEquiped = false;
        ChangeRoomData.indexX = SelectRoom.IndexX;
        ChangeRoomData.indexY = SelectRoom.IndexY;
        SelectRoom = Main.Get<TileManager>().ChangeRoom(ChangeRoomData.indexX, ChangeRoomData.indexY, roomDataName);
    }

    private void SetMapInventory()
    {
        foreach (Transform item in _content.transform)
        {
            Destroy(item.gameObject);
        }

        for (int i = 0; i < Main.Get<GameManager>().PlayerRooms.Count; i++)
        {
            RoomSelectImageUI roomSelectImage = Main.Get<UIManager>().CreateSubitem<RoomSelectImageUI>("RoomSelectImageUI", _content);
            roomSelectImage.RoomData = Main.Get<GameManager>().PlayerRooms[i];
            roomSelectImage.Owner = this;
            /*if (roomSelectImage.RoomData.isEquiped)
            {
                roomSelectImage.isEquipedImage.gameObject.SetActive(true);
            }*/
        }
        
    }

    private void SetUnitClick(PointerEventData eventData)
    {
        Main.Get<UIManager>().OpenPopup<ChangeUnit_PopupUI>("ChangeUnit_PopUpUI").SelectRoom = SelectRoom;
    }

    private void SetActiveSetUnitButton()
    {
        if (ChangeRoomData.Type != EStatusformat.Bat)
        {
            _setUnitButton.gameObject.SetActive(false);
        }
        else
        {
            _setUnitButton.gameObject.SetActive(true);
        }
    }
}
