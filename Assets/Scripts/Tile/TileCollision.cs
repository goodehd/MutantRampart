using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCollision : MonoBehaviour
{
    private RoomBehavior _owner;

    void Start()
    {
        _owner = transform.parent.GetComponent<RoomBehavior>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Enemy") && EnterNewRoom(collision.gameObject))
        {
            _owner.EnterRoom(collision.gameObject.GetComponent<Enemy>());
        }
    }

    private bool EnterNewRoom(GameObject go)
    {
        CharacterBehaviour enemy = go.GetComponent<CharacterBehaviour>();
        if (enemy == null)
        {
            return false;
        }
        return _owner.IndexX != enemy.CurPosX || _owner.IndexY != enemy.CurPosY;
    }
}
