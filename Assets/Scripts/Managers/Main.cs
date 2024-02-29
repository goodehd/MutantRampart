using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : SingletonBehavior<Main>
{
    private readonly Dictionary<Type, IManagers> _managers = new Dictionary<Type, IManagers>();

    public static T Get<T>() where T : class, IManagers
    {
        if(Instance._managers.TryGetValue(typeof(T), out IManagers manager))
        {
            return manager as T;
        }
        return null;
    }

    protected override void Init()
    {
        CreateManager<SaveDataManager>();
        CreateManager<UpgradeManager>();
        CreateManager<DataManager>();
        CreateManager<PoolManager>();
        CreateManager<ResourceManager>();
        CreateManager<SceneManager>();
        CreateManager<SoundManager>();
        CreateManager<UIManager>();
        CreateManager<TileManager>();
        CreateManager<GameManager>();
        CreateManager<StageManager>();
    }

    private void CreateManager<T>() where T : IManagers, new()
    {
        T manager = new T();

        if (!manager.Init())
        {
            Debug.LogError($"Initialize Fail : {typeof(T).Name}");
        }
        _managers.Add(typeof(T), manager);
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
