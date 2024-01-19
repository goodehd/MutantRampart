using UnityEngine;

public enum EUnitState
{
    Idle,
    Attacking
}

public class Unit : MonoBehaviour
{

    private Animator _animator;

    private EUnitState unitState;

    //private List<Enemy> currentEnemiesInRange = new List<Enemy>(); // 플레이어의 공격 범위 안에 들어온 적들을 List 로 관리.

    private int damage; // 얘네 나중에 DataManager 에서 델꼬오는걸로.
    private float attackRate; // 얘네 나중에 DataManager 에서 델꼬오는걸로.
    private float lastAttackTime; // 얘네 나중에 DataManager 에서 델꼬오는걸로.


    // Update is called once per frame
    void Update()
    {
        // Enemy List 가 비어있는지 확인하여 상태 업데이트.
        UpdateUnitState();

    }

    private void UpdateUnitState()
    {
        //if (currentEnemiesInRange.Count > 0) // 범위 안에 적이 있을 때 공격 상태로 전환.
        //{
        //    SetUnitState(EUnitState.Attacking);
        //}
        //else // 범위 안에 적이 없을 때 Idle 상태로 전환.
        //{
        //    SetUnitState(EUnitState.Idle);
        //}
    }

    private void SetUnitState(EUnitState newState)
    {
        unitState = newState;

        // 여기에 상태가 변경될 때 수행할 작업 추가 가능.
        switch (unitState)
        {
            case EUnitState.Idle:
                // todo : Idle Animation 추가
                //_animator.SetBool("Idle", unitState == EUnitState.Idle);
                Debug.Log("Unit is in Idle state");
                break;

            case EUnitState.Attacking:
                Attack();
                Debug.Log("Unit is in Attacking state");
                break;
        }
    }

    private void Attack()
    {
        if (Time.time - lastAttackTime > attackRate) // Time.time - lastAttackTime 은 최근 공격 시간 간격을 의미함. 이 값이 attackRate 보다 크다면 공격 go !
        {
            lastAttackTime = Time.time;

            // todo : Attacking 상태에서의 공격 동작
            //foreach (Enemy enemy in currentEnemiesInRange)
            //{
            //    // e.g. 적에게 데미지를 입히는 등의 동작 수행
            //    // enemy.GetComponent<Enemy>().TakeDamage(damageAmount);
            //}

            // todo : Attack Animation 추가
            //_animator.SetTrigger("Attack");
        }
    }

    private void OnTriggerEnter(Collider other) // Enemy 가 해당 범위 안에 들어올 경우 List.Add
    {
        //if (other.CompareTag("Enemy"))
        //{
        //    currentEnemiesInRange.Add(other);
        //}
    }

    private void OnTriggerExit(Collider other) // 적이 collider 범위 밖으로 나가면(근데 나갈 일이 있으려나?) List 에서 제거
    {
        //if (other.CompareTag("Enemy"))
        //{
        //    currentEnemiesInRange.Remove(other);
        //}
    }

    private void GetDamage(int damage) // todo : GetDamage() 구현
    {
        //hp 가 0 이 되면 죽겠지
    }

    private void Die() // todo : Die() 구현
    {
        // 애니메이션
    }
}
