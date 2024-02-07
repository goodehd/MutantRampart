using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PathInfo
{
    public GameObject CurRoom;
    public PathInfo PrevRoom;
    

    public PathInfo(GameObject cur, PathInfo prev)
    {
        CurRoom = cur;
        PrevRoom = prev;
    }
}

public class TileManager : IManagers
{
   
    private ResourceManager resource;
    private List<List<GameObject>> _roomObjList = new List<List<GameObject>>();
    private GameObject _gridObject;

    public SpawnTile SpawnTile { get; private set; }
    public BatPoint BatSlot { get; set; }

    public RoomBehavior SelectRoom { get; private set; }
    public event Action OnSlectRoomEvent;
    public int CurMapRow = 3;
    public int CurMapCol = 3;

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

        for (int i = 0; i < x; i++)
        {
            _roomObjList.Add(new List<GameObject>());

            pos.Set(-3f * i, 1.5f * i);
            for (int j = 0; j < y; j++)
            {
                offset.Set(3f * j, 1.5f * j);
                GameObject obj = Main.Get<SceneManager>().Scene.CreateRoom("Default");
                obj.transform.position = pos + offset;
                obj.transform.parent = _gridObject.transform;
                _roomObjList[i].Add(obj);
                RoomBehavior room = obj.GetComponent<RoomBehavior>();
                room.IndexX = i;
                room.IndexY = j;
            }
        }

        GameObject spawn = resource.Instantiate("Prefabs/Room/SpawnTile", _gridObject.transform);
        spawn.transform.position = new Vector3(-6f, 0, 0);
        SpawnTile = spawn.GetComponent<SpawnTile>();

        BatSlot = resource.Instantiate("Prefabs/Room/BatPoint", _gridObject.transform).GetComponent<BatPoint>();
    }

    public void ExpandMapRow()
    {
        Vector2 pos = Vector2.zero;
        Vector2 offset = Vector2.zero;

        _roomObjList.Add(new List<GameObject>());
        pos.Set(-3f * CurMapRow, 1.5f * CurMapRow);
        for (int i = 0; i < CurMapCol; i++)
        {
            offset.Set(3f * i, 1.5f * i);
            GameObject obj = Main.Get<SceneManager>().Scene.CreateRoom("Default");
            obj.transform.position = pos + offset;
            obj.transform.parent = _gridObject.transform;
            _roomObjList[CurMapRow].Add(obj);
            RoomBehavior room = obj.GetComponent<RoomBehavior>();
            room.IndexX = CurMapRow;
            room.IndexY = i;
        }
        CurMapRow++;
    }

    public void ExpandMapCol()
    {
        Vector2 pos = Vector2.zero;
        Vector2 offset = Vector2.zero;

        pos.Set(3f * CurMapCol, 1.5f * CurMapCol);
        for (int i = 0; i < CurMapRow; i++)
        {
            offset.Set(-3f * i, 1.5f * i);
            GameObject obj = Main.Get<SceneManager>().Scene.CreateRoom("Default");
            obj.transform.position = pos + offset;
            obj.transform.parent = _gridObject.transform;
            _roomObjList[i].Add(obj);
            RoomBehavior room = obj.GetComponent<RoomBehavior>();
            room.IndexX = i;
            room.IndexY = CurMapCol;
        }
        CurMapCol++;
    }
    public bool Init()
    {
        resource = Main.Get<ResourceManager>();
        return true;
    }

    public void SetSelectRoom(RoomBehavior room)
    {
        SelectRoom = room;
        if(room != null)
            OnSlectRoomEvent?.Invoke();
    }

    public RoomBehavior ChangeRoom(Room changeRoom)
    {
        GameObject obj = Main.Get<SceneManager>().Scene.CreateRoom($"{changeRoom.Data.Key}");
        obj.transform.position = SelectRoom.transform.position;
        obj.transform.parent = _gridObject.transform;
        _roomObjList[SelectRoom.IndexX][SelectRoom.IndexY] = obj;

        RoomBehavior room = obj.GetComponent<RoomBehavior>();
        room.RoomInfo = changeRoom;
        room.IndexX = SelectRoom.IndexX;
        room.IndexY = SelectRoom.IndexY;
        room.RoomInfo.EquipedRoom();

        SelectRoom.RoomInfo.UnEquipedRoom();
        resource.Destroy(SelectRoom.gameObject);
        SelectRoom = room;
        SelectRoom.StartFlashing();

        return room;
    }

    public List<GameObject> GetNeighbors(int curPosX, int curPosY)
    {
        List<GameObject> outPut = new List<GameObject>();

        // 오른쪽, 왼쪽, 위, 아래
        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        for (int i = 0; i < 4; ++i)
        {
            if (!IsRoomPositionValid(curPosX + dx[i], curPosY + dy[i]))
            {
                continue;
            }

            outPut.Add(_roomObjList[curPosX + dx[i]][curPosY + dy[i]]);
        }
        return outPut;
    }

    public GameObject GetRoom(int x, int y)
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

    public Stack<GameObject> FindUnvisitedRoom(int x, int y, bool[,] visited)
    {
        Queue<PathInfo> roomQ = new Queue<PathInfo>();
        List<GameObject> Neighbors = GetNeighbors(x, y);
        bool[,] isVisited = new bool[_roomObjList.Count, _roomObjList[0].Count];

        foreach (var neighbor in Neighbors)
        {
            roomQ.Enqueue(new PathInfo(neighbor, null));
            RoomBehavior room = neighbor.GetComponent<RoomBehavior>();
            isVisited[room.IndexX, room.IndexY] = true;
        }

        PathInfo targetInfo = null;
        bool isfind = false;
        while (true)
        {
            PathInfo info = roomQ.Dequeue();
            RoomBehavior curRoom = info.CurRoom.GetComponent<RoomBehavior>();
            Neighbors = GetNeighbors(curRoom.IndexX, curRoom.IndexY);

            foreach (var neighbor in Neighbors)
            {
                RoomBehavior neighborRoom = neighbor.GetComponent<RoomBehavior>();
                if (!visited[neighborRoom.IndexX, neighborRoom.IndexY])
                {
                    targetInfo = new PathInfo(neighborRoom.gameObject, info);
                    isfind = true;
                    break;
                }
                else if (!isVisited[neighborRoom.IndexX, neighborRoom.IndexY])
                {
                    roomQ.Enqueue(new PathInfo(neighborRoom.gameObject, info));
                    isVisited[neighborRoom.IndexX, neighborRoom.IndexY] = true;
                }
            }
            
            if (isfind)
                break;

            if (roomQ.Count <= 0)
                return null;
        }

        Stack<GameObject> pathList = new Stack<GameObject>();
        while (targetInfo != null)
        {
            pathList.Push(targetInfo.CurRoom);
            targetInfo = targetInfo.PrevRoom;
        }

        return pathList;
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
}