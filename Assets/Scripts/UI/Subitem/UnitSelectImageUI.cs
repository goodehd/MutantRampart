using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitSelectImageUI : BaseUI
{
    private Image _unitImage;
    private Image _collocateImage;
    private Image _cancelImage;

    private Button _unitSelectButton;
    private Button _collocateBtn;
    private Button _cancelBtn;

    public CharacterData CharacterData { get; set; }
    public ChangeUnit_PopupUI Owner { get; set; }
    
    protected override void Init()
    {
        SetUI<Image>();
        SetUI<Button>();

        _unitImage = GetUI<Image>("UnitSelectImage");
        _collocateImage = GetUI<Image>("ButtonPanel");
        _cancelImage = GetUI<Image>("CancelPanel");

        _unitSelectButton = GetUI<Button>("UnitSelectImage");
        _collocateBtn = GetUI<Button>("SetButton");
        _cancelBtn = GetUI<Button>("CancelButton");

        _unitImage.sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.UNIT_SPRITE_PATH}{CharacterData.Key}");
        SetUICallback(_unitSelectButton.gameObject, EUIEventState.Click, UnitImageClick);
        SetUICallback(_collocateBtn.gameObject, EUIEventState.Click, CollocateClick);
        SetUICallback(_cancelBtn.gameObject, EUIEventState.Click, CancelClick);
    }

    private void UnitImageClick(PointerEventData EventData)
    {
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

    public void CancelCollocate()
    {
        _collocateImage.gameObject.SetActive(false);
        _cancelImage.gameObject.SetActive(false);
    }
}
