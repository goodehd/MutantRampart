using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitSelectImageUIPanel : BaseUI
{
    private Image _unitImage;
    private Image _collocateImage;
    private Image _cancelImage;
    private Image _unequipped;

    private Button _unitSelectButton;
    private Button _collocateBtn;
    private Button _cancelBtn;
    private Button _unequippedBtn;

    private TextMeshProUGUI _equipText;

    public Character CharacterData { get; set; }
    public PocketBlock_PopupUI Owner { get; set; }
    
    protected override void Init()
    {
        SetUI<Image>();
        SetUI<Button>();
        SetUI<TextMeshProUGUI>();

        _unitImage = GetUI<Image>("UnitSelectImageUI");
        _collocateImage = GetUI<Image>("ButtonPanel");
        _cancelImage = GetUI<Image>("CancelPanel");
        _unequipped = GetUI<Image>("UnequippedPanel");

        _unitSelectButton = GetUI<Button>("UnitSelectImageUI");
        _collocateBtn = GetUI<Button>("SetButton");
        _cancelBtn = GetUI<Button>("CancelButton");
        _unequippedBtn = GetUI<Button>("UnequippedButton");

        _equipText = GetUI<TextMeshProUGUI>("EquipText");

        _unitImage.sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.UNIT_SPRITE_PATH}{CharacterData.Data.Key}");

        SetUICallback(_unitSelectButton.gameObject, EUIEventState.Click, UnitImageClick);
        SetUICallback(_unitSelectButton.gameObject, EUIEventState.Hovered, UnitImageHovered);
        SetUICallback(_unitSelectButton.gameObject, EUIEventState.Exit, UnitImageExit);

        if (CharacterData.CurRoom != null)
        {
            _equipText.gameObject.SetActive(true);
            _unequipped.gameObject.SetActive(true);
        }
    }

    private void UnitImageClick(PointerEventData EventData)
    {

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
