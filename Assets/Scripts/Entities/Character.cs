using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int CurPosX { get; set; }
    public int CurPosY { get; set; }

    public CharacterStatus Status { get; private set; }
    public StateMachine StateMachine { get; private set; }
    public Animator Animator { get; private set; }
    public SpriteRenderer Renderer { get; private set; }

    private bool _initialize = false;

    private void Start()
    {
        Init(Main.Get<DataManager>().enemy["Slime"]);
    }

    public virtual void Init(CharacterData data)
    {
        if (_initialize)
            return;

        Status = new CharacterStatus(data);
        StateMachine = new StateMachine();

        StateMachine.AddState(EState.Idle, new IdleState(this));
        StateMachine.AddState(EState.Move, new MoveState(this));
        StateMachine.AddState(EState.Attack, new AttackState(this));
        StateMachine.AddState(EState.Dead, new DeadState(this));
        StateMachine.ChangeState(EState.Idle);

        this.Animator = GetComponentInChildren<Animator>();
        this.Renderer = GetComponentInChildren<SpriteRenderer>();

        CurPosX = 0;
        CurPosY = 0;

        _initialize = true;
    }

    private void Update()
    {
        StateMachine?.UpdateState();
    }
}
