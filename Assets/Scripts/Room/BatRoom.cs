using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BatRoom : Room
{
    public Character[] Units { get; private set; } = new Character[3];
    public LinkedList<Character> Enemys { get; private set; } = new LinkedList<Character>();

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        _roomStatus = EStatusformat.Bat;
        OnEnemyEnterRoom += EnemyEnterRoom;

        return true;
    }

    public override void EnemyEnterRoom(GameObject g)
    {
        base.EnemyEnterRoom(g);

        if (Units.Length > 0)
        {
            Character enemy = g.GetComponent<Character>();
            enemy.Renderer.flipX = false;
            enemy.StateMachine.ChangeState(EState.Attack);
            enemy.transform.position = new Vector3(transform.position.x - 1f, transform.position.y + 1.25f, g.transform.position.z);
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
    }

    public void DeleteUnit(int slotIndex)
    {
        Main.Get<ResourceManager>().Destroy(Units[slotIndex].gameObject);
        Units[slotIndex] = null;
    }

    public void CreateUnitTest()
    {
        GameObject go = Main.Get<ResourceManager>().InstantiateWithPoolingOption("Prefabs/Character/Unit_GunTest");
        go.transform.position = new Vector3(transform.position.x + 1f, transform.position.y + 2f, go.transform.position.z);
        Units[0] = go.GetComponent<Character>();
    }
}
