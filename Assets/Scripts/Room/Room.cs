using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public enum EStatusformat
{
    Bat,
    Trap,
    DefaultTile,
    Count
}

public class Room : MonoBehaviour
{
    private bool _isInitialized;
    private GameObject[] _childObj = new GameObject[2];
    private TilemapRenderer[] _renderer = new TilemapRenderer[2];
    private Material[] _origin = new Material[2];
    protected EStatusformat _roomStatus = EStatusformat.DefaultTile;
    [SerializeField] protected Material _buildAvailable;
    [SerializeField] protected Material _buildNotAvailable;
    public bool isEquipedRoom = false;
    public bool isCanBuildRoom => Main.Get<TileManager>().isCanBuildRoom;
    public event Action<GameObject> OnEnemyEnterRoom; //임시로 GameObject를 넣어둠
    
    public virtual void Awake()
    {
        Initialize();
    }
    

    public virtual bool Initialize()
    {
        if (_isInitialized) return false;
        for (int i = 0; i < 2; i++)
        {
            _renderer[i] = this.transform.GetChild(i).GetComponent<TilemapRenderer>();
            _origin[i] = this.transform.GetChild(i).GetComponent<TilemapRenderer>().material;
            _childObj[i] = this.transform.GetChild(i).gameObject;
        }
        _isInitialized = true;
        return true;
    }

    public virtual void EnemyEnterRoom(GameObject g) //매개변수로 Enemy스크립트가 들어갈듯? 혹은 움직임 로직 관련 (이벤트에도)
    {
        // thisRoomNum을 리턴시켜줌(어떤 형식으로 보내줄지는 미정)
        // Enemy가 방을 기억하고 다시 올 확률을 낮추기 위해서.
    }
    private void OnMouseEnter()
    {
        if (isCanBuildRoom)
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
        if(EventSystem.current.IsPointerOverGameObject())return;
        
        Debug.Log(this.gameObject.name);
        // UI띄우고
        // 내가 누군지 보내주고
        ChangeRoomUI changeRoomUI = Main.Get<UIManager>().OpenPopup<ChangeRoomUI>("ChangeRoom_PopUpUI");
        changeRoomUI.SelectRoom = this;
        changeRoomUI.RoomName = this.gameObject.name;
        /*

        Debug.Log(this.gameObject.name);
        _tilePosition = this.gameObject.transform.position;
        this.gameObject.SetActive(false);
        GameObject tile = Main.Get<PoolManager>().Pop(Main.Get<TileManager>().rooms[1]);

        tile.transform.SetParent(Main.Get<TileManager>().gridObject.transform);
        tile.transform.position = _tilePosition;
        */
    }
    
}
