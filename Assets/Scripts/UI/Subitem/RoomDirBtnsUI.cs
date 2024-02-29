using DG.Tweening.Core.Easing;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    private void ClickRightTopBtn(PointerEventData eventData)
    {
        RoomBehavior room = tileManager.SelectRoom;
        tileManager.SetRoomDir(room, ERoomDir.RightTop, !room.IsDoorOpen(ERoomDir.RightTop));
    }

    private void ClickLeftTopBtn(PointerEventData eventData)
    {
        RoomBehavior room = tileManager.SelectRoom;
        tileManager.SetRoomDir(room, ERoomDir.LeftTop, !room.IsDoorOpen(ERoomDir.LeftTop));
    }

    private void ClickRightBottomBtn(PointerEventData eventData)
    {
        RoomBehavior room = tileManager.SelectRoom;
        tileManager.SetRoomDir(room, ERoomDir.RightBottom, !room.IsDoorOpen(ERoomDir.RightBottom));
    }

    private void ClickLeftBottomBtn(PointerEventData eventData)
    {
        RoomBehavior room = tileManager.SelectRoom;
        tileManager.SetRoomDir(room, ERoomDir.LeftBottom, !room.IsDoorOpen(ERoomDir.LeftBottom));
    }
}
