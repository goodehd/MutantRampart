using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceManager : IManagers
{
    private PoolManager _pool; 
    private Dictionary<string, Object> _resources = new Dictionary<string, Object>();

    public bool Init()
    {
        _pool = Main.Get<PoolManager>();
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
                return null;
            }
        }
        return resource as T;
    }

    public GameObject Instantiate(string key, Transform parent = null)
    {
        GameObject prefab = LoadResource<GameObject>(key);
        if (prefab == null)
        {
            return null;
        }

        if (_pool.IsPooling(prefab.name))
        {
            GameObject go = _pool.Pop(prefab);
            go.transform.SetParent(parent);
            return go;
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

        if (_pool.Push(obj))
        {
            return;
        }

        Object.Destroy(obj);
    }
}
