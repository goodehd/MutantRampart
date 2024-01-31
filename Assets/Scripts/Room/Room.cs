using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using DG.Tweening;

public enum EStatusformat
{
    Bat,
    Trap,
    Home,
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
    public RoomData ThisRoomData { get; set; }
    //public event Action<GameObject> OnEnemyEnterRoom; //임시로 GameObject를 넣어둠
    
    public LinkedList<CharacterBehaviour> Enemys { get; private set; } = new LinkedList<CharacterBehaviour>();

    public int IndexX { get; set; }
    public int IndexY { get; set; }
    public bool isEndPoint { get; protected set; }

    public virtual void Start()
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
        ThisRoomData = Main.Get<DataManager>().Room[gameObject.name];

        _isInitialized = true;
        return true;
    }

    public virtual void EnemyEnterRoom(GameObject g) //매개변수로 Enemy스크립트가 들어갈듯? 혹은 움직임 로직 관련 (이벤트에도)
    {
        // thisRoomNum을 리턴시켜줌(어떤 형식으로 보내줄지는 미정)
        // Enemy가 방을 기억하고 다시 올 확률을 낮추기 위해서.
        CharacterBehaviour enemy = g.GetComponent<CharacterBehaviour>();
        enemy.CurPosX = this.IndexX;
        enemy.CurPosY = this.IndexY;
        //enemy.CurRoom = this;
    }
    protected virtual void OnMouseEnter()
    {
        if(EventSystem.current.IsPointerOverGameObject())return;
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
        if (EventSystem.current.IsPointerOverGameObject()) return;
        FocusCamera();

        //((DayMain_SceneUI)Main.Get<UIManager>().SceneUI).ActiveCategory();


        //ChangeRoom_PopupUI changeRoomUI = Main.Get<UIManager>().OpenPopup<ChangeRoom_PopupUI>("ChangeRoom_PopUpUI");
        //changeRoomUI.SelectRoom = this;
        //changeRoomUI.RoomName = gameObject.name;
    }

    private void FocusCamera()
    {
        Vector3 pos = new Vector3(transform.position.x + 1.5f, transform.position.y + 1.8f, Camera.main.transform.position.z);
        Camera.main.transform.DOMove(pos, 0.5f);
        Camera.main.DOOrthoSize(2.5f, 0.5f);
    }

    public void RemoveEnemy(CharacterBehaviour src)
    {
        Enemys.Remove(src);
    }
}
