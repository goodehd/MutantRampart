using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCollision : MonoBehaviour
{
    private Room _owner;

    void Start()
    {
        _owner = transform.parent.GetComponent<Room>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Enemy") && EnterNewRoom(collision.gameObject))
        {
            _owner.EnemyEnterRoom(collision.gameObject);
        }
    }

    private bool EnterNewRoom(GameObject go)
    {
        Character enemy = go.GetComponent<Character>();
        if (enemy == null)
        {
            return false;
        }
        return _owner.IndexX != enemy.CurPosX || _owner.IndexY != enemy.CurPosY;
    }
}
