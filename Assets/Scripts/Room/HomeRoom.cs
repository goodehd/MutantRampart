using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeRoom : BatRoom
{
    public int damageAmount = 10;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // 적 오브젝트와 충돌했을 때
        {
            if (!_isUnitAlive) // Home 안의 유닛이 살아있지 않다면
            {
                // 플레이어의 Hp 감소
                Main.Get<GameManager>().PlayerHp -= damageAmount;
                Destroy(other.gameObject);
            }
            // 여기에서 필요한 다른 작업 수행 가능
        }
    }
}
