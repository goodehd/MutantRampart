using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeUnit_PopupUI : BaseUI
{
    private Transform _content;
    private Image[] _slots = new Image[3];
    private Button[] _collocateBtn = new Button[3];
    private Button[] _collocateImageBtn = new Button[3];
    private Button[] _deleteBtn = new Button[3];
    private Button _closeBtn;
    public Room SelectRoom { get; set; }
    public UnitSelectImageUI SelectUintImage { get; set; }

    protected override void Init()
    {
        SetUI<Image>();
        SetUI<Button>();
        SetUI<Transform>();

        _content = GetUI<Transform>("Content");

        for(int i = 0; i < _slots.Length; i++)
        {
            _slots[i] = GetUI<Image>($"Slot_{i + 1}");
            _collocateImageBtn[i] = GetUI<Button>($"Slot_{i + 1}");
            _collocateBtn[i] = GetUI<Button>($"CollocateButton{i + 1}");

            if (i < 1)//SelectRoom.RoomData.MaxUnitCount)
            {
                _slots[i].color = Color.white;
            }
        }

        for(int i = 0; i < 3; ++i)
        {
            Character unit = ((BatRoom)SelectRoom).Units[i];
            if(unit != null)
            {
                _slots[i].sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.UNIT_SPRITE_PATH}{unit.Data.Key}");
            }
        }

        _deleteBtn[0] = GetUI<Button>("DelButton1");
        _closeBtn = GetUI<Button>("CloseBtn");

        SetUICallback(_collocateBtn[0].gameObject, EUIEventState.Click, CollocateBtnClick1);
        SetUICallback(_collocateBtn[1].gameObject, EUIEventState.Click, CollocateBtnClick2);
        SetUICallback(_collocateBtn[2].gameObject, EUIEventState.Click, CollocateBtnClick3);
        SetUICallback(_deleteBtn[0].gameObject, EUIEventState.Click, DeleteBtnClick1);
        SetUICallback(_collocateImageBtn[0].gameObject, EUIEventState.Click, SlotImageClick);
        SetUICallback(_closeBtn.gameObject, EUIEventState.Click, CloseBtnClick);

        SetUnitInventory();
    }
    
    private void SetUnitInventory()
    {
        List<CharacterData> playerUnits = Main.Get<GameManager>().playerUnits;
        foreach (Transform item in _content.transform)
        {
            Destroy(item.gameObject);
        }

        for (int i = 0; i < playerUnits.Count; i++)
        {
            UnitSelectImageUI unitSelectImage = Main.Get<UIManager>().CreateSubitem<UnitSelectImageUI>("UnitSelectImage", _content);
            unitSelectImage.CharacterData = playerUnits[i];
            unitSelectImage.Owner = this;
        }
    }

    public void SetCharacterData(UnitSelectImageUI image)
    {
        ResetSelect();
        SelectUintImage = image;
    }

    public void CollocateBtnActive()
    {
        for (int i = 0; i < 1; ++i)
        {
            _collocateBtn[i].gameObject.SetActive(true);
        }
    }

    public void ResetSelect()
    {
        if (SelectUintImage == null)
        {
            return;
        }

        SelectUintImage.CancelCollocate();
        SelectUintImage = null;

        for (int i = 0; i < _collocateBtn.Length; ++i)
        {
            _collocateBtn[i].gameObject.SetActive(false);
        }
    }

    private void CloseBtnClick(PointerEventData EventData)
    {
        Main.Get<UIManager>().ClosePopup();
    }

    private void CollocateBtnClick1(PointerEventData EventData)
    {
        Collocate(0);
    }

    private void CollocateBtnClick2(PointerEventData EventData)
    {
        Collocate(1);
    }

    private void CollocateBtnClick3(PointerEventData EventData)
    {
        Collocate(2);
    }

    private void DeleteBtnClick1(PointerEventData EventData)
    {
        Delete(0);
    }

    private void SlotImageClick(PointerEventData EventData)
    {
        if (((BatRoom)SelectRoom).Units[0] == null)
            return;

        ResetSelect();
        _deleteBtn[0].gameObject.SetActive(true);
    }

    private void Collocate(int index)
    {
        ((BatRoom)SelectRoom).CreateUnit(index, SelectUintImage.CharacterData);
        _slots[index].sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.UNIT_SPRITE_PATH}{SelectUintImage.CharacterData.Key}");
        SelectUintImage.CancelCollocate();
        ResetSelect();
        SetUnitInventory();
    }

    private void Delete(int index)
    {
        ((BatRoom)SelectRoom).DeleteUnit(index);
        _deleteBtn[0].gameObject.SetActive(false);
        _slots[index].sprite = null;
    }
}
