using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitSelectImageUIPanel : BaseUI
{
    private BatRoom _batRoom;

    private Image _unitImage;

    private Button _unitSelectButton;

    private TextMeshProUGUI _equipText;

    public Character CharacterData { get; set; }
    public PocketBlock_PopupUI Owner { get; set; }
    
    protected override void Init()
    {
        SetUI<Image>();
        SetUI<Button>();
        SetUI<TextMeshProUGUI>();

        _unitImage = GetUI<Image>("UnitSelectImageUI");
        _unitSelectButton = GetUI<Button>("UnitSelectImageUI");
        _equipText = GetUI<TextMeshProUGUI>("EquipText");

        _unitImage.sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.UNIT_SPRITE_PATH}{CharacterData.Data.Key}");

        SetUICallback(_unitSelectButton.gameObject, EUIEventState.Click, UnitImageClick);
        SetUICallback(_unitSelectButton.gameObject, EUIEventState.Hovered, UnitImageHovered);
        SetUICallback(_unitSelectButton.gameObject, EUIEventState.Exit, UnitImageExit);

        if (CharacterData.CurRoom != null)
        {
            _equipText.gameObject.SetActive(true);
        }

    }

    private void UnitImageClick(PointerEventData EventData)
    {
        if(Main.Get<TileManager>().SelectRoom.RoomInfo.Data.Type != EStatusformat.Bat)
            return;

        _batRoom = (BatRoom)Main.Get<TileManager>().SelectRoom;
        if (_batRoom == CharacterData.CurRoom)
        {
            _batRoom.DeleteUnit(CharacterData);
            _equipText.gameObject.SetActive(false);
        }
        else if (CharacterData.CurRoom == null)
        {
            if(_batRoom.CreateUnit(CharacterData))
                _equipText.gameObject.SetActive(true);
        }
    }

    private void UnitImageHovered(PointerEventData EventData)
    {
        Owner.SetUintInfo(CharacterData);
        Owner._unitDescription.gameObject.SetActive(true);
    }

    private void UnitImageExit(PointerEventData EventData)
    {
        Owner._unitDescription.gameObject.SetActive(false);
    }
}
