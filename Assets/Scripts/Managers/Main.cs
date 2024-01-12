using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : SingletonBehavior<Main>
{
    private readonly Dictionary<string, IManagers> _managers = new Dictionary<string, IManagers>();

    public static T Get<T>() where T : IManagers
    {
        if(Instance._managers.TryGetValue(typeof(T).Name, out IManagers manager))
        {
            return (T)manager;
        }
        return default;
    }

    protected override void Init()
    {
        CreateManager<DataManager>();
        CreateManager<ResourceManager>();
        CreateManager<ScenesManager>();
        CreateManager<SoundManager>();
        CreateManager<UIManager>();
        CreateManager<PoolManager>();
    }

    private void CreateManager<T>() where T : IManagers, new()
    {
        T manager = new T();

        if (!manager.Init())
            Debug.LogError($"Initialize Fail : {typeof(T).Name}");

        _managers.Add(typeof(T).Name, manager);
    }

    /*
    private readonly DataManager _data = new DataManager();
    private readonly ResourceManager _resource = new ResourceManager();
    private readonly ScenesManager _scenes = new ScenesManager();
    private readonly SoundManager _sound = new SoundManager();
    private readonly UIManager _ui = new UIManager();
    private readonly PoolManager _pool = new PoolManager();

    public static DataManager Data { get { return Instance._data; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static ScenesManager Scene { get { return Instance._scenes; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static UIManager UI { get { return Instance._ui; } }
    */
}
