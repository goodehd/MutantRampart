using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoomSelectImage : BaseUI
{
    private Image _roomImage;
    private Image _isEquipedImage;
    private Button _roomSelectButton;
    private Sprite _roomSprite;
    public RoomData RoomData { get; set; }
    public ChangeRoomUI Owner { get; set; }
    
    
    protected override void Init()
    {
        SetUI<Image>();
        SetUI<Button>();

        _roomImage = GetUI<Image>("RoomSelectImage");
        _isEquipedImage = GetUI<Image>("IsEquipedImage");
        _roomSelectButton = GetUI<Button>("RoomSelectImage");

        if(RoomData.isEquiped)
        {
            _isEquipedImage.gameObject.SetActive(true);
        }
        _roomImage.sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.ROOM_SPRITES_PATH}{RoomData.Key}");
        _roomSprite = _roomImage.sprite;
        SetUICallback(_roomSelectButton.gameObject, EUIEventState.Click, SetInfo);
    }

    private void SetInfo(PointerEventData EventData)
    {
        Owner.ChangeRoomData = RoomData;
        Owner.isSelectChangeRoom = true;
    }
    
    
}
