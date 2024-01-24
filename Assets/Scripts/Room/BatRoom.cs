using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BatRoom : Room
{
    private Transform _unitPos; //테스트용 유닛을 배치 할 위치
    private bool _isUnitOn = false; //유닛을 배치 했는지 안했는지. 
    private bool _isUnitAlive = true; //유닛이 살았는지 죽었는지. 나중에 유닛의 이벤트를 받던가 함수를 받아서 이걸 꺼줘야하는데 흠.. 어떻게하지 ㅋㅋ

    public List<Character> Units { get; private set; } = new List<Character>();
    public LinkedList<GameObject> Enemys { get; private set; } = new LinkedList<GameObject>();

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        _roomStatus = EStatusformat.Bat;
        OnEnemyEnterRoom += EnemyEnterRoom;
        _unitPos = this.transform.GetChild(2);

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

    public void CreateUnitTest()
    {
        GameObject go = Main.Get<ResourceManager>().InstantiateWithPoolingOption("Prefabs/Character/Unit_GunTest");
        go.transform.position = new Vector3(transform.position.x + 1f, transform.position.y + 2f, go.transform.position.z);
        Units.Add(go.GetComponent<Character>());
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
            Main.Get<TileManager>().ChangeUnit(IndexX, IndexY, _unitPos);
            _isUnitOn = true;
        }

    }
}
