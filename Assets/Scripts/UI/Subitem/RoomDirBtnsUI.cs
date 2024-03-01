using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoomDirBtnsUI : BaseUI
{
    public TileManager tileManager;

    public Button RightTopButton { get; private set; }
    public Button RightBottomButton { get; private set; }
    public Button LeftTopButton { get; private set; }
    public Button LeftBottomButton { get; private set; }
    public DayMain_SceneUI Owner { get; set; }

    protected override void Init()
    {
        base.Init();

        tileManager = Main.Get<TileManager>();

        SetUI<Button>();

        RightTopButton = GetUI<Button>("RightTop");
        RightBottomButton = GetUI<Button>("RightBottom");
        LeftTopButton = GetUI<Button>("LeftTop");
        LeftBottomButton = GetUI<Button>("LeftBottom");

        SetUICallback(RightTopButton.gameObject, EUIEventState.Click, ClickRightTopBtn);
        SetUICallback(RightBottomButton.gameObject, EUIEventState.Click, ClickRightBottomBtn);
        SetUICallback(LeftTopButton.gameObject, EUIEventState.Click, ClickLeftTopBtn);
        SetUICallback(LeftBottomButton.gameObject, EUIEventState.Click, ClickLeftBottomBtn);

        if (_gameManager.isTutorial)
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
        if (_gameManager.isTutorial) return;

        RoomBehavior room = tileManager.SelectRoom;
        tileManager.SetRoomDir(room, ERoomDir.RightTop, !room.IsDoorOpen(ERoomDir.RightTop));
    }

    private void ClickLeftTopBtn(PointerEventData eventData)
    {
        RoomBehavior room = tileManager.SelectRoom;
        tileManager.SetRoomDir(room, ERoomDir.LeftTop, !room.IsDoorOpen(ERoomDir.LeftTop));

        if (_gameManager.isTutorial) // 튜토리얼 중이라면
        {
            CheckBtnForTutorial();

            LeftTopButton.gameObject.SetActive(false); // 다른 버튼은 Init 에서 이미 꺼져있음.

            if (Owner.tweener.IsActive())
            {
                Owner.tweener.Kill(); // 열기닫기 가리키는 화살표 kill.
            }
            Owner.dayArrowTransform.anchoredPosition = new Vector3(860f, 60f, 0f); // unit 버튼 가리키는 화살표
            Owner.dayArrowTransform.Rotate(0f, 0f, 90f);
            Owner.tweener = Owner.dayArrowTransform.DOAnchorPosY(30f, Owner.animationDuration).SetLoops(-1, LoopType.Yoyo);
        }
    }

    private void ClickRightBottomBtn(PointerEventData eventData)
    {
        if (_gameManager.isTutorial) return;

        RoomBehavior room = tileManager.SelectRoom;
        tileManager.SetRoomDir(room, ERoomDir.RightBottom, !room.IsDoorOpen(ERoomDir.RightBottom));
    }

    private void ClickLeftBottomBtn(PointerEventData eventData)
    {
        if (_gameManager.isTutorial) return;

        RoomBehavior room = tileManager.SelectRoom;
        tileManager.SetRoomDir(room, ERoomDir.LeftBottom, !room.IsDoorOpen(ERoomDir.LeftBottom));
    }

    private void CheckBtnForTutorial()
    {
        Main.Get<UIManager>().CloseAllPopup(); // 먼저 떠 있던 튜토리얼 팝업창 및 PocketBlock 닫기

        if (_gameManager.PlayerRooms[0].IsEquiped && _gameManager.PlayerRooms[1].IsEquiped) // 보유한 Room 모두 장착 중일 때
        {
            if (Owner.tutorialMsg_PopupUI == null)
            {
                Owner.tutorialMsg_PopupUI = Main.Get<UIManager>().OpenPopup<TutorialMsg_PopupUI>();
                Owner.tutorialMsg_PopupUI.curTutorialText = Main.Get<DataManager>().Tutorial["T15"].Description;
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
}
