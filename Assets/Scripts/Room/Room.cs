using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum EStatusformat
{
    Bat,
    Trap,
    DefaultTile,
    Count
}

public class Room : MonoBehaviour
{
    public int[] thisRoomNum = new int[2]; //tile에서 2중배열의 위치를 각각 룸이 가지고 있음 ex) tile[0,1] == tishRoomNum{0,1}
    protected EStatusformat roomStatus = EStatusformat.DefaultTile;
    public bool _canBuildRoom => Main.Get<TileManager>().CanBuildRoom;
    private GameObject[] _childobj = new GameObject[2];
    private TilemapRenderer[] _renderer = new TilemapRenderer[2];
    private Material[] _origin = new Material[2];
    [SerializeField] protected Material _buildAvailable;
    [SerializeField] protected Material _buildNotAvailable;
    
    private Vector3 _tilePosition;
    
    public event Action<GameObject> OnEnemyEnterRoom; //임시로 GameObject를 넣어둠
    
    private bool _initialized;
    public virtual void Awake()
    {
        Initialize();
    }
    

    public virtual bool Initialize()
    {
        if (_initialized) return false;
        for (int i = 0; i < 2; i++)
        {
            _renderer[i] = this.transform.GetChild(i).GetComponent<TilemapRenderer>();
            _origin[i] = this.transform.GetChild(i).GetComponent<TilemapRenderer>().material;
            _childobj[i] = this.transform.GetChild(i).gameObject;
        }
        _initialized = true;
        return true;
    }

    public virtual void EnemyEnterRoom(GameObject g) //매개변수로 Enemy스크립트가 들어갈듯? 혹은 움직임 로직 관련 (이벤트에도)
    {
        // thisRoomNum을 리턴시켜줌(어떤 형식으로 보내줄지는 미정)
        // Enemy가 방을 기억하고 다시 올 확률을 낮추기 위해서.
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
        Debug.Log(this.gameObject.name);
        // UI띄우고
        // 내가 누군지 보내주고
        ChangeRoomUI _changeRoomUI = Main.Get<UIManager>().OpenPopup<ChangeRoomUI>("ChangeRoom_PopUpUI");
        _changeRoomUI.selectRoom = this;
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
