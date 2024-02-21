using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoomSelectImageUI : BaseUI
{
    private TileManager _tile;
    private DataManager _data;
    private GameManager _game;

    private Image _roomImage;
    private Image _isEquipedImage;
    private Image _selectRoomEquipImage;
    private Button _roomSelectButton;

    public Room Room;
    public PocketBlock_PopupUI Owner { get; set; }

    protected override void Init()
    {
        _tile = Main.Get<TileManager>();
        _data = Main.Get<DataManager>();
        _game = Main.Get<GameManager>();

        SetUI<Image>();
        SetUI<Button>();

        _roomImage = GetUI<Image>("RoomSelectImageUI");
        _isEquipedImage = GetUI<Image>("IsEquipedImage");
        _selectRoomEquipImage = GetUI<Image>("SelectRoomEquipImage");
        _roomSelectButton = GetUI<Button>("RoomSelectImageUI");

        if (Room.IsEquiped)
        {
            _isEquipedImage.gameObject.SetActive(true);
        }

        if (_tile.SelectRoom.RoomInfo == Room)
        {
            _selectRoomEquipImage.gameObject.SetActive(true);
        }
        else
        {
            _selectRoomEquipImage.gameObject.SetActive(false);
        }

        _roomImage.sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.ROOM_SPRITES_PATH}{Room.Data.Key}");
        SetUICallback(_roomSelectButton.gameObject, EUIEventState.Click, ChangeRoom);
        SetUICallback(_roomSelectButton.gameObject, EUIEventState.Hovered, RoomImageHovered);
        SetUICallback(_roomSelectButton.gameObject, EUIEventState.Exit, RoomImageExit);

        Room.OnEquipedEvent += Equiped;
        Room.OnUnEquipedEvent += UnEquiped;
    }

    private void ChangeRoom(PointerEventData EventData)
    {
        if (_game.isTutorial)
        {
            if (Room.Data.Type == EStatusformat.Home && !_game.isHomeSet) // Home 이 배치 안 되어 있을 때 Home 을 배치하려는 경우
            {
                _tile.ChangeRoom(Room);
                Main.Get<GameManager>().tutorialIndexY = 0;

                Main.Get<UIManager>().ClosePopup();
                TutorialMsg_PopupUI ui = Main.Get<UIManager>().OpenPopup<TutorialMsg_PopupUI>(); // tutorialpopup - 배치모드 관련해서 튜토리얼팝업 만들어주고
                ui.curTutorialText = "<color=#E9D038><b>왼쪽 하단의 Ground</b></color> 를 클릭하고\n남은 Room 을 배치해봅시다!";
                if (Owner.Owner.tweener.IsActive())
                {
                    Owner.Owner.tweener.Kill(); // home 타입 가리키는 화살표 Kill.
                }
                Owner.Owner.dayArrowTransform.Rotate(0f, 0f, 90f);
                Owner.Owner._dayArrowImg.gameObject.SetActive(false);

                _tile.GetRoom(1, 0).StartFlashing();
            }
            else if (Room.Data.Type != EStatusformat.Home && _game.isHomeSet && _tile.SelectRoom.RoomInfo.Data.Type != EStatusformat.Bat) // Home 은 배치되어 있지만 배치 타입의 Room 을 배치하려는 경우, 그리고 배치하려는 곳이 Default 일 때 
            {
                _tile.ChangeRoom(Room);
                if (Owner.Owner.tweener.IsActive())
                {
                    Owner.Owner.tweener.Kill();
                }
                Owner.Owner.dayArrowTransform.anchoredPosition = new Vector3(-347f, 324f, 0f); // 열기닫기 버튼 가리키는 화살표.
                Owner.Owner.tweener = Owner.Owner.dayArrowTransform.DOAnchorPosX(-377f, Owner.Owner.animationDuration).SetLoops(-1, LoopType.Yoyo);

                TutorialMsg_PopupUI ui = Main.Get<UIManager>().OpenPopup<TutorialMsg_PopupUI>();
                ui.curTutorialText = "Room 을 배치하고 <color=#E9D038><b>열기/닫기 버튼</b></color>을 통해\nRoom 의 입구를 통제할 수도 있어요.\n\n만약, Home 으로 가는 길이 없다면\nBattle 을 시작할 수 없다는 점 참고해주세요!";
                Owner.Owner.rightBottomButton.gameObject.SetActive(true);
                Owner.Owner.rightTopButton.gameObject.SetActive(true);
                Owner.Owner.leftBottomButton.gameObject.SetActive(true);
                Owner.Owner.leftTopButton.gameObject.SetActive(true);
            }
            return;
        }

        if (_tile.SelectRoom.RoomInfo != Room && Room.IsEquiped)
        {
            return;
        }

        if (_tile.SelectRoom.RoomInfo == Room)
        {
            _tile.ChangeRoom(new Room(_data.Room["Default"]));
            Main.Get<TileManager>().SelectRoom.SortRoom();
        }
        else
        {
            _tile.ChangeRoom(Room);
            Main.Get<TileManager>().SelectRoom.SortRoom();
        }
    }

    private void Equiped()
    {
        _isEquipedImage.gameObject.SetActive(true);
        _selectRoomEquipImage.gameObject.SetActive(true);
    }

    private void UnEquiped()
    {
        _isEquipedImage.gameObject.SetActive(false);
        _selectRoomEquipImage.gameObject.SetActive(false);
    }

    private void RoomImageHovered(PointerEventData eventData)
    {
        Owner.SetRoomInfo(Room);
        Owner._roomDescription.gameObject.SetActive(true);
    }

    private void RoomImageExit(PointerEventData EventData)
    {
        Owner._roomDescription.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        Room.OnEquipedEvent -= Equiped;
        Room.OnUnEquipedEvent -= UnEquiped;
    }
}
