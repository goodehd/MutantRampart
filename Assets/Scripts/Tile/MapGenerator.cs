using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapGenerator : MonoBehaviour
{
    public GameObject tilePrefab; // 타일 프리팹을 연결할 변수

    private void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        // 그리드를 생성하고 Grid 컴포넌트를 추가
        GameObject gridObject = new GameObject("Grid");
        Grid gridComponent = gridObject.AddComponent<Grid>();

        // Cell Size 및 Cell Gap 설정
        gridComponent.cellSize = new Vector3(1, 0.5f, 1); // x = 1, y = 0.5, z = 1
        gridComponent.cellGap = new Vector3(0, 0, 0); // x = 0, y = 0, z = 0

        // Cell Layout 설정
        gridComponent.cellLayout = GridLayout.CellLayout.IsometricZAsY; // Isometric Z As Y

        // Cell Swizzle 설정
        gridComponent.cellSwizzle = GridLayout.CellSwizzle.XYZ; // XYZ

        List<Vector3> tileMapCoordinates = new List<Vector3>()
        {
            new Vector3(-6, 0, 0),
            new Vector3(-3, 1.5f, 0),
            new Vector3(0, 3, 0),
            new Vector3(-3, -1.5f, 0),
            new Vector3(0, 0, 0),
            new Vector3(3, 1.5f, 0),
            new Vector3(0, -3, 0),
            new Vector3(3, -1.5f, 0),
            new Vector3(6, 0, 0)
        };

        foreach (Vector3 coordinate in tileMapCoordinates)
        {
            Instantiate(tilePrefab, coordinate, Quaternion.identity, gridObject.transform);
        }
        
    }
}