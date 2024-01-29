using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : IManagers
{
    private ResourceManager resource;
    private List<List<GameObject>> _roomObjList = new List<List<GameObject>>();
    private GameObject _gridObject;

    public SpawnTile SpawnTile { get; private set; }
    public int x = 3;
    public int y = 3;

   
    public void GenerateMap()
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

        GameObject spawn = resource.InstantiateWithPoolingOption("Prefabs/Room/SpawnTile", _gridObject.transform);
        spawn.transform.position = new Vector3(-6f, 0, 0);
        SpawnTile = spawn.GetComponent<SpawnTile>();
    }

    public bool Init()
    {
        resource = Main.Get<ResourceManager>();
        return true;
    }

    public Room ChangeRoom(int indexX, int indexY, string srcName)
    {
        Vector3 pos = _roomObjList[indexX][indexY].transform.position;
        resource.Destroy(_roomObjList[indexX][indexY].gameObject);

        GameObject obj = resource.InstantiateWithPoolingOption($"Prefabs/Room/{srcName}", _gridObject.transform);
        obj.transform.position = pos;
        _roomObjList[indexX][indexY] = obj;

        Room room = obj.GetComponent<Room>();
        room.IndexX = indexX;
        room.IndexY = indexY;

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
            return null;

        return _roomObjList[x][y];
    }

    public void GetMapSize(out int mapSizeX, out int mapSizeY)
    {
        mapSizeX = _roomObjList.Count;
        mapSizeY = _roomObjList[0].Count;
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