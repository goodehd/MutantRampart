using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BatRoom : Room
{
    [SerializeField] private Transform _unitPos; //테스트용 유닛을 배치 할 위치
    
    private bool _isUnitOn = false; //유닛을 배치 했는지 안했는지. 
    private bool _isUnitAlive = true; //유닛이 살았는지 죽었는지. 나중에 유닛의 이벤트를 받던가 함수를 받아서 이걸 꺼줘야하는데 흠.. 어떻게하지 ㅋㅋ

    public List<Character> Units { get; private set; } = new List<Character>();
    public LinkedList<GameObject> Enemys { get; private set; } = new LinkedList<GameObject>();

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        _roomStatus = EStatusformat.Bat;
        OnEnemyEnterRoom += EnemyEnterRoom;
        
        return true;
    }

    public override void EnemyEnterRoom(GameObject g)
    {
        base.EnemyEnterRoom(g);

        if (Units.Count > 0)
        {
            Enemys.AddLast(g);
            Character enemy = g.GetComponent<Character>();
            enemy.Renderer.flipX = false;
            enemy.StateMachine.ChangeState(EState.Attack);
            enemy.transform.position = new Vector3(transform.position.x - 1f, transform.position.y + 1.25f, g.transform.position.z);

            foreach (Character unit in Units)
            {
                unit.StateMachine.ChangeState(EState.Attack);
            }
        }
    }

    public void UnitPlacement(GameObject unit) //현재 버튼에 임시로 연결해둠
    {
        if(_isUnitOn) return; //배치할 수 없습니다! UI띄우기
        GameObject obj = Instantiate(unit);
        obj.transform.position = _unitPos.position;
        _isUnitOn = true;
    }

    public void CreateUnitTest()
    {
        GameObject go = Main.Get<ResourceManager>().InstantiateWithPoolingOption("Prefabs/Character/Unit_GunTest");
        go.transform.position = new Vector3(transform.position.x + 1f, transform.position.y + 2f, go.transform.position.z);
        Units.Add(go.GetComponent<Character>());
    }
}
