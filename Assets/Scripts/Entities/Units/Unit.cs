using UnityEngine;

public class Unit : CharacterBehaviour
{
    public override void Init(CharacterData data) 
    {
        base.Init(data); 
        StateMachine.AddState(EState.Attack, new UnitAttackState(this));
        StateMachine.AddState(EState.Dead, new UnitDeadState(this));

        InitializeCharacter(data.PrefabName);
    }

    private void InitializeCharacter(string prefabName)
    {
        switch (prefabName)
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

    public override void Die()
    {
        ((BatRoom)CharacterInfo.CurRoom).UnitCount--;
        base.Die();
    }

    public override void ResetCharacter()
    {
        base.ResetCharacter();
    }
}
