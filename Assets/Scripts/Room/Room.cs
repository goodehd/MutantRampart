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
    protected TilemapRenderer[] _renderer = new TilemapRenderer[2];
    protected Material[] _origin = new Material[2];
    protected EStatusformat _roomStatus = EStatusformat.DefaultTile;
    [SerializeField] protected Material _buildAvailable;
    [SerializeField] protected Material _buildNotAvailable;
    public bool isEquipedRoom = false;
    public RoomData RoomData { get; set; }
    public event Action<GameObject> OnEnemyEnterRoom; //임시로 GameObject를 넣어둠
    
    
    
    public int IndexX { get; set; }
    public int IndexY { get; set; }

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
        Character enemy = g.GetComponent<Character>();
        enemy.CurPosX = this.IndexX;
        enemy.CurPosY = this.IndexY;
    }
    protected virtual void OnMouseEnter()
    {
        foreach (var _Ren in _renderer)
        {
            _Ren.material = _buildAvailable;
        }
        
        
    }

    protected virtual void OnMouseExit()
    {
        for (int i = 0; i < 2; i++)
        {
            _renderer[i].material = _origin[i];
        }
        
        
    }

    protected virtual void OnMouseDown()
    {
        if(EventSystem.current.IsPointerOverGameObject())return;
        ChangeRoomUI changeRoomUI = Main.Get<UIManager>().OpenPopup<ChangeRoomUI>("ChangeRoom_PopUpUI");
        changeRoomUI.SelectRoom = this;
        changeRoomUI.RoomName = gameObject.name;
    }
    
}
