using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BatRoom : Room
{
    public Character[] Units { get; private set; } = new Character[3];
    public int UnitCount {  get; set; } 

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        _roomStatus = EStatusformat.Bat;
        OnEnemyEnterRoom += EnemyEnterRoom;
        UnitCount = 0;

        return true;
    }

    public override void EnemyEnterRoom(GameObject g)
    {
        base.EnemyEnterRoom(g);

        if (UnitCount > 0)
        {
            Character enemy = g.GetComponent<Character>();
            enemy.Renderer.flipX = false;
            enemy.StateMachine.ChangeState(EState.Attack);
            enemy.transform.position = new Vector3(transform.position.x - 0.5f - Enemys.Count * 0.1f, 
                transform.position.y + 1.25f + Enemys.Count * 0.1f, g.transform.position.z);
            Enemys.AddLast(enemy);

            foreach (Character unit in Units)
            {
                if(unit == null)
                {
                    continue;
                }
                unit.StateMachine.ChangeState(EState.Attack);
            }
        }
    }

    public void CreateUnit(int slotIndex, CharacterData data)
    {
        if (Units[slotIndex] != null)
        {
            DeleteUnit(slotIndex);
        }

        Units[slotIndex] = Main.Get<SceneManager>().Scene.CreateCharacter(data.Key);
        Units[slotIndex].transform.position = new Vector3(transform.position.x + 1f, transform.position.y + 2f, 3.0f);
        Units[slotIndex].CurRoom = this;
        UnitCount++;
    }

    public void DeleteUnit(int slotIndex)
    {
        Main.Get<ResourceManager>().Destroy(Units[slotIndex].gameObject);
        Units[slotIndex] = null;
        UnitCount--;
    }
}
