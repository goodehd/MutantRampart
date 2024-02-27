using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    private UIManager _ui;
    public Animator Animator { get; private set; }
    public SpriteRenderer Renderer { get; private set; }
    public Character CharacterInfo { get; private set; }
    public StateMachine StateMachine { get; private set; }
    public ConditionMachine ConditionMachine { get; private set; }
    public CharacterStatus Status { get { return CharacterInfo.Status; } }

    public int CurPosX { get { return CharacterInfo.CurPosX; } set { CharacterInfo.CurPosX = value; } }
    public int CurPosY { get { return CharacterInfo.CurPosY; } set { CharacterInfo.CurPosY = value; } }
    public int CurIndex { get { return CharacterInfo.CurIndex; } set { CharacterInfo.CurIndex = value; } }
    public RoomBehavior CurRoom { get { return CharacterInfo.CurRoom; } set { CharacterInfo.CurRoom = value; } }

    public event Action OnChangeCharcterInfoEvent;

    private bool _initialize = false;
    public virtual void Init(CharacterData data)
    {
        SetData(new Character(data));
        if (_initialize)
            return;

        _ui = Main.Get<UIManager>();
        this.Animator = GetComponentInChildren<Animator>();
        this.Renderer = GetComponentInChildren<SpriteRenderer>();

        StateMachine = new StateMachine();
        StateMachine.AddState(EState.Idle, new IdleState(this));
        StateMachine.AddState(EState.Move, new MoveState(this));
        StateMachine.ChangeState(EState.Idle);

        ConditionMachine = new ConditionMachine();

        _initialize = true;
    }

    public void SetData(Character data)
    {
        CharacterInfo = data;
        data.Owner = this;
        OnChangeCharcterInfoEvent?.Invoke();
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
            ConditionMachine.ClearConditions();
        }
    }

    public virtual void ResetCharacter()
    {
        CharacterInfo.IsDead = false;
        Status.GetStat<Vital>(EstatType.Hp).SetCurValueMax();
        Status.GetStat<Vital>(EstatType.Mp).CurValue = 0;
        StateMachine.ChangeState(EState.Idle);
        this.Renderer.color = Color.white;
        this.Renderer.flipX = true;
    }

    public Vector3 GetWorldPos()
    {
        return transform.position + transform.GetChild(0).localPosition;
    }

    public void TakeDamage(float damage)
    {
        float finalDamage = damage - (damage * Status[EstatType.Defense].Value / (Status[EstatType.Defense].Value+100));
        if (finalDamage <= 0)
        {
            finalDamage = 1;
        }
        Status.GetStat<Vital>(EstatType.Hp).CurValue -= finalDamage;
        CreateDamageText(finalDamage);
        if (Status.GetStat<Vital>(EstatType.Hp).CurValue <= 0)
        {
            Die();
        }
    }

    public void TakeDmageNoneDefense(float damage)
    {
        Status.GetStat<Vital>(EstatType.Hp).CurValue -= damage;
        CreateDamageText(damage);
        if (Status.GetStat<Vital>(EstatType.Hp).CurValue <= 0)
        {
            Die();
        }
    }

    private void CreateDamageText(float value)
    {
        DamageTextUI damageUI = _ui.CreateSubitem<DamageTextUI>();
        damageUI.SetText(value);
        if (!Renderer.flipX)
        {
            damageUI.SetPos(GetWorldPos(), GetWorldPos() + new Vector3(-1f, 0f, 0f));
        }
        else
        {
            damageUI.SetPos(GetWorldPos(), GetWorldPos() + new Vector3(1f, 0f, 0f));
        }
    }

    public void DestroyUnit()
    {
        ConditionMachine.ClearConditions();
    }
}