using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

[Flags]
public enum ERoomDir
{
    RightTop = 1 << 0,
    RightBottom = 1 << 1,
    LeftTop = 1 << 2,
    LeftBottom = 1 << 3,
    All = RightTop | RightBottom | LeftTop | LeftBottom
}

public class RoomBehavior : MonoBehaviour
{
    public TilemapRenderer Renderer { get; private set; }
    public Tilemap[] tilemap;

    public GameObject RightWall { get; private set; }
    public GameObject LeftWall { get; private set; }

    public Room RoomInfo { get; set; }
    public ERoomDir RoomDir { get; set; }
    public bool isEndPoint { get; set; }
    public bool isFlashing { get; set; } = false;

    public int IndexX { get { return RoomInfo.IndexX; } set { RoomInfo.IndexX = value;} }
    public int IndexY { get { return RoomInfo.IndexY; } set { RoomInfo.IndexY = value;} }
    public Vector2 Pos { get { return RoomInfo.Pos; } set { RoomInfo.Pos = value; } }
    public LinkedList<CharacterBehaviour> Enemys { get { return RoomInfo.Enemys; } set { RoomInfo.Enemys = value; } }

    private bool _initialize = false;

    private float blinkDuration = 0.4f;

    private Coroutine _flashingCoroutine;
    private TileManager _tile;

    public virtual void Init(RoomData data)
    {

        if (_initialize)
            return;
        tilemap = new Tilemap[4];
        _tile = Main.Get<TileManager>();

        RoomInfo = new Room();
        RoomInfo.Init(data);
        for (int i = 0; i < 2; i++)
        {
            tilemap[i] = transform.GetChild(i).GetComponent<Tilemap>();
        }

        RightWall = transform.GetChild(2).gameObject;
        tilemap[2] = RightWall.GetComponent<Tilemap>();
        LeftWall = transform.GetChild(3).gameObject;
        tilemap[3] = LeftWall.GetComponent<Tilemap>();


        RoomDir = ERoomDir.All;

        _initialize = true;
    }

    public virtual void EnterRoom(Enemy enemy)
    {
        RoomBehavior prevRoom = _tile.GetRoom(enemy.CurPosX, enemy.CurPosY);
        if (prevRoom != null)
        {
            prevRoom.RemoveEnemy(enemy);
        }

        enemy.CurPosX = IndexX;
        enemy.CurPosY = IndexY;
        enemy.CurRoom = this;

        Enemys.AddLast(enemy);

        List<RoomBehavior> neighbors = _tile.GetNeighbors(enemy.CurPosX, enemy.CurPosY, false);

        foreach (RoomBehavior neighbor in neighbors)
        {
            if(neighbor.RoomInfo.Data.Type == EStatusformat.Bat)
            {
                ((BatRoom)neighbor).RangedUnitChangeState(enemy);
            }
        }
    }

    public void SetData(Room data)
    {
        RoomInfo = data;
        data.Owner = this;
    }

    protected virtual void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (Main.Get<TutorialManager>().isTutorial) // 튜토리얼 중이라면 (마우스 호버)
        {
            if (Main.Get<TutorialManager>().isPlacingTutorialClear) return; // 배치모드 튜토리얼 클리어 했다면 타일 호버 안 되도록.

            if (Main.Get<GameManager>().isHomeSet)
            {
                Main.Get<TutorialManager>().tutorialIndexY = 0; // 1,0
            }

            if (IndexX == Main.Get<TutorialManager>().tutorialIndexX && IndexY == Main.Get<TutorialManager>().tutorialIndexY) // 1, 0
            {

            }
            else
            {
                return;
            }
        }

        for (int i = 0; i < tilemap.Length; i++)
        {
            tilemap[i].color = Color.green;
        }

    }

    protected virtual void OnMouseExit()
    {
        for (int i = 0; i < tilemap.Length; i++)
        {
            tilemap[i].color = Color.white;
        }
    }

    protected virtual void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Main.Get<TutorialManager>().isTutorial) // 튜토리얼 중이라면 (마우스 클릭)
        {
            if (Main.Get<TutorialManager>().isPlacingTutorialClear) return; // 배치모드 튜토리얼 클리어 했다면 타일 클릭 안 되도록.

            if (IndexX == Main.Get<TutorialManager>().tutorialIndexX && IndexY == Main.Get<TutorialManager>().tutorialIndexY) // 1, 0
            {

            }
            else
            {
                return;
            }
        }

        if (_tile.SelectRoom != this)
        {
            if (_tile.SelectRoom != null)
                _tile.SelectRoom.StopFlashing();
            _tile.SetSelectRoom(this);
            SortRoom();
        }
    }

    public virtual void OnDestroyRoom()
    {

    }

    public void RemoveEnemy(CharacterBehaviour src)
    {
        Enemys.Remove(src);
    }

    private IEnumerator FlashEffect()
    {
        isFlashing = true;

        while (isFlashing)
        {
            for (int i = 0; i < tilemap.Length; i++)
            {
                tilemap[i].color = Color.green;
            }

            yield return new WaitForSeconds(blinkDuration);

            for (int i = 0; i < tilemap.Length; i++)
            {
                tilemap[i].color = Color.white;
            }

            yield return new WaitForSeconds(blinkDuration);
        }

    }

    public void StartFlashing()
    {
        // 만약 다른 오브젝트에서 작동 중인 코루틴이 있다면 중지
        if (_flashingCoroutine != null)
        {
            StopCoroutine(_flashingCoroutine);
        }
        // 깜빡이는 효과 시작
        _flashingCoroutine = StartCoroutine(FlashEffect());
    }

    public void StopFlashing()
    {
        // 깜빡이는 효과 중지
        if (_flashingCoroutine != null)
        {
            StopCoroutine(_flashingCoroutine);
            _flashingCoroutine = null;
        }
        // 다시 원래 색상으로 되돌리기
        for (int i = 0; i < tilemap.Length; i++)
        {
            tilemap[i].color = Color.white;
        }
        isFlashing = false;
    }

    public void SortRoom()
    {
        Room Room = RoomInfo;

        if (RoomInfo.Data.Key != "Default")
        {
            // PlayerRooms 리스트에서 해당 RoomInfo를 찾음
            int index = Main.Get<GameManager>().PlayerRooms.IndexOf(Room);

            // 찾았으면 리스트에서 제거하고 0번째 인덱스에 추가
            if (index != -1)
            {
                Main.Get<GameManager>().PlayerRooms.RemoveAt(index);
                Main.Get<GameManager>().PlayerRooms.Insert(0, Room);
            }
        }
    }

    public bool IsDoorOpen(ERoomDir direction)
    {
        return (RoomDir & direction) != 0;
    }

    public void OpenDoor(ERoomDir direction)
    {
        RoomDir |= direction;

        if (direction == ERoomDir.RightTop)
        {
            RightWall.SetActive(false);
        }

        if (direction == ERoomDir.LeftTop)
        {
            LeftWall.SetActive(false);
        }
    }

    public void CloseDoor(ERoomDir direction)
    {
        RoomDir &= ~direction;

        if (direction == ERoomDir.RightTop)
        {
            RightWall.SetActive(true);
        }

        if (direction == ERoomDir.LeftTop)
        {
            LeftWall.SetActive(true);
        }
    }

    public void ModifyDoor(ERoomDir direction, bool isOpen)
    {
        if (isOpen)
        {
            OpenDoor(direction);
        }
        else
        {
            CloseDoor(direction);
        }
    }

    public RoomDirSavableData CreateRoomDirSavableData()
    {
        return new RoomDirSavableData(this);
    }
}
