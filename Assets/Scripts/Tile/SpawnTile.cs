using System.Collections;
using UnityEngine;
public class SpawnTile : MonoBehaviour
{
    private Scene _curScene;
    private float _spawnRatio;
    private Vector3 _spawnPos = Vector3.zero;
    public void Start()
    {
        _curScene = Main.Get<SceneManager>().Scene;
        _spawnPos = transform.position;
        _spawnPos.y += 1.5f;
        _spawnPos.z = 3f;
        _spawnRatio = 1.0f;
    }

    public void StartStage(StageMonsterInfo info)
    {
        StartCoroutine(SpawnEnemy(info));
    }

    private IEnumerator SpawnEnemy(StageMonsterInfo info)
    {
        foreach (Monster monsterInfo in info.Monsters)
        {
            for (int i = 0; i < monsterInfo.Count; i++)
            {
                CharacterBehaviour enemy = _curScene.CreateCharacter(monsterInfo.Name);
                ((Enemy)enemy).ClearVisited();
                enemy.transform.position = _spawnPos;
                enemy.StateMachine.ChangeState(EState.Move);
                yield return new WaitForSeconds(_spawnRatio);
            }
        }
    }
}