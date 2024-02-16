using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class TileManager : IManagers
{
    private ResourceManager resource;
    private List<List<RoomBehavior>> _roomObjList = new List<List<RoomBehavior>>();
    private GameObject _gridObject;
    public NavigationTile _navigation;

    public SpawnTile SpawnTile { get; private set; }
    public BatPoint BatSlot { get; set; }

    public RoomBehavior SelectRoom { get; private set; }
    public event Action OnSlectRoomEvent;
    

    public void GenerateMap(int x, int y)
    {
        _gridObject = new GameObject("Tile");

        // 그리드를 생성하고 Grid 컴포넌트를 추가
        Grid gridComponent = _gridObject.AddComponent<Grid>();

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

        GameObject spawn = resource.Instantiate("Prefabs/Room/SpawnTile", _gridObject.transform);
        spawn.transform.position = new Vector3(-6f, 0, 0);
        SpawnTile = spawn.GetComponent<SpawnTile>();

        BatSlot = resource.Instantiate("Prefabs/Room/BatPoint", _gridObject.transform).GetComponent<BatPoint>();

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
            RoomBehavior room = CreateDefaultRoom(y, i, pos + offset);
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
        return true;
    }

    public void SetSelectRoom(RoomBehavior room)
    {
        if (SelectRoom == room)
            return;

        SelectRoom = room;
        if(room != null)
            OnSlectRoomEvent?.Invoke();
    }

    public RoomBehavior ChangeRoom(Room changeRoom)
    {
        GameObject obj = Main.Get<SceneManager>().Scene.CreateRoom($"{changeRoom.Data.PrefabName}");
        obj.transform.position = SelectRoom.transform.position;
        obj.transform.parent = _gridObject.transform;

        RoomBehavior room = obj.GetComponent<RoomBehavior>();
        room.RoomInfo = changeRoom;
        room.IndexX = SelectRoom.IndexX;
        room.IndexY = SelectRoom.IndexY;
        room.RoomInfo.EquipedRoom();

        _roomObjList[SelectRoom.IndexX][SelectRoom.IndexY] = room;

        SelectRoom.RoomInfo.UnEquipedRoom();
        SelectRoom.OnDestroyRoom();
        resource.Destroy(SelectRoom.gameObject);
        SelectRoom = room;
        SelectRoom.StartFlashing();

        return room;
    }

    public List<RoomBehavior> GetNeighbors(int curPosX, int curPosY)
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
            if (!room.IsDoorOpen((ERoomDir)(1 << i)))
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

    public void SetRoomDir(RoomBehavior room, ERoomDir dir, bool isOpen)
    {
        RoomBehavior Neighbor = null;
        switch (dir)
        {
            case ERoomDir.RightTop:
                Neighbor = GetRoom(room.IndexX, room.IndexY + 1);
                if (Neighbor == null)
                    return;
                Neighbor.ModifyDoor(ERoomDir.LeftBottom, isOpen);
                break;
            case ERoomDir.RightBottom:
                Neighbor = GetRoom(room.IndexX - 1, room.IndexY);
                if (Neighbor == null)
                    return;
                Neighbor.ModifyDoor(ERoomDir.LeftTop, isOpen);
                break;
            case ERoomDir.LeftTop:
                Neighbor = GetRoom(room.IndexX + 1, room.IndexY);
                if (Neighbor == null)
                    return;
                Neighbor.ModifyDoor(ERoomDir.RightBottom, isOpen);
                break;
            case ERoomDir.LeftBottom:
                Neighbor = GetRoom(room.IndexX, room.IndexY - 1);
                if (Neighbor == null)
                    return;
                Neighbor.ModifyDoor(ERoomDir.RightTop, isOpen);
                break;
        }
        room.ModifyDoor(dir, isOpen);
        _navigation.SetCheckWall(room);
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
        obj.transform.parent = _gridObject.transform;
        RoomBehavior room = obj.GetComponent<RoomBehavior>();
        room.IndexX = x;
        room.IndexY = y;

        return room;
    }

    public void DeleteRoom(Room room)
    {
        RoomBehavior newRoom = CreateDefaultRoom(room.IndexX, room.IndexY, _roomObjList[room.IndexX][room.IndexY].transform.position);
        _roomObjList[room.IndexX][room.IndexY].OnDestroyRoom();
        resource.Destroy(_roomObjList[room.IndexX][room.IndexY].gameObject);
        _roomObjList[room.IndexX][room.IndexY] = newRoom;
    }
}