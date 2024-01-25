using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : BaseState
{
    private TileManager _tileMap;
    private Vector3 _targetPos = Vector3.zero;
    private Coroutine _coroutine;
    private bool _isMove;

    public MoveState(Character owner) : base(owner)
    {
        _tileMap = Main.Get<TileManager>();
        _isMove = false;
    }

    public override void EnterState()
    {
        MoveStart();
        Owner.Animator.SetBool(Literals.Move, true);
    }

    public override void ExitState()
    {
        Owner.Animator.SetBool(Literals.Move, false);
        CoroutineManagement.Instance.StopManagedCoroutine(_coroutine);
        _isMove = false;
    }

    public override void UpdateState()
    {

    }

    private void MoveStart()
    {
        if (!_isMove)
        {
            _coroutine = CoroutineManagement.Instance.StartManagedCoroutine(Movement());
        }
    }

    private void SetTargetPos(out int targetX, out int targetY)
    {
        List<GameObject> rooms = _tileMap.GetNeighbors(Owner.CurPosX, Owner.CurPosY);

        if(rooms.Count == 0)
        {
            SetStageStartMovePos(out targetX, out targetY);
            return;
        }

        int randomIndex = Random.Range(0, rooms.Count);

        Room room = rooms[randomIndex].GetComponent<Room>();
        _targetPos = room.transform.position;
        _targetPos.y += 1.5f;
        _targetPos.z = 3f;

        targetX = room.IndexX; 
        targetY = room.IndexY;
    }

    private void SetStageStartMovePos(out int targetX, out int targetY)
    {
        Room room = _tileMap.GetRoom(1, 0).GetComponent<Room>();
        _targetPos = room.transform.position;
        _targetPos.y += 1.5f;
        _targetPos.z = 3f;

        targetX = room.IndexX;
        targetY = room.IndexY;
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
        _isMove = true;

        SetTargetPos(out int x, out int y);
        SetDir();

        while (Owner.transform.position != _targetPos)
        {
            float step = Owner.Status[EstatType.MoveSpeed].Value * Time.deltaTime;
            Owner.transform.position = Vector3.MoveTowards(Owner.transform.position, _targetPos, step);
            yield return null;
        }

        Owner.CurPosX = x;
        Owner.CurPosY = y;

        _isMove = false;

        if (_tileMap.GetRoom(x, y).GetComponent<Room>().isEndPoint)
        {
            Owner.StateMachine.ChangeState(EState.Dead);
            Main.Get<GameManager>().PlayerHp--;
        }
        else
        {
            MoveStart();
        }
    }
}
