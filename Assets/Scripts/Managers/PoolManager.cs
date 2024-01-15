using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Pool
{
    private GameObject _prefab;
    private IObjectPool<GameObject> _pool;
    private Transform _root;
    private Transform Root
    {
        get
        {
            if (_root == null)
            {
                GameObject obj = new GameObject() { name = $"[Pool_Root] {_prefab.name}" };
                _root = obj.transform;
            }
            return _root;
        }
    }
    public Pool(GameObject prefab)
    {
        this._prefab = prefab;
        this._pool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestroy);
    }

    public GameObject Pop()
    {
        return _pool.Get();
    }

    public void Push(GameObject gameObject)
    {
        _pool.Release(gameObject);
    }

    private GameObject OnCreate()
    {
        GameObject gameObject = GameObject.Instantiate(_prefab);
        gameObject.transform.SetParent(Root);
        gameObject.name = _prefab.name;
        return gameObject;
    }

    private void OnGet(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    private void OnRelease(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy(GameObject gameObject)
    {
        GameObject.Destroy(gameObject);
    }
}

public class PoolManager : IManagers
{
    private Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();

    public GameObject Pop(GameObject prefab)
    {
        // 1. 풀이 없으면 새로 만든다.
        if(_pools.ContainsKey(prefab.name) == false)
        {
            CreatePool(prefab);
        }
        // 2. 해당 풀에서 하나 가져온다.
        return _pools[prefab.name].Pop();
    }

    public bool Push(GameObject gameObject)
    {
        // 1. 풀이 있는지 확인한다.
        if(_pools.ContainsKey(gameObject.name) == false)
        {
            return false;
        }
        // 2. 풀에 게임 오브젝트를 넣는다.
        _pools[gameObject.name].Push(gameObject);

        return true;
    }

    public void Clear()
    {
        _pools.Clear();
    }

    private void CreatePool(GameObject prefab)
    {
        Pool pool = new Pool(prefab);
        _pools.Add(prefab.name, pool);
        Debug.Log($"[pool] {prefab.name} 생성");
    }

    public bool Init()
    {
        return true;
    }
}
