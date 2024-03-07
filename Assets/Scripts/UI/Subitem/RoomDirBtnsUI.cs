using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoomDirBtnsUI : BaseUI
{
    private TileManager tileManager;
    private GameManager gameManager;
    public Button RightTopButton { get; private set; }
    public Button RightBottomButton { get; private set; }
    public Button LeftTopButton { get; private set; }
    public Button LeftBottomButton { get; private set; }
    public DayMain_SceneUI Owner { get; set; }

    protected override void Init()
    {
        base.Init();

        tileManager = Main.Get<TileManager>();
        gameManager = Main.Get<GameManager>();

        SetUI<Button>();

        RightTopButton = GetUI<Button>("RightTop");
        RightBottomButton = GetUI<Button>("RightBottom");
        LeftTopButton = GetUI<Button>("LeftTop");
        LeftBottomButton = GetUI<Button>("LeftBottom");

        SetUICallback(RightTopButton.gameObject, EUIEventState.Click, ClickRightTopBtn);
        SetUICallback(RightBottomButton.gameObject, EUIEventState.Click, ClickRightBottomBtn);
        SetUICallback(LeftTopButton.gameObject, EUIEventState.Click, ClickLeftTopBtn);
        SetUICallback(LeftBottomButton.gameObject, EUIEventState.Click, ClickLeftBottomBtn);

        if (_tutorialManager.isTutorial)
        {
            RightTopButton.gameObject.SetActive(false);
            RightBottomButton.gameObject.SetActive(false);
            LeftTopButton.gameObject.SetActive(true); // 왼쪽 상단 버튼만 켜둬 ~
            LeftBottomButton.gameObject.SetActive(false);
        }
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    private void ClickRightTopBtn(PointerEventData eventData)
    {
        if (_tutorialManager.isTutorial) return;

        if (CheckDoorCount(ERoomDir.RightTop))
        {
            RoomBehavior room = tileManager.SelectRoom;
            bool isSetRoomDir = tileManager.SetRoomDir(room, ERoomDir.RightTop, !room.IsDoorOpen(ERoomDir.RightTop));
            if (isSetRoomDir)
            {
                if (room.IsDoorOpen(ERoomDir.RightTop) && gameManager.SetWallCount >= 1)
                {
                    gameManager.SetWallCount -= 1;
                }
                else if (!room.IsDoorOpen(ERoomDir.RightTop))
                {
                    gameManager.SetWallCount += 1;
                }
                Owner.UpdateSetWallCountText();
            }
        }
        else
        {
            OpenNotEnoughPopUp();
        }
    }

    private void ClickLeftTopBtn(PointerEventData eventData)
    {
        if (CheckDoorCount(ERoomDir.LeftTop))
        {
            RoomBehavior room = tileManager.SelectRoom;
            bool isSetRoomDir = tileManager.SetRoomDir(room, ERoomDir.LeftTop, !room.IsDoorOpen(ERoomDir.LeftTop));
            if (isSetRoomDir)
            {
                if (room.IsDoorOpen(ERoomDir.LeftTop) && gameManager.SetWallCount >= 1)
                {
                    gameManager.SetWallCount -= 1;
                }
                else if (!room.IsDoorOpen(ERoomDir.LeftTop))
                {
                    gameManager.SetWallCount += 1;
                }
                Owner.UpdateSetWallCountText();
            }
        }
        else
        {
            OpenNotEnoughPopUp();
        }
        
        if (_tutorialManager.isTutorial) // 튜토리얼 중이라면
        {
            CheckBtnForTutorial();

            LeftTopButton.gameObject.SetActive(false); // 다른 버튼은 Init 에서 이미 꺼져있음.

            _tutorialManager.KillDOTween(); // 유닛배치 룸 가리키는 화살표 kill 로 바뀜.
            _tutorialManager.SetArrowPosition(898f, 60f); // unit 버튼 가리키는 화살표
            _tutorialManager.SetDOTweenY(30f);
            _tutorialManager.RotateArrow(-90f);
        }
    }

    private void ClickRightBottomBtn(PointerEventData eventData)
    {
        if (_tutorialManager.isTutorial) return;

        if (CheckDoorCount(ERoomDir.RightBottom))
        {
            RoomBehavior room = tileManager.SelectRoom;
            bool isSetRoomDir = tileManager.SetRoomDir(room, ERoomDir.RightBottom, !room.IsDoorOpen(ERoomDir.RightBottom));
            if (isSetRoomDir)
            {
                if (room.IsDoorOpen(ERoomDir.RightBottom) && gameManager.SetWallCount >= 1)
                {
                    gameManager.SetWallCount -= 1;
                }
                else if (!room.IsDoorOpen(ERoomDir.RightBottom))
                {
                    gameManager.SetWallCount += 1;
                }
                Owner.UpdateSetWallCountText();
            }
        }
        else
        {
            OpenNotEnoughPopUp();
        }
    }

    private void ClickLeftBottomBtn(PointerEventData eventData)
    {
        if (_tutorialManager.isTutorial) return;

        if (CheckDoorCount(ERoomDir.LeftBottom))
        {
            RoomBehavior room = tileManager.SelectRoom;
            bool isSetRoomDir = tileManager.SetRoomDir(room, ERoomDir.LeftBottom, !room.IsDoorOpen(ERoomDir.LeftBottom));
            if (isSetRoomDir)
            {
                if (room.IsDoorOpen(ERoomDir.LeftBottom) && gameManager.SetWallCount >= 1)
                {
                    gameManager.SetWallCount -= 1;
                }
                else if (!room.IsDoorOpen(ERoomDir.LeftBottom))
                {
                    gameManager.SetWallCount += 1;
                }
                Owner.UpdateSetWallCountText();
            }
        }
        else
        {
            OpenNotEnoughPopUp();
        }
    }

    private void CheckBtnForTutorial()
    {
        Main.Get<UIManager>().CloseAllPopup(); // 먼저 떠 있던 튜토리얼 팝업창 및 PocketBlock 닫기

        if (_gameManager.PlayerRooms[0].IsEquiped && _gameManager.PlayerRooms[1].IsEquiped) // 보유한 Room 모두 장착 중일 때
        {
            if (Owner.tutorialMsg_PopupUI == null)
            {
                _tutorialManager.CreateTutorialPopup("T15");
                _tutorialManager.SetArrowActive(true);
            }

            if (Owner.roomButton.gameObject.activeSelf) // Room 버튼 활성화되어있다면 비활성화 진행.
            {
                Owner.roomButton.gameObject.SetActive(false);
            }
            if (!Owner.unitButton.gameObject.activeSelf) // Unit 버튼 비활성화 되어있다면 활성화 진행.
            {
                Owner.unitButton.gameObject.SetActive(true);
            }
        }
    }

    private void OpenNotEnoughPopUp()
    {
        Error_PopupUI ui = _ui.OpenPopup<Error_PopupUI>();
        ui.curErrorText = "벽을 설치할 수 없습니다.";
        return;
    }

    private bool CheckDoorCount(ERoomDir dir)
    {
        if ((Main.Get<GameManager>().SetWallCount) >= Main.Get<TileManager>().WallLimit &&
            tileManager.SelectRoom.IsDoorOpen(dir))
        {
            return false;
        }
        return true;
    }
}
