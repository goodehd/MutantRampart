using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : IManagers
{
    private ResourceManager resource;
    private NavigationTile _navigation;

    public GameObject GridObject;
    public List<List<RoomBehavior>> _roomObjList = new List<List<RoomBehavior>>();

    public SpawnTile SpawnTile { get; private set; }
    public BatPoint BatSlot { get; set; }

    public RoomBehavior SelectRoom { get; private set; }
    public RoomBehavior PrevSelectRoom { get; set; }
    public event Action OnSelectRoomEvent;

    public int WallLimit;

    public void GenerateMap(int x, int y)
    {
        GridObject = new GameObject("Tile");

        // 그리드를 생성하고 Grid 컴포넌트를 추가
        Grid gridComponent = GridObject.AddComponent<Grid>();

        // Cell Size 및 Cell Gap 설정
        gridComponent.cellSize = new Vector3(1, 0.5f, 1); // x = 1, y = 0.5, z = 1
        gridComponent.cellGap = new Vector3(0, 0, 0); // x = 0, y = 0, z = 0

        // Cell Layout 설정
        gridComponent.cellLayout = GridLayout.CellLayout.IsometricZAsY; // Isometric Z As Y

        // Cell Swizzle 설정
        gridComponent.cellSwizzle = GridLayout.CellSwizzle.XYZ; // XYZ

        Vector2 pos = Vector2.zero;
        Vector2 offset = Vector2.zero;

        _roomObjList.Clear();
        for (int i = 0; i < x; i++)
        {
            _roomObjList.Add(new List<RoomBehavior>());

            pos.Set(-3f * i, 1.5f * i);
            for (int j = 0; j < y; j++)
            {
                offset.Set(3f * j, 1.5f * j);
                RoomBehavior room = CreateDefaultRoom(i, j, pos + offset);
                _roomObjList[i].Add(room);
            }
        }

        GameObject spawn = resource.Instantiate("Prefabs/Room/SpawnTile", GridObject.transform);
        spawn.transform.position = new Vector3(-6f, 0, 0);
        SpawnTile = spawn.GetComponent<SpawnTile>();

        BatSlot = resource.Instantiate("Prefabs/Room/BatPoint", GridObject.transform).GetComponent<BatPoint>();

        _navigation.CreateNavigation(_roomObjList);
    }

    public void ExpandMapRow()
    {
        Vector2 pos = Vector2.zero;
        Vector2 offset = Vector2.zero;
        GetMapSize(out int x, out int y);
        _roomObjList.Add(new List<RoomBehavior>());
        pos.Set(-3f * x, 1.5f * x);
        for (int i = 0; i < y; i++)
        {
            offset.Set(3f * i, 1.5f * i);
            RoomBehavior room = CreateDefaultRoom(x, i, pos + offset);
            _roomObjList[_roomObjList.Count - 1].Add(room);
        }
        _navigation.ExpandNodeRow();
    }

    public void ExpandMapCol()
    {
        Vector2 pos = Vector2.zero;
        Vector2 offset = Vector2.zero;
        GetMapSize(out int x, out int y);
        pos.Set(3f * y, 1.5f * y);
        for (int i = 0; i < x; i++)
        {
            offset.Set(-3f * i, 1.5f * i);
            RoomBehavior room = CreateDefaultRoom(i, y, pos + offset);
            _roomObjList[i].Add(room);
        }
        _navigation.ExpandNodeCol();
    }

    public bool Init()
    {
        resource = Main.Get<ResourceManager>();
        _navigation = new NavigationTile();

        UpdateWallCount();
        
        return true;
    }

    public void SetSelectRoom(RoomBehavior room)
    {
        if (SelectRoom == room)
            return;

        SelectRoom = room;
        if(room != null)
            OnSelectRoomEvent?.Invoke();
    }

    public RoomBehavior ChangeRoom(Room changeRoom)
    {
        GameObject obj = Main.Get<SceneManager>().Scene.CreateRoom(changeRoom.Data.Key);
        obj.transform.position = SelectRoom.transform.position;
        obj.transform.parent = GridObject.transform;

        RoomBehavior room = obj.GetComponent<RoomBehavior>();
        room.SetData(changeRoom);
        room.IndexX = SelectRoom.IndexX;
        room.IndexY = SelectRoom.IndexY;
        room.Pos = SelectRoom.Pos;
        room.RoomInfo.EquipedRoom();

        _roomObjList[SelectRoom.IndexX][SelectRoom.IndexY] = room;

        SelectRoom.RoomInfo.UnEquipedRoom();
        SelectRoom.OnDestroyRoom();
        resource.Destroy(SelectRoom.gameObject);

        room.RoomDir = SelectRoom.RoomDir;

        SelectRoom = room;
        SelectRoom.StartFlashing();
        SetCheckWall(SelectRoom);

        return room;
    }

    public void SetCheckWall(RoomBehavior room)
    {
        foreach (ERoomDir dir in Enum.GetValues(typeof(ERoomDir)))
        {
            if (dir > ERoomDir.LeftBottom)
            {
                break;
            }

            SetRoomDir(room, dir, room.IsDoorOpen(dir));
        }
    }

    public List<RoomBehavior> GetNeighbors(int curPosX, int curPosY, bool isCheckDoorOpen = true)
    {
        List<RoomBehavior> outPut = new List<RoomBehavior>();

        int[] dx = { 0, -1, 1, 0 };
        int[] dy = { 1, 0, 0, -1 };

        for (int i = 0; i < 4; ++i)
        {
            if (!IsRoomPositionValid(curPosX + dx[i], curPosY + dy[i]))
            {
                continue;
            }

            RoomBehavior room = _roomObjList[curPosX][curPosY];
            if (isCheckDoorOpen && !room.IsDoorOpen((ERoomDir)(1 << i)))
            {
                continue;
            }

            outPut.Add(_roomObjList[curPosX + dx[i]][curPosY + dy[i]]);
        }
        return outPut;
    }

    public RoomBehavior GetRoom(int x, int y)
    {
        if (!IsRoomPositionValid(x, y))
        {
            return null;
        }
        return _roomObjList[x][y];
    }

    public void GetMapSize(out int mapSizeX, out int mapSizeY)
    {
        mapSizeX = _roomObjList.Count;
        mapSizeY = _roomObjList[0].Count;
    }

    public void ActiveBatSlot()
    {
        BatSlot.gameObject.SetActive(true);
        BatSlot.transform.position = SelectRoom.transform.position;
        BatSlot.SetSlotColor(SelectRoom.RoomInfo.Data.MaxUnitCount);
    }

    public void InactiveBatSlot()
    {
        BatSlot.gameObject.SetActive(false);
    }

    public Stack<RoomBehavior> FindUnvisitedRoom(int x, int y, bool[,] visited) 
    { 
        return _navigation.FindUnvisitedRoom(x, y, visited);
    }

    public bool FindPath(Vector2 start, Vector2 end, out Stack<Vector2> stackPath)
    {
        return _navigation.FindPath(start, end, out stackPath);
    }

    public bool SetRoomDir(RoomBehavior room, ERoomDir dir, bool isOpen)
    {
        RoomBehavior Neighbor = null;
        switch (dir)
        {
            case ERoomDir.RightTop:
                Neighbor = GetRoom(room.IndexX, room.IndexY + 1);
                if (Neighbor == null)
                {
                    room.OpenDoor(ERoomDir.RightTop);
                    return false;
                }
                Neighbor.ModifyDoor(ERoomDir.LeftBottom, isOpen);
                break;
            case ERoomDir.RightBottom:
                Neighbor = GetRoom(room.IndexX - 1, room.IndexY);
                if (Neighbor == null)
                {
                    room.OpenDoor(ERoomDir.RightBottom);
                    return false;
                }
                Neighbor.ModifyDoor(ERoomDir.LeftTop, isOpen);
                break;
            case ERoomDir.LeftTop:
                Neighbor = GetRoom(room.IndexX + 1, room.IndexY);
                if (Neighbor == null)
                {
                    room.OpenDoor(ERoomDir.LeftTop);
                    return false;
                }
                Neighbor.ModifyDoor(ERoomDir.RightBottom, isOpen);
                break;
            case ERoomDir.LeftBottom:
                Neighbor = GetRoom(room.IndexX, room.IndexY - 1);
                if (Neighbor == null)
                {
                    room.OpenDoor(ERoomDir.LeftBottom);
                    return false;
                }
                Neighbor.ModifyDoor(ERoomDir.RightTop, isOpen);
                break;
        }
        room.ModifyDoor(dir, isOpen);
        _navigation.SetCheckWall(room);
        return true;
    }

    private bool IsRoomPositionValid(int posX, int posY)
    {
        if (posX < 0 || posX >= _roomObjList.Count)
        {
            return false;
        }
        if (posY < 0 || posY >= _roomObjList[posX].Count)
        {
            return false;
        }
        return true;
    }

    private RoomBehavior CreateDefaultRoom(int x, int y, Vector2 pos)
    {
        GameObject obj = Main.Get<SceneManager>().Scene.CreateRoom("Default");
        obj.transform.position = pos;
        obj.transform.parent = GridObject.transform;
        RoomBehavior room = obj.GetComponent<RoomBehavior>();
        room.IndexX = x;
        room.IndexY = y;
        room.Pos = pos;
        room.Pos = obj.transform.position;

        return room;
    }

    public void DeleteRoom(Room room)
    {
        RoomBehavior newRoom = CreateDefaultRoom(room.IndexX, room.IndexY, _roomObjList[room.IndexX][room.IndexY].transform.position);
        _roomObjList[room.IndexX][room.IndexY].OnDestroyRoom();

        if(room.Owner != null)
        {
            newRoom.RoomDir = room.Owner.RoomDir;
        }

        resource.Destroy(_roomObjList[room.IndexX][room.IndexY].gameObject);
        _roomObjList[room.IndexX][room.IndexY] = newRoom;

        SetCheckWall(newRoom);
    }

    public void DestroyRoom(int x, int y)
    {
        resource.Destroy(_roomObjList[x][y].gameObject);
    }

    public void SetRoomDir(int x, int y, ERoomDir roomDir)
    {
        _roomObjList[x][y].RoomDir = roomDir;
        SetCheckWall(_roomObjList[x][y]);
    }

    public void RoomMove()
    {
        ERoomDir prevRoomDir = PrevSelectRoom.RoomDir;
        Vector3 prevRoomPos = PrevSelectRoom.transform.position;
        int prevIndexX = PrevSelectRoom.IndexX;
        int prevIndexY = PrevSelectRoom.IndexY;

        ERoomDir curRoomDir = SelectRoom.RoomDir;
        Vector3 curRoomPos = SelectRoom.transform.position;
        int curIndexX = SelectRoom.IndexX;
        int curIndexY = SelectRoom.IndexY;

        PrevSelectRoom.transform.position = curRoomPos;
        SelectRoom.transform.position = prevRoomPos;

        PrevSelectRoom.RoomDir = curRoomDir;
        SelectRoom.RoomDir = prevRoomDir;

        _roomObjList[prevIndexX][prevIndexY] = SelectRoom;
        SelectRoom.IndexX = prevIndexX;
        SelectRoom.IndexY = prevIndexY;
        SelectRoom.RoomInfo.Pos = prevRoomPos;

        _roomObjList[curIndexX][curIndexY] = PrevSelectRoom;
        PrevSelectRoom.IndexX = curIndexX;
        PrevSelectRoom.IndexY = curIndexY;
        PrevSelectRoom.RoomInfo.Pos = curRoomPos;

        if (SelectRoom.RoomInfo.Data.Type == EStatusformat.Bat)
        {
            ((BatRoom)SelectRoom).SetUnitPos();
        }

        if (PrevSelectRoom.RoomInfo.Data.Type == EStatusformat.Bat)
        {
            ((BatRoom)PrevSelectRoom).SetUnitPos();
        }

        SetCheckWall(SelectRoom);
        SetCheckWall(PrevSelectRoom);

        SelectRoom = PrevSelectRoom;
        PrevSelectRoom = null;
    }

    public void UpdateWallCount()
    {
        WallLimit = 3; // 기본 설치가능한 벽의 개수 3개
        int wallUpgradeLevel = Main.Get<UpgradeManager>().WallUpgradeLevel;

        if (wallUpgradeLevel > 1)
        {
            WallLimit = wallUpgradeLevel * WallLimit;
        }
    }
}