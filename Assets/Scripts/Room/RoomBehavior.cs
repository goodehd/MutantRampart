using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class RoomBehavior : MonoBehaviour
{
    public TilemapRenderer Renderer { get; private set; }
    public Tilemap[] tilemap = new Tilemap[2];
    public ThisRoom RoomInfo { get; set; }
    public bool isEndPoint { get; set; }
    public bool isFlashing { get; set; } = false;

    public int IndexX { get { return RoomInfo.IndexX; } set { RoomInfo.IndexX = value;} }
    public int IndexY { get { return RoomInfo.IndexY; } set { RoomInfo.IndexY = value;} }
    public LinkedList<CharacterBehaviour> Enemys { get { return RoomInfo.Enemys; } set { RoomInfo.Enemys = value; } }

    private bool _initialize = false;

    private float blinkDuration = 0.4f;

    private Coroutine _flashingCoroutine;

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
            if (Main.Get<TileManager>().SelectRoom != null)
                Main.Get<TileManager>().SelectRoom.StopFlashing();
            Main.Get<TileManager>().SelectRoom = this;
            Main.Get<UIManager>().CloseAllPopup();
            SortRoom();
        }
        
        if(_flashingCoroutine == null)
            StartFlashing();
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

    private IEnumerator FlashEffect()
    {
        isFlashing = true;

        while (isFlashing)
        {
            for (int i = 0; i < 2; i++)
            {
                tilemap[i].color = Color.green;
            }

            yield return new WaitForSeconds(blinkDuration);

            for (int i = 0; i < 2; i++)
            {
                tilemap[i].color = Color.white;
            }

            yield return new WaitForSeconds(blinkDuration);
        }
        
    }

    public void StartFlashing()
    {
        // 만약 다른 오브젝트에서 작동 중인 코루틴이 있다면 중지
        if (_flashingCoroutine != null)
        {
            StopCoroutine(_flashingCoroutine);
        }
        // 깜빡이는 효과 시작
        _flashingCoroutine = StartCoroutine(FlashEffect());
    }

    public void StopFlashing()
    {
        // 깜빡이는 효과 중지
        if (_flashingCoroutine != null)
        {
            StopCoroutine(_flashingCoroutine);
            _flashingCoroutine = null;
        }
        // 다시 원래 색상으로 되돌리기
        for (int i = 0; i < 2; i++)
        {
            tilemap[i].color = Color.white;
        }
        isFlashing = false;
    }

    public void SortRoom()
    {
        ThisRoom Room = RoomInfo;

        if (RoomInfo.Data.Key != "Default")
        {
            // PlayerRooms 리스트에서 해당 RoomInfo를 찾음
            int index = Main.Get<GameManager>().PlayerRooms.IndexOf(Room);

            // 찾았으면 리스트에서 제거하고 0번째 인덱스에 추가
            if (index != -1)
            {
                Main.Get<GameManager>().PlayerRooms.RemoveAt(index);
                Main.Get<GameManager>().PlayerRooms.Insert(0, Room);
            }
        }
    }
}
