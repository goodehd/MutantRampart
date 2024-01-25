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

    public void StartStage(string enemyName, int count)
    {
        StartCoroutine(SpawnEnemy(enemyName, count));
    }

    private IEnumerator SpawnEnemy(string enemyName, int count)
    {
        while (count != 0)
        {
            Character enemy = _curScene.CreateCharacter(enemyName);
            enemy.transform.position = _spawnPos;
            enemy.StateMachine.ChangeState(EState.Move);
            yield return new WaitForSeconds(_spawnRatio);
            count--;
        }
    }
}
