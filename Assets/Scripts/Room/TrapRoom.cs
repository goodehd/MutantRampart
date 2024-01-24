using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRoom : Room
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
    
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        _roomStatus = EStatusformat.Trap;
        //_trapType = Enum.Parse<ETrapType>(this.gameObject.name) ;
        OnEnemyEnterRoom += EnemyEnterRoom;
        
        return true;
    }

    protected override void OnMouseEnter()
    {
        if (!Main.Get<TileManager>().ChangeSetButtons.isUnitSet)
        {
            base.OnMouseEnter();
        }
        else
        {
            foreach (var _Ren in _renderer)
            {
                _Ren.material = _buildNotAvailable;
            }
        }
    }

    protected override void OnMouseExit()
    {
        base.OnMouseExit();
    }

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
    }

    public override void EnemyEnterRoom(GameObject g)
    {
        base.EnemyEnterRoom(g);
        OnFallinTrap = null;
        switch (_trapType)
        {
            case ETrapType.Lava :
                OnFallinTrap += LavaTrap;
                break;
            case ETrapType.Snow : 
                OnFallinTrap += SnowTrap;
                break;
            default:
                break;
        }
        OnFallinTrap?.Invoke(g);
    }

    private void LavaTrap(GameObject g)
    {
        //도트뎀
        StartCoroutine(LavaDamage(4));
    }

    IEnumerator LavaDamage(int a)
    {
        for (int i = 0; i < a; i++)
        {
            TestDamage(10);
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void TestDamage(int a) //테스트용. 데미지를 주는 함수(가상)
    {
        //enemy가 들고있으려나??
    }

    private void SnowTrap(GameObject g)
    {
        //슬로우
        StartCoroutine(SnowSlow(4));
    }
    IEnumerator SnowSlow(float a)
    { 
        //느려지게 하는 함수
        yield return new WaitForSeconds(a);
    }

    private void MolarTrap(GameObject g)
    {
        //어떤 함정을 만들까나 어금니 꽉
    }

}