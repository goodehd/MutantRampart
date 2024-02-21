using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PocketBlock_PopupUI : BaseUI
{
    private GameManager player;
    private TileManager tile;

    public Image _roomScroll;
    public Image _unitScroll; 
    public Image _roomDescription { get; private set; }
    public Image _unitDescription { get; private set; }

    public TextMeshProUGUI _unitName;
    public TextMeshProUGUI _unitHP;
    public TextMeshProUGUI _unitATK;
    public TextMeshProUGUI _unitDEF;
    public TextMeshProUGUI _unitATKSpeed;
    public TextMeshProUGUI _unitSkillDesc;
    public TextMeshProUGUI _roomName;
    public TextMeshProUGUI _roomType;
    public TextMeshProUGUI _roomDescript;
    private Transform _roomContent;
    private Transform _unitContent;

    public bool IsUnit { get; set; }

    public DayMain_SceneUI Owner { get; set; }


    protected override void Init()
    {
        base.Init();
        player = Main.Get<GameManager>();
        tile = Main.Get<TileManager>();

        SetUI<Image>();
        SetUI<Transform>();
        SetUI<TextMeshProUGUI>();

        _roomScroll = GetUI<Image>("PRoom_Scroll View");
        _unitScroll = GetUI<Image>("PUnit_Scroll View");
        _roomDescription = GetUI<Image>("RoomDescriBox");
        _unitDescription = GetUI<Image>("UnitDescriBox");

        _roomContent = GetUI<Transform>("PRoom_Content");
        _unitContent = GetUI<Transform>("PUnit_Content");

        _unitName = GetUI<TextMeshProUGUI>("UnitNameTxt");
        _unitHP = GetUI<TextMeshProUGUI>("UnitHpTxt");
        _unitATK = GetUI<TextMeshProUGUI>("UnitAtkTxt");
        _unitDEF = GetUI<TextMeshProUGUI>("UnitDefTxt");
        _unitATKSpeed = GetUI<TextMeshProUGUI>("UnitAtkSpeedTxt");
        _unitSkillDesc = GetUI<TextMeshProUGUI>("SkillDescText");

        _roomName = GetUI<TextMeshProUGUI>("RoomNameTxt");
        _roomType = GetUI<TextMeshProUGUI>("RoomTypeTxt");
        _roomDescript = GetUI<TextMeshProUGUI>("RoomDescriptionTxt");

        List<Character> playerUnits = player.PlayerUnits;
        List<Room> playerRooms = player.PlayerRooms;

        for(int i = 0; i < playerUnits.Count; i++)
        {
            UnitSelectImageUIPanel charUI = _ui.CreateSubitem<UnitSelectImageUIPanel>("UnitSelectImageUIPanel", _unitContent.transform);
            charUI.CharacterData = playerUnits[i];
            charUI.Owner = this;
        }
        
        for(int i = 0; i < playerRooms.Count; i++)
        {
            RoomSelectImageUI roomSelectImage = _ui.CreateSubitem<RoomSelectImageUI>("RoomSelectImageUI", _roomContent.transform);
            roomSelectImage.Room = Main.Get<GameManager>().PlayerRooms[i];
            roomSelectImage.Owner = this;
        }

        if (IsUnit)
        {
            ToggleContents(true);
        }
        else
        {
            ToggleContents(false);
        }
    }

    public void SetUintInfo(Character data)
    {
        _unitName.text = $"{data.Data.PrefabName}";
        _unitHP.text = $"HP : {data.Status[EstatType.Hp].Value}";
        _unitATK.text = $"ATK : {data.Status[EstatType.Damage].Value}";
        _unitDEF.text = $"DEF : {data.Status[EstatType.Defense].Value}";
        _unitATKSpeed.text = $"ATKSpeed : {data.Status[EstatType.AttackSpeed].Value}";

        if(data.SkillData != null)
            _unitSkillDesc.text = $"Skill : {data.SkillData.Description}";
        else
            _unitSkillDesc.text = $"Skill : None";
    }

    public void SetRoomInfo(Room room)
    {
        _roomName.text = $"{room.Data.Key}";
        _roomType.text = $"{room.Data.Type}";
        _roomDescript.text = $"{room.Data.Instruction}";
    }

    public void ToggleContents(bool isUint)
    {
        if(isUint)
        {
            tile.BatSlot.transform.localPosition = new Vector3(tile.SelectRoom.transform.localPosition.x, 
                tile.SelectRoom.transform.position.y + 0.25f, 2.5f);
            _unitScroll.gameObject.SetActive(true);
            _roomScroll.gameObject.SetActive(false);
            tile.ActiveBatSlot();
        }
        else
        {
            _roomScroll.gameObject.SetActive(true);
            _unitScroll.gameObject.SetActive(false);
            tile.InactiveBatSlot();
        }
    }
}
