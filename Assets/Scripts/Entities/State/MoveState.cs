using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : BaseState
{
    private TileManager _tileMap;
    private Stack<GameObject> _pathObjStk;
    private Vector3 _targetPos = Vector3.zero;
    private Coroutine _coroutine;
    private bool[,] _visited;

    public MoveState(CharacterBehaviour owner) : base(owner)
    {
        _tileMap = Main.Get<TileManager>();
        _pathObjStk = new Stack<GameObject>();
    }

    public override void EnterState()
    {
        Owner.Animator.SetBool(Literals.Move, true);
        _tileMap.GetMapSize(out int x, out int y);
        _visited = new bool[x, y];
        MoveStart();
    }

    public override void ExitState()
    {
        Owner.Animator.SetBool(Literals.Move, false);
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

    private void SetPathList()
    {
        List<RoomBehavior> rooms = _tileMap.GetNeighbors(Owner.CurPosX, Owner.CurPosY);
        List<RoomBehavior> targetRoom = new List<RoomBehavior>();

        if (rooms.Count == 0)
        {
            SetStageStartMovePos();
            return;
        }
        else
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (!_visited[rooms[i].IndexX, rooms[i].IndexY])
                {
                    targetRoom.Add(rooms[i]);
                }
            }
        }

        if (targetRoom.Count == 0)
        {
            _pathObjStk = _tileMap.FindUnvisitedRoom(Owner.CurPosX, Owner.CurPosY, _visited);
        }
        else
        {
            int randomIndex = Random.Range(0, targetRoom.Count);
            _pathObjStk.Push(targetRoom[randomIndex].gameObject);
        }
    }

    private void SetStageStartMovePos()
    {
        _pathObjStk.Push(_tileMap.GetRoom(1, 0).gameObject);
        RoomBehavior room = _tileMap.GetRoom(1, 0);
        _visited[room.IndexX, room.IndexY] = true;
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

    private void SetTargetPos()
    {
        GameObject obj = _pathObjStk.Pop();
        RoomBehavior roomBehavior = obj.GetComponent<RoomBehavior>();
        _targetPos = obj.transform.position;
        _targetPos.y += 1.5f;
        _targetPos.z = 3f;
        _visited[roomBehavior.IndexX, roomBehavior.IndexY] = true;
    }

    private IEnumerator Movement()
    {
        while (true)
        {
            if(_pathObjStk.Count <= 0)
            {
                SetPathList();
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
