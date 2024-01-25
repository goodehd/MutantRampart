using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ResourceManager : IManagers
{
    private Dictionary<string, Object> _resources = new Dictionary<string, Object>();

    public bool Init()
    {
        return true;
    }

    public T Load<T>(string path) where T : Object
    {
        if (!_resources.TryGetValue(path, out Object resource))
        {
            resource = Resources.Load<T>(path);
            if (resource != null)
                _resources.Add(path, resource);
        }

        return resource as T;
    }

    public T[] LoadAll<T>(string path) where T : Object
    {
        T[] resources = Resources.LoadAll<T>(path);

        if(resources != null)
        {
            foreach (T resource in resources)
            {
                _resources.Add(resource.name, resource);
            }
        }

        return resources;
    }

    public T LoadResource<T>(string key) where T : Object
    {
        if (!_resources.TryGetValue(key, out Object resource))
        {
            resource = Load<T>(key);
            if(resource == null)
            {
                Debug.LogError($"[ResourceManager] LoadResource({key}) : Failed to load resource.");
                return null;
            }
        }
        return resource as T;
    }

    public GameObject InstantiateWithPoolingOption(string key, Transform parent = null, bool pooling = false) // 선택적 매개변수 -- 위치 항상 뒤에 있도록 주의 !
    {
        GameObject prefab = LoadResource<GameObject>(key);
        if (prefab == null)
        {
            Debug.LogError($"[ResourceManager] InstantiateWithPoolingOption({key}) : Failed to load resource_prefab.");
            return null;
        }

        if (pooling)
        {
            return Main.Get<PoolManager>().Pop(prefab);
        }

        // pooling 이 false 면 pooling 옵션 없는 Instantiate 진행 !
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
