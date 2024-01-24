using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatRoom : Room
{
    private Transform _unitPos; //테스트용 유닛을 배치 할 위치
    
    protected bool _isUnitOn = false; //유닛을 배치 했는지 안했는지. 
    protected bool _isUnitAlive = true; //유닛이 살았는지 죽었는지. 나중에 유닛의 이벤트를 받던가 함수를 받아서 이걸 꺼줘야하는데 흠.. 어떻게하지 ㅋㅋ
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        _roomStatus = EStatusformat.Bat;
        OnEnemyEnterRoom += EnemyEnterRoom;
        _unitPos = this.transform.GetChild(2);
        return true;
    }

    protected override void OnMouseEnter()
    {
        if (!Main.Get<TileManager>().ChangeSetButtons.isUnitSet)
        {
            base.OnMouseEnter();
        }
        else
        {
            foreach (var _Ren in _renderer)
            {
                _Ren.material = _buildAvailable;
            }
        }
    }

    protected override void OnMouseExit()
    {
        for (int i = 0; i < 2; i++)
        {
            _renderer[i].material = _origin[i];
        }
        /*
        if (!Main.Get<TileManager>()._changeSetButtons.isUnitSet)
        {
            base.OnMouseExit();
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                _renderer[i].material = _origin[i];
            }
        }
        */
    }

    protected override void OnMouseDown()
    {
        if (Main.Get<TileManager>().ChangeSetButtons.isRoomSet)
        {
            base.OnMouseDown();
        }
        else
        {
            Main.Get<TileManager>().ChangeUnit(IndexX,IndexY,_unitPos);
            _isUnitOn = true;
        }
        
    }

    public override void EnemyEnterRoom(GameObject g)
    {
        base.EnemyEnterRoom(g);
        if (_isUnitOn && _isUnitAlive)
        {
            //전투에 관련한 함수 실행
        }
    }

    public void UnitPlacement(GameObject unit) //현재 버튼에 임시로 연결해둠
    {
        if(_isUnitOn) return; //배치할 수 없습니다! UI띄우기
        GameObject obj = Instantiate(unit);
        obj.transform.position = _unitPos.position;
        _isUnitOn = true;
    }
}
