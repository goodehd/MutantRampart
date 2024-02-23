using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : BaseState
{
    private TileManager _tileMap;
    private Stack<RoomBehavior> _pathObjStk;
    private Stack<Vector2> _pathPosStk;
    private Vector3 _targetPos = Vector3.zero;
    private Coroutine _coroutine;
    private bool[,] _visited;

    public MoveState(CharacterBehaviour owner) : base(owner)
    {
        _tileMap = Main.Get<TileManager>();
        _pathObjStk = new Stack<RoomBehavior>();
        _pathPosStk = new Stack<Vector2>();

        Init();
    }

    public override void Init()
    {
        _tileMap.GetMapSize(out int x, out int y);
        _visited = new bool[x, y];

        SetStageStartMovePos();
    }

    public override void EnterState()
    {
        Owner.Animator.SetBool(Literals.Move, true);
        MoveStart();
    }

    public override void ExitState()
    {
        Owner.Animator.SetBool(Literals.Move, false);
        _pathObjStk.Clear();
        _pathPosStk.Clear();
        StopCoroutine();
    }

    public override void UpdateState()
    {

    }

    public void StopCoroutine()
    {
        if (_coroutine != null)
        {
            Owner.StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    private void MoveStart()
    {
        _coroutine = Owner.StartCoroutine(Movement());
    }

    private void SetStageStartMovePos()
    {
        Vector3 startPos = _tileMap.GetRoom(1, 0).transform.position;
        startPos.y += 1.5f;
        startPos.z = 3f;

        _pathPosStk.Push(startPos);

        RoomBehavior room = _tileMap.GetRoom(1, 0);
        _visited[room.IndexX, room.IndexY] = true;
    }

    private void SetPathObjList()
    {
        List<RoomBehavior> rooms = _tileMap.GetNeighbors(Owner.CurPosX, Owner.CurPosY);
        List<RoomBehavior> targetRoom = new List<RoomBehavior>();

        for (int i = 0; i < rooms.Count; i++)
        {
            if (!_visited[rooms[i].IndexX, rooms[i].IndexY])
            {
                targetRoom.Add(rooms[i]);
            }
        }

        if (targetRoom.Count == 0)
        {
            _pathObjStk = _tileMap.FindUnvisitedRoom(Owner.CurPosX, Owner.CurPosY, _visited);
        }
        else
        {
            int randomIndex = Random.Range(0, targetRoom.Count);
            _pathObjStk.Push(targetRoom[randomIndex]);
        }
    }

    private void SetPathPosList()
    {
        if (_pathObjStk.Count <= 0)
        {
            SetPathObjList();
        }

        RoomBehavior roomBehavior = _pathObjStk.Pop();
        _visited[roomBehavior.IndexX, roomBehavior.IndexY] = true;

        Vector3 endPos = roomBehavior.transform.position;
        endPos.y += 1.5f;
        endPos.z = 3f;

        _tileMap.FindPath(Owner.transform.position, endPos, out _pathPosStk);
    }

    private void SetTargetPos()
    {
        _targetPos = _pathPosStk.Pop();
        _targetPos.z = 3.0f;
    }

    private void SetDir()
    {
        Vector3 direction = _targetPos - Owner.transform.position;
        float dotProduct = Vector3.Dot(direction, Vector3.right);

        if (dotProduct > 0)
        {
            Owner.Renderer.flipX = false;
        }
        else if (dotProduct < 0)
        {
            Owner.Renderer.flipX = true;
        }
    }

    private IEnumerator Movement()
    {
        while (true)
        {
            if(_pathPosStk.Count <= 0)
            {
                if (Owner.CurRoom.RoomInfo.Data.Type == EStatusformat.Home)
                {
                    ((HomeRoom)Owner.CurRoom).EnemyEnter();
                    Owner.Die();
                    yield break;
                }

                SetPathPosList();
            }
            SetTargetPos();
            SetDir();

            while (Owner.transform.position != _targetPos)
            {
                float step = Owner.Status[EstatType.MoveSpeed].Value * Time.deltaTime;
                Owner.transform.position = Vector3.MoveTowards(Owner.transform.position, _targetPos, step);
                yield return null;
            }
        }
    }
}
