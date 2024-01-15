using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : IManagers
{
    // 각 Assets 들을 Dictionary 로 관리
    private Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();
    private Dictionary<string, GameObject> _prefabs = new Dictionary<string, GameObject>();

    // 초기화 과정에서 LoadAll
    public bool Init()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(""); // todo : 경로에서 sprites 다 델꼬와
        foreach (Sprite sprite in sprites)
        {
            _sprites.Add(sprite.name, sprite);
        }

        GameObject[] objs = Resources.LoadAll<GameObject>(""); // todo : 경로에서 prefab 대상 델꼬와
        foreach (GameObject obj in objs)
        {
            _prefabs.Add(obj.name, obj);
        }

        return true;
    }

    public Sprite LoadSprite(string key)
    {
        // TryGetValue - Dictionary 안에 해당 key 가 있는지 확인하고, 있으면 sprite 리턴, 없으면 false 리턴.
        // TryGetValue 는 key 가 없는 경우에 false 를 리턴해주면서 최대한 에러 뜨는 걸 최소화(?)시켜준다고 보면 된다.
        if (!_sprites.TryGetValue(key, out Sprite sprite))
        {
            Debug.LogError($"[ResourceManager] LoadSprite({key}) : Failed to load sprite.");
            return null;
        }

        return sprite;
    }

    public GameObject LoadPrefab(string key)
    {
        if (!_prefabs.TryGetValue(key, out GameObject prefab))
        {
            Debug.LogError($"[ResourceManager] LoadPrefab({key}) : Failed to load prefab.");
            return null;
        }

        return prefab;
    }

    /// <summary>
    /// Instantiate 함수에 Pooling Option 추가한 힘수입니다.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="parent"></param>
    /// <param name="pooling"></param>
    /// <returns></returns>
    public GameObject InstantiateWithPoolingOption(string key, Transform parent = null, bool pooling = false) // 선택적 매개변수 -- 위치 항상 뒤에 있도록 주의 !
    {
        GameObject prefab = LoadPrefab(key);
        if (prefab == null)
        {
            Debug.LogError($"[ResourceManager] InstantiateWithPoolingOption({key}) : Failed to load prefab.");
            return null;
        }

        if (pooling)
        {
            return Main.Get<PoolManager>().Pop(prefab);
        }

        // pooling 이 false 면 pooling 이 적용 안 된 Instantiate 진행 !
        GameObject obj = Object.Instantiate(prefab, parent);
        obj.name = prefab.name;
        return obj;
    }

    public void Destroy(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }

        if (Main.Get<PoolManager>().Push(obj))
        {
            return;
        }

        // pooling 이 적용 안 된 친구들은 Destroy.
        Object.Destroy(obj);
    }
}
