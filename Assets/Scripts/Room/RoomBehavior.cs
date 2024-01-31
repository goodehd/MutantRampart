using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class RoomBehavior : MonoBehaviour
{
    public TilemapRenderer Renderer { get; private set; }
    public Tilemap[] tilemap = new Tilemap[2];
    public ThisRoom RoomInfo { get; set; }
    public bool isEndPoint { get; set; }

    public int IndexX { get { return RoomInfo.IndexX; } set { RoomInfo.IndexX = value;} }
    public int IndexY { get { return RoomInfo.IndexY; } set { RoomInfo.IndexY = value;} }
    public LinkedList<CharacterBehaviour> Enemys { get { return RoomInfo.Enemys; } set { RoomInfo.Enemys = value; } }

    private bool _initialize = false;

    public virtual void Init(RoomData data)
    {
        if (_initialize)
            return;

        RoomInfo = new ThisRoom();
        RoomInfo.Init(data);
        for (int i = 0; i < 2; i++)
        {
            tilemap[i] = transform.GetChild(i).GetComponent<Tilemap>();
        }

        _initialize = true;
    }

    public virtual void EnterRoom(Enemy enemy)
    {
        enemy.CurPosX = IndexX;
        enemy.CurPosY = IndexY;
        enemy.CurRoom = this;
    }

    public void SetData(ThisRoom data)
    {
        RoomInfo = data;
    }

    protected virtual void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        for (int i = 0; i < 2; i++)
        {
            tilemap[i].color = Color.green;
        }

    }

    protected virtual void OnMouseExit()
    {
        for (int i = 0; i < 2; i++)
        {
            tilemap[i].color = Color.white;
        }


    }
    protected virtual void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        FocusCamera();

        ((DayMain_SceneUI)Main.Get<UIManager>().SceneUI).TileBat();

        if(Main.Get<TileManager>().SelectRoom != this)
        {
            Main.Get<TileManager>().SelectRoom = this;
            Main.Get<UIManager>().CloseAllPopup();
        }
    }

    private void FocusCamera()
    {
        Vector3 pos = new Vector3(transform.position.x + 1.5f, transform.position.y + 1.8f, Camera.main.transform.position.z);
        Camera.main.transform.DOMove(pos, 0.5f);
    }

    public void RemoveEnemy(CharacterBehaviour src)
    {
        Enemys.Remove(src);
    }
}
