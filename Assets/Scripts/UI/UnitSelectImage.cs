using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitSelectImage : BaseUI
{
    private Image _unitImage;
    private Button _unitSelectButton;

    public CharacterData CharacterData { get; set; }
    
    public ChangeUnitUI Owner { get; set; }
    
    protected override void Init()
    {
        SetUI<Image>();
        SetUI<Button>();

        _unitImage = GetUI<Image>("UnitSelectImage");
        _unitSelectButton = GetUI<Button>("UnitSelectImage");

        _unitImage.sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.UNIT_SPRITE_PATH}{CharacterData.Key}");
        SetUICallback(_unitSelectButton.gameObject, EUIEventState.Click, SetInfo);

    }

    private void SetInfo(PointerEventData EventData)
    {
        Owner.SelectUnitData = CharacterData;
        Debug.Log(Owner.SelectUnitData.Key);
    }
    
}
