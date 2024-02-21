using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    public Animator Animator { get; private set; }
    public SpriteRenderer Renderer { get; private set; }
    public Character CharacterInfo { get; private set; }
    public StateMachine StateMachine { get; private set; }
    public ConditionMachine ConditionMachine { get; private set; }

    public CharacterStatus Status { get { return CharacterInfo.Status; } }
    public int CurPosX { get { return CharacterInfo.CurPosX ; } set { CharacterInfo.CurPosX = value; } }
    public int CurPosY { get { return CharacterInfo.CurPosY; } set { CharacterInfo.CurPosY = value; } }
    public int CurIndex { get { return CharacterInfo.CurIndex; } set { CharacterInfo.CurIndex = value; } }
    public RoomBehavior CurRoom { get { return CharacterInfo.CurRoom; } set { CharacterInfo.CurRoom = value; } }

    private bool _initialize = false;

    public virtual void Init(CharacterData data)
    {
        if (_initialize)
            return;

        this.Animator = GetComponentInChildren<Animator>();
        this.Renderer = GetComponentInChildren<SpriteRenderer>();

        CharacterInfo = new Character(data);
        CharacterInfo.Init(data);


        StateMachine = new StateMachine();

        StateMachine.AddState(EState.Idle, new IdleState(this));
        StateMachine.AddState(EState.Move, new MoveState(this));
        StateMachine.ChangeState(EState.Idle);

        ConditionMachine = new ConditionMachine();
        
        CharacterInfo.Status.GetStat<Vital>(EstatType.Hp).OnValueZero += Die;

        _initialize = true;


    }

    public void SetData(Character data)
    {
        CharacterInfo = data;
        data.Owner = this;
        CharacterInfo.Status.GetStat<Vital>(EstatType.Hp).OnValueZero += Die;
    }

    private void Update()
    {
        StateMachine?.UpdateState();
    }

    public virtual void Die()
    {
        if (!CharacterInfo.IsDead)
        {
            CharacterInfo.IsDead = true;
            StateMachine.ChangeState(EState.Dead);
        }
    }

    public virtual void ResetCharacter()
    {
        CharacterInfo.IsDead = false;
        Status.GetStat<Vital>(EstatType.Hp).SetCurValueMax();
        StateMachine.ChangeState(EState.Idle);
    }

    private void OnDestroy()
    {
        Debug.Log("사라짐 뿅");
        ConditionMachine.ClearConditions();
        CharacterInfo.Status.GetStat<Vital>(EstatType.Hp).OnValueZero -= Die;
    }

}
