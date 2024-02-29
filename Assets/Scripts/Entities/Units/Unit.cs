using System;
using UnityEngine;

public class Unit : CharacterBehaviour
{
    private SpriteOutline _outline;

    public override void Init(CharacterData data) 
    {
        base.Init(data); 
        StateMachine.AddState(EState.Dead, new UnitDeadState(this));

        InitializeAttack(data);
        InitializeSkill(data);

        _outline = GetComponentInChildren<SpriteOutline>();
    }

    private void InitializeSkill(CharacterData data)
    {
        switch (data.PrefabName)
        {
            case "Cleric":
                StateMachine.AddState(EState.Skill, new UnitClericSkillState(this));
                break;
            case "Knight":
                StateMachine.AddState(EState.Skill, new UnitKnightSkillState(this));
                break;
            case "Kunoichi":
                StateMachine.AddState(EState.Skill, new UnitKunoichiSkillState(this));
                break;
            case "Priest":
                StateMachine.AddState(EState.Skill, new UnitPriestSkillState(this));
                break;
            case "Shaman":
                StateMachine.AddState(EState.Skill, new UnitShamanSkillState(this));
                break;
            default:
                break;
        }
    }

    private void InitializeAttack(CharacterData data)
    {
        switch (data.AttackType)
        {
            case EAttackType.MeleeAttack:
                StateMachine.AddState(EState.Attack, new UnitAttackState(this));
                break;
            case EAttackType.RangedAttack:
                StateMachine.AddState(EState.Attack, new UnitRangedAttackState(this));
                break;
            case EAttackType.AreaAttack:
                StateMachine.AddState(EState.Attack, new UnitAreaAttackState(this));
                break;
            default:
                throw new ArgumentException($"Warning : Invalid attack type");
        }
    }

    public override void Die()
    {
        ((BatRoom)CharacterInfo.CurRoom).UnitCount--;
        base.Die();
    }

    public override void ResetCharacter()
    {
        base.ResetCharacter();
    }

    public void AddRagnedAttackTarget(Enemy enemy)
    {
        if(CharacterInfo.Data.AttackType != EAttackType.RangedAttack)
        {
            return;
        }

        UnitRangedAttackState state = (UnitRangedAttackState)StateMachine.GetState(EState.Attack);
        state.AddNeighborTarget(enemy);
    }

    public void DrawOutline()
    {
        _outline.DrawOutline();
    }

    public void UndrawOutline()
    {
        _outline.UndrawOutline();
    }
}
