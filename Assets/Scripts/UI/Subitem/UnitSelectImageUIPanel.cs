using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitSelectImageUIPanel : BaseUI
{
    private TileManager _tile;

    private BatRoom _batRoom;

    private Image _unitImage;
    private Image _selectUnitEquipImage;

    private Button _unitSelectButton;

    private TextMeshProUGUI _equipText;

    public Character CharacterData { get; set; }
    public PocketBlock_PopupUI Owner { get; set; }
    
    protected override void Init()
    {
        _tile = Main.Get<TileManager>();   

        SetUI<Image>();
        SetUI<Button>();
        SetUI<TextMeshProUGUI>();

        _unitImage = GetUI<Image>("UnitSelectImageUI");
        _selectUnitEquipImage = GetUI<Image>("SelectUnitEquipImage");
        _unitSelectButton = GetUI<Button>("UnitSelectImageUI");
        _equipText = GetUI<TextMeshProUGUI>("EquipText");

        _unitImage.sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.UNIT_SPRITE_PATH}{CharacterData.Data.Key}");

        SetUICallback(_unitSelectButton.gameObject, EUIEventState.Click, UnitImageClick);
        SetUICallback(_unitSelectButton.gameObject, EUIEventState.Hovered, UnitImageHovered);
        SetUICallback(_unitSelectButton.gameObject, EUIEventState.Exit, UnitImageExit);

        if (CharacterData.CurRoom != null)
        {
            _equipText.gameObject.SetActive(true);
            if (_tile.SelectRoom.RoomInfo == CharacterData.CurRoom.RoomInfo)
            {
                _selectUnitEquipImage.gameObject.SetActive(true);
            }
            else
            {
                _selectUnitEquipImage.gameObject.SetActive(false);
            }
        }

        CharacterData.Ondeploy += ActiveEquip;
        CharacterData.OnRecall += InactiveEquip;
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
            _selectUnitEquipImage.gameObject.SetActive(false);
            _batRoom.SortCharacter();
        }
        else if (CharacterData.CurRoom == null)
        {
            if(_batRoom.CreateUnit(CharacterData))
            {
                _equipText.gameObject.SetActive(true);
                _selectUnitEquipImage.gameObject.SetActive(true);
                _batRoom.SortCharacter();
            }

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

    private void ActiveEquip()
    {
        _equipText.gameObject.SetActive(false);
        _selectUnitEquipImage.gameObject.SetActive(true);
    }

    private void InactiveEquip()
    {
        _equipText.gameObject.SetActive(false);
        _selectUnitEquipImage.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        CharacterData.Ondeploy -= ActiveEquip;
        CharacterData.OnRecall -= InactiveEquip;
    }
}
