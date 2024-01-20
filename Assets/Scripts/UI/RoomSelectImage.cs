using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoomSelectImage : BaseUI
{
    private Image _roomImage;
    private Button _roomSelectButton;
    private Sprite _roomSprite;
    public RoomData RoomData { get; set; }
    public ChangeRoomUI Owner { get; set; }
    
    
    protected override void Init()
    {
        SetUI<Image>();
        SetUI<Button>();

        _roomImage = GetUI<Image>("RoomSelectImage(Clone)");
        _roomSelectButton = GetUI<Button>("RoomSelectImage(Clone)");

        _roomImage.sprite = Main.Get<ResourceManager>().Load<Sprite>(RoomData.SpritePath);
        _roomSprite = _roomImage.sprite;
        SetUICallback(_roomSelectButton.gameObject, EUIEventState.Click, SetInfo);

    }

    private void SetInfo(PointerEventData EventData)
    {
        Owner.SetSelectRoomInfo(RoomData, _roomSprite);

    }
    
    
}
