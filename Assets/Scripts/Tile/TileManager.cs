using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour, IManagers
{

    public int x = 3;
    public int y = 3;

    private List<List<GameObject>> _roomObjList = new List<List<GameObject>>();

    public GameObject tilePrefab;
    public GameObject gridObject;
    public GameObject[] rooms;
    private int _canBuildRoomCount;
    private int _currentBuildroomCount;
    private bool _canBuildRoom = false;
    public bool CanBuildRoom // 방을 설치할 수 있는 타일이 몇개인지에 대한 정보 
    {
        get
        {
            if (_currentBuildroomCount <= _canBuildRoomCount)
            {
                _canBuildRoom = true;
            }
            return _canBuildRoom;
        }
    }

    private void Start()
    {
        gridObject = new GameObject("Tile");
        GenerateMap();
    }

    private void OnMouseDown()
    {
        // 마우스 클릭 시 실행되는 코드
        ChangeTile(); // 타일을 다른 타일로 변경하거나 삭제하는 함수 호출
    }

    private void ChangeTile()
    {
        // 이 함수에서 원하는 동작을 구현합니다.
        // 다른 타일로 변경하거나 삭제하는 코드를 작성할 수 있습니다.

        // 새로운 타일 프리팹을 생성하여 현재 타일 위치에 놓음
        //Instantiate(newTilePrefab, transform.position, Quaternion.identity);

        // 현재 타일을 삭제
        //Destroy(gameObject);
    }

    

    private void GenerateMap()
    {
        // 그리드를 생성하고 Grid 컴포넌트를 추가
        Grid gridComponent = gridObject.AddComponent<Grid>();

        // Cell Size 및 Cell Gap 설정
        gridComponent.cellSize = new Vector3(1, 0.5f, 1); // x = 1, y = 0.5, z = 1
        gridComponent.cellGap = new Vector3(0, 0, 0); // x = 0, y = 0, z = 0

        // Cell Layout 설정
        gridComponent.cellLayout = GridLayout.CellLayout.IsometricZAsY; // Isometric Z As Y

        // Cell Swizzle 설정
        gridComponent.cellSwizzle = GridLayout.CellSwizzle.XYZ; // XYZ

        //List<Vector3> tileMapCoordinates = new List<Vector3>()
        //{
        //    new Vector3(-6, 0, 0),
        //    new Vector3(-3, 1.5f, 0),
        //    new Vector3(0, 3, 0),
        //    new Vector3(-3, -1.5f, 0),
        //    new Vector3(0, 0, 0),
        //    new Vector3(3, 1.5f, 0),
        //    new Vector3(0, -3, 0),
        //    new Vector3(3, -1.5f, 0),
        //    new Vector3(6, 0, 0)


        //};

        //foreach (Vector3 coordinate in tileMapCoordinates)
        //{
        //    //Instantiate(tilePrefab, coordinate, Quaternion.identity, gridObject.transform);
        //    GameObject tile = Main.Get<PoolManager>().Pop(tilePrefab);
        //    tile.transform.SetParent(gridComponent.transform);
        //    tile.transform.position = coordinate;
        //}

        Vector2 pos = Vector2.zero;
        Vector2 offset = Vector2.zero;

       for (int i = 0; i < x; i++)
       {
            _roomObjList.Add(new List<GameObject>());
            pos.Set(-3f * i, 1.5f * i);
            for (int j = 0; j < y; j++)
            {
                offset.Set(3f * j, 1.5f * j);
                GameObject obj = Instantiate(tilePrefab, pos + offset, Quaternion.identity, gridObject.transform);
                _roomObjList[i].Add(obj);
            }
        }
    }

    public bool Init()
    {
        return true;
    }
}
