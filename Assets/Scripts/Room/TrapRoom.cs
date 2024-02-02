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

    public event Action<GameObject> OnFallinTrap;
    private bool _isTrapOn = false;
    
    public override void Init(RoomData data)
    {
        base.Init(data);

        _trapType = Enum.Parse<ETrapType>(this.gameObject.name);

    }
    public override void EnterRoom(Enemy enemy)
    {
        base.EnterRoom(enemy);
        if (_isTrapOn)return;
        StartCoroutine(LavaTrap(Enemys,enemy));
        
    }

    IEnumerator LavaTrap(LinkedList<CharacterBehaviour> enemys, Enemy enemy)
    {
        enemy.Renderer.flipX = false;
        enemy.transform.position = Literals.TrapEnemyPos[Enemys.Count % 6] + transform.position;
        StatModifier mod = new StatModifier(0f, EStatModType.Multip, 1, this);
        enemy.Status.GetStat<Stat>(EstatType.MoveSpeed).AddModifier(mod);
        Enemys.AddLast(enemy);

        yield return new WaitForSeconds(3f); //피해를 주기까지의 시간
        
        foreach (var listenemy in enemys)
        {
            //피해를 주는 로직
            listenemy.Status.GetStat<Vital>(EstatType.Hp).CurValue -= 10;
            //피해를 주었으니 enemy가 움직일 수 있게하고
            listenemy.Status.GetStat<Stat>(EstatType.MoveSpeed).RemoveModifier(mod);
        }
        _isTrapOn = true;
        yield return new WaitForSeconds(5f); //쿨타임
        _isTrapOn = false;
    }

}
