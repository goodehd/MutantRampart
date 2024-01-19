using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoomSelectImage : BaseUI
{
    public RoomData roomData { get; set; }
    private Image roomImage;
    private Button roomselectButton;
    private Sprite roomSprite;
    public ChangeRoomUI owner { get; set; }
    
    
    protected override void Init()
    {
        SetUI<Image>();
        SetUI<Button>();

        roomImage = GetUI<Image>("RoomSelectImage(Clone)");
        roomselectButton = GetUI<Button>("RoomSelectImage(Clone)");

        roomImage.sprite = Main.Get<ResourceManager>().Load<Sprite>(roomData.SpritePath);
        roomSprite = roomImage.sprite;
        SetUICallback(roomselectButton.gameObject, EUIEventState.Click, SetInfo);

    }

    private void SetInfo(PointerEventData EventData)
    {
        owner.SetSelectRoomInfo(roomData, roomSprite);

    }
    
    
}
