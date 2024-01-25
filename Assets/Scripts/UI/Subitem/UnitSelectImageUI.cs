using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitSelectImageUI : BaseUI
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
    public ChangeUnit_PopupUI Owner { get; set; }
    
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
        SetUICallback(_collocateBtn.gameObject, EUIEventState.Click, CollocateClick);
        SetUICallback(_cancelBtn.gameObject, EUIEventState.Click, CancelClick);
        SetUICallback(_unequippedBtn.gameObject, EUIEventState.Click, UnequippedClick);

        if(CharacterData.CurRoom != null)
        {
            _equipText.gameObject.SetActive(true);
            _unequipped.gameObject.SetActive(true);
        }
    }

    private void UnitImageClick(PointerEventData EventData)
    {
        if (_unequipped.IsActive())
            return;

        _collocateImage.gameObject.SetActive(true);
        Owner.SetCharacterData(this);
    }

    private void CollocateClick(PointerEventData EventData)
    {
        _collocateImage.gameObject.SetActive(false);
        _cancelImage.gameObject.SetActive(true);
        Owner.CollocateBtnActive();
    }

    private void CancelClick(PointerEventData EventData)
    {
        CancelCollocate();
        Owner.ResetSelect();
    }

    private void UnequippedClick(PointerEventData EventData)
    {
        _equipText.gameObject.SetActive(false);
        _unequipped.gameObject.SetActive(false);

        Owner.Delete(0, (BatRoom)CharacterData.CurRoom);
    }

    public void CancelCollocate()
    {
        _collocateImage.gameObject.SetActive(false);
        _cancelImage.gameObject.SetActive(false);
    }
}
