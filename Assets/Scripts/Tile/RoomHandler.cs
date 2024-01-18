using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomHandler : MonoBehaviour
{
    public static RoomHandler instance;
    [SerializeField] private Material _previewMaterial;
    private MeshFilter[] _meshFilters;
    private TilemapRenderer[] _tilemapRenderers;
    public void ShowPreview(Room room)
    {
    }

    private void Awake()
    {
        instance = this;
        _tilemapRenderers = GetComponentsInChildren<TilemapRenderer>();
    }
}
