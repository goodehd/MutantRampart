using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PocketBlock_PopupUI : BaseUI
{
    private GameManager player;

    public Image _roomScroll;
    public Image _unitScroll; 
    public Image _roomDescription { get; private set; }
    public Image _unitDescription { get; private set; }

    public TextMeshProUGUI _unitName;
    public TextMeshProUGUI _unitHP;
    public TextMeshProUGUI _unitATK;
    public TextMeshProUGUI _unitDEF;
    public TextMeshProUGUI _unitATKSpeed;

    private Transform _roomContent;
    private Transform _unitContent; 

    protected override void Init()
    {
        base.Init();
        player = Main.Get<GameManager>();

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

        List<Character> playerUnits = player.playerUnits;
        for(int i = 0; i < playerUnits.Count; i++)
        {
            UnitSelectImageUI charUI = _ui.CreateSubitem<UnitSelectImageUI>("UnitSelectImageUI", _unitContent.transform);
            charUI.CharacterData = playerUnits[i];
            charUI.Owner = this;
        }

        _unitScroll.gameObject.SetActive(true);
    }

    public void SetUintInfo(Character data)
    {
        _unitName.text = $"{data.Data.Key}";
        _unitHP.text = $"HP : {data.Status[EstatType.Hp].Value}";
        _unitATK.text = $"ATK : {data.Status[EstatType.Damage].Value}";
        _unitDEF.text = $"DEF : {data.Status[EstatType.Defense].Value}";
        _unitATKSpeed.text = $"ATKSpeed : {data.Status[EstatType.AttackSpeed].Value}";
    }
}
