using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class RoomBehavior : MonoBehaviour
{
    public TilemapRenderer Renderer { get; private set; }
    public ThisRoom RoomInfo { get; private set; }

    public int CurPosX { get { return RoomInfo.CurPosX; } set { RoomInfo.CurPosX = value;} }
    public int CurPosY { get { return RoomInfo.CurPosY; } set { RoomInfo.CurPosY = value;} }
    public LinkedList<CharacterBehaviour> Enemys { get { return RoomInfo.Enemys; } set { RoomInfo.Enemys = value; } }

    private bool _initialize = false;

    public virtual void Init(RoomData data)
    {
        if (_initialize)
            return;

        RoomInfo = new ThisRoom();
        RoomInfo.Init(data);

        _initialize = true;
    }

    public void SetData(ThisRoom data)
    {
        RoomInfo = data;
    }

    protected virtual void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        FocusCamera();

        ((DayMain_SceneUI)Main.Get<UIManager>().SceneUI).ActiveCategory();

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
