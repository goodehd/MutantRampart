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
        Status.GetStat<Vital>(EstatType.Mp).CurValue = 0;
        this.Renderer.color = Color.white;
        StateMachine.ChangeState(EState.Idle);
    }

    public Vector3 GetWorldPos()
    {
        return transform.position + transform.GetChild(0).localPosition;
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = damage - Status[EstatType.Defense].Value;
        if(finalDamage <= 0)
        {
            finalDamage = 1;
        }
        Status.GetStat<Vital>(EstatType.Hp).CurValue -= finalDamage;
    }

    private void OnDestroy()
    {
        ConditionMachine.ClearConditions();
        if(CharacterInfo != null && CharacterInfo.Status != null)
            CharacterInfo.Status.GetStat<Vital>(EstatType.Hp).OnValueZero -= Die;
    }
}
