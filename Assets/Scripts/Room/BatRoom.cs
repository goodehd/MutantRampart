using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BatRoom : Room
{
    public CharacterBehaviour[] Units { get; private set; } = new CharacterBehaviour[3];
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
            CharacterBehaviour enemy = g.GetComponent<CharacterBehaviour>();
            enemy.Renderer.flipX = false;
            enemy.StateMachine.ChangeState(EState.Attack);
            enemy.transform.position = new Vector3(transform.position.x - Enemys.Count * 0.2f, 
                transform.position.y + 1.5f + Enemys.Count * 0.2f, g.transform.position.z);
            Enemys.AddLast(enemy);

            foreach (CharacterBehaviour unit in Units)
            {
                if(unit == null)
                {
                    continue;
                }
                unit.StateMachine.ChangeState(EState.Attack);
            }
        }
    }

    public void CreateUnit(int slotIndex, Character data)
    {
        if (Units[slotIndex] != null)
        {
            DeleteUnit(slotIndex);
        }

        Units[slotIndex] = Main.Get<SceneManager>().Scene.CreateCharacter(data.Data.Key);
        Units[slotIndex].SetData(data);
        Units[slotIndex].transform.position = new Vector3(transform.position.x + 1f, transform.position.y + 2f, 3.0f);
        Units[slotIndex].CurRoom = this;
        UnitCount++;
    }

    public void DeleteUnit(int slotIndex)
    {
        Units[slotIndex].CurRoom = null;

        Main.Get<ResourceManager>().Destroy(Units[slotIndex].gameObject);
        Units[slotIndex] = null;
        UnitCount--;
    }
}
