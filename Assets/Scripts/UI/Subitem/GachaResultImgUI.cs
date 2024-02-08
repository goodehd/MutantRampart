using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaResultImgUI : BaseUI
{
    private Image _gachaResultItemImg;

    // Data
    public CharacterData GachaUnitData { get; set; }
    public RoomData GachaRoomData { get; set; }
    public ItemData GachaItemData { get; set; }

    protected override void Init()
    {
        SetUI<Image>();

        _gachaResultItemImg = GetUI<Image>("GachaResultImgUI");

        SetSpriteInfo();
    }

    private void SetSpriteInfo()
    {
        if (GachaUnitData != null)
        {
            _gachaResultItemImg.sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.UNIT_SPRITE_PATH}{GachaUnitData.Key}");
        }
        else if (GachaRoomData != null)
        {
            _gachaResultItemImg.sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.ROOM_SPRITES_PATH}{GachaRoomData.Key}");
        }
        else if (GachaItemData != null)
        {
            _gachaResultItemImg.sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.ITEM_SPRITE_PATH}{GachaItemData.Key}");
        }
    }

}
