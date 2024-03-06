using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrapRoom : RoomBehavior
{
    private enum ETrapType
    {
        Lava,
        Snow,
        Molar,
        Count
    }

    private ETrapType _trapType;



    public override void Init(RoomData data)
    {
        base.Init(data);

        _trapType = Enum.Parse<ETrapType>(this.gameObject.name);

    }
    public override void EnterRoom(Enemy enemy)
    {
        base.EnterRoom(enemy);
        ConditionAdd(enemy);
    }
    private void ConditionAdd(Enemy enemy)
    {
        switch (_trapType)
        {
            case ETrapType.Lava:
                StartCoroutine(LavaTrap(enemy));
                break;
            case ETrapType.Snow:
                enemy.ConditionMachine.AddCondition(new FrostbiteCondition(enemy, RoomInfo.Data));
                break;
            case ETrapType.Molar:
                enemy.ConditionMachine.AddCondition(new PuppetCondition(enemy, RoomInfo.Data));
                break;
            case ETrapType.Count:
                break;
        }
    }

    IEnumerator LavaTrap(Enemy enemy)
    {
        enemy.Renderer.flipX = false;
        enemy.transform.position = Literals.TrapEnemyPos[Enemys.Count % 6] + transform.position;
        enemy.StateMachine.ChangeState(EState.Idle);

        yield return new WaitForSeconds(3f); //피해를 주기까지의 시간

        enemy.TakeDmageNoneDefense(enemy.Status.GetStat<Vital>(EstatType.Hp).CurValue * RoomInfo.Data.UpgradeValue_1 * 0.01f);

        if (!enemy.CharacterInfo.IsDead)
        {
            enemy.StateMachine.ChangeState(EState.Move);
        }

    }
}

