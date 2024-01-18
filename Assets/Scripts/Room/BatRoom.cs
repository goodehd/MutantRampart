using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatRoom : Room
{
    [SerializeField] private Transform unitPos; //테스트용 유닛을 배치 할 위치
    
    private bool is_UnitOn = false; //유닛을 배치 했는지 안했는지. 
    private bool is_UnitAlive = true; //유닛이 살았는지 죽었는지. 나중에 유닛의 이벤트를 받던가 함수를 받아서 이걸 꺼줘야하는데 흠.. 어떻게하지 ㅋㅋ
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        roomStatus = EStatusformat.Bat;
        OnEnemyEnterRoom += EnemyEnterRoom;
        
        return true;
    }

    public override void EnemyEnterRoom(GameObject g)
    {
        base.EnemyEnterRoom(g);
        if (is_UnitOn && is_UnitAlive)
        {
            //전투에 관련한 함수 실행
        }
    }

    public void UnitPlacement(GameObject unit) //현재 버튼에 임시로 연결해둠
    {
        if(is_UnitOn) return; //배치할 수 없습니다! UI띄우기
        GameObject obj = Instantiate(unit);
        obj.transform.position = unitPos.position;
        is_UnitOn = true;
    }
}
