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
        CreateManager<TutorialManager>();
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

    public static void ManagerInit()
    {
        Get<UpgradeManager>().SaveUpgrade();
        Get<SaveDataManager>().SaveUpgradeData();
        Get<SaveDataManager>().DeleteData();
        Get<GameManager>().Init();
        Get<UIManager>().Init();
        Get<StageManager>().Init();
        Get<PoolManager>().Init();
        Get<UpgradeManager>().Init();
        Get<TutorialManager>().Init();
        Get<SoundManager>().SoundStop(ESoundType.BGM);
        Time.timeScale = 1.0f;
    }
}
