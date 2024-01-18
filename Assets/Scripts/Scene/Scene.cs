using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour
{
    private ResourceManager _resource;
    private PoolManager _pool;
    private DataManager _data;

    private LinkedList<GameObject> _listObj = new LinkedList<GameObject>();

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        _resource = Main.Get<ResourceManager>();
        _data = Main.Get<DataManager>();
    }

    public GameObject CreateCharcter(string name)
    {
        GameObject obj = _resource.InstantiateWithPoolingOption(name);

        if (obj == null)
            return null;

        Character character = obj.GetComponent<Character>();
        character.SetStatus(_data.enemy[obj.name]);

        return obj;
    }
}
