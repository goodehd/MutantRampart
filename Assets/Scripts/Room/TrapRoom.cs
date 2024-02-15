using System;
using System.Collections;
using System.Collections.Generic;
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
    
    protected bool _isTrapOn = false;
    
    public override void Init(RoomData data)
    {
        base.Init(data);

        _trapType = Enum.Parse<ETrapType>(this.gameObject.name);

    }
    public override void EnterRoom(Enemy enemy)
    {
        base.EnterRoom(enemy);
        if (_isTrapOn)return;
        ConditionAdd(enemy);
    }
    private void ConditionAdd(Enemy enemy)
    {
        switch (_trapType)
        {
            case ETrapType.Lava:
                StartCoroutine(LavaTrap(Enemys, enemy));
                break;
            case ETrapType.Snow:
                Debug.Log("눈맵 진입");
                Debug.Log($"적용 전 속도 : {enemy.CharacterInfo.Status.GetStat<Stat>(EstatType.MoveSpeed).Value}");
                enemy.ConditionMachine.AddCondition(new FrostbiteCondition(enemy,RoomInfo.Data));
                Debug.Log($"적용 후 속도 : {enemy.CharacterInfo.Status.GetStat<Stat>(EstatType.MoveSpeed).Value}");
                break;
            case ETrapType.Molar:
                enemy.ConditionMachine.AddCondition(new PuppetCondition(enemy, RoomInfo.Data));
                break;
            case ETrapType.Count:
                break;
        }
    }

    IEnumerator LavaTrap(LinkedList<CharacterBehaviour> enemys, Enemy enemy)
    {
        enemy.Renderer.flipX = false;
        enemy.transform.position = Literals.TrapEnemyPos[Enemys.Count % 6] + transform.position;
        //StatModifier mod = new StatModifier(0f, EStatModType.Multip, 1, this);
        //enemy.Status.GetStat<Stat>(EstatType.MoveSpeed).AddModifier(mod);
        enemy.StateMachine.ChangeState(EState.Idle);
        Enemys.AddLast(enemy);

        yield return new WaitForSeconds(3f); //피해를 주기까지의 시간

        foreach (var listenemy in enemys)
        {
            //피해를 주는 로직
            listenemy.Status.GetStat<Vital>(EstatType.Hp).CurValue -= 10;
            //피해를 주었으니 enemy가 움직일 수 있게하고
            enemy.StateMachine.ChangeState(EState.Move);
            //listenemy.Status.GetStat<Stat>(EstatType.MoveSpeed).RemoveModifier(mod);
        }

        _isTrapOn = true;
        yield return new WaitForSeconds(5f); //쿨타임
        _isTrapOn = false;
    }
}

