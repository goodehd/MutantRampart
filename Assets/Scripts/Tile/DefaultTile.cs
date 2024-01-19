using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DefaultTile : MonoBehaviour
{
    public Room RoomBuilt { get; set; }
    public bool _canBuildRoom => Main.Get<TileManager>().CanBuildRoom;
    private TilemapRenderer _renderer;
    private Material _origin;
    [SerializeField] private Material _buildAvailable;
    [SerializeField] private Material _buildNotAvailable;

   /* public bool TryBuildRoomHere(EStatusformat status, out Room room)
    {
        room = null;

        if (Main.Get<TileManager>().CanBuildRoom == false)
        {
            Debug.Log("현재 배치할 수 있는 방의 Count 초과입니다.");
        }

        
    }*/

    private void Awake()
    {
        _renderer = GetComponent<TilemapRenderer>();
        _origin = GetComponent<TilemapRenderer>().material;
    }

    private void OnMouseEnter()
    {
        if (_canBuildRoom)
        {
            _renderer.material = _buildAvailable;
        }
        else
        {
            _renderer.material = _buildNotAvailable;
        }
    }

    private void OnMouseExit()
    {
        _renderer.material = _origin;
    }
}
