using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DefaultTile : MonoBehaviour
{
    public Room RoomBuilt { get; set; }
    public bool _canBuildRoom => Main.Get<TileManager>().CanBuildRoom;
    private TilemapRenderer[] _renderer = new TilemapRenderer[2];
    private Material[] _origin = new Material[2];
    private Vector3 _tilePosition;
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
        for (int i = 0; i < 2; i++)
        {
            _renderer[i] = this.transform.GetChild(i).GetComponent<TilemapRenderer>();
            _origin[i] = this.transform.GetChild(i).GetComponent<TilemapRenderer>().material;
        }
    }
    
    private void OnMouseEnter()
    {
        if (_canBuildRoom)
        {
            foreach (var _Ren in _renderer)
            {
                _Ren.material = _buildAvailable;
            }
            
            
        }
        else
        {
            foreach (var _Ren in _renderer)
            {
                _Ren.material = _buildNotAvailable;
            }

        }
    }

    private void OnMouseExit()
    {
        for (int i = 0; i < 2; i++)
        {
            _renderer[i].material = _origin[i];
        }
    }

    private void OnMouseDown()
    {
        _tilePosition = this.gameObject.transform.position;
        this.gameObject.SetActive(false);
        GameObject tile = Main.Get<PoolManager>().Pop(TileManager.instance.rooms[1]);
        tile.transform.SetParent(TileManager.instance.gridObject.transform);
        tile.transform.position = _tilePosition;
    }
}
