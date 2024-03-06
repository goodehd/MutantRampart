using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitSelectImageUIPanel : BaseUI
{
    private TileManager _tile;
    private GameManager _game;
    private TutorialManager _tuManager;

    private BatRoom _batRoom;

    private Image _unitImage;
    private Image _selectUnitEquipImage;

    private Button _unitSelectButton;

    private TextMeshProUGUI _equipText;

    public Character CharacterData { get; set; }
    public PocketBlock_PopupUI Owner { get; set; }
    
    protected override void Init()
    {
        base.Init();

        _tile = Main.Get<TileManager>();
        _game = Main.Get<GameManager>();
        _tuManager = Main.Get<TutorialManager>();

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

        if (_tutorialManager.isTutorial) // 튜토리얼 중이라면
        {
            if (_game.PlayerUnits[0].CurRoom != null) // 유닛이 장착중이라면
            {
                Main.Get<UIManager>().CloseAllPopup(); // 튜토리얼 창과 포켓팝업UI 끄기.
                Owner.Owner.backButton.gameObject.SetActive(true); // 뒤로가기 버튼 활성화
                _tuManager.KillDOTween(Owner.Owner.tweener);

                _tuManager.SetArrowPosition(Owner.Owner.dayArrowTransform, -720f, 453f); // 뒤로가기 버튼 향하는 화살표
                _tuManager.RotateArrow(Owner.Owner.dayArrowTransform, -180f);
                Owner.Owner.tweener = _tuManager.SetDOTweenX(Owner.Owner.dayArrowTransform, -750f);
                _tuManager.CreateTutorialPopup("T16");

                _tutorialManager.isPlacingTutorialClear = true;
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
