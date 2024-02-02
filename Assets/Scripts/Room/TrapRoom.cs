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

    
    public override void Init(RoomData data)
    {
        base.Init(data);

        _trapType = Enum.Parse<ETrapType>(this.gameObject.name);

    }

    protected override void OnMouseEnter()
    {
        base.OnMouseEnter();
    }

    protected override void OnMouseExit()
    {
        base.OnMouseExit();
    }

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
    }

    public override void EnterRoom(Enemy enemy)
    {
        base.EnterRoom(enemy);
    }

    /*private void LavaTrap(GameObject g)
    {
        StartCoroutine(LavaDamage(g));//0.5초마다 1의 데미지를 10번
    }

    IEnumerator LavaDamage(GameObject g)
    {
        Character enemy = g.GetComponent<Character>();
        for (int i = 0; i < 10; i++) //0.5초마다 1의 데미지를 10번
        {
            enemy.Status.GetStat<Vital>(EstatType.Hp).CurValue -= 1;
            
            yield return new WaitForSeconds(0.5f);
        }
    }*/

    /*private void SnowTrap(GameObject g)
    {
        StartCoroutine(SnowSlow(g));
    }
    IEnumerator SnowSlow(GameObject g)
    { 
        //느려지게 하는 함수
        Character enemy = g.GetComponent<Character>();
        StatModifier mod = new StatModifier(0.5f, EStatModType.Multip, 1);
        enemy.Status.GetStat<Stat>(EstatType.MoveSpeed).AddModifier(mod);
        yield return new WaitForSeconds(2);
        enemy.Status.GetStat<Stat>(EstatType.MoveSpeed).RemoveModifier(mod);
    }*/

}
