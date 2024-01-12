using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : SingletonBehavior<Main>
{
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

    protected override void Init()
    {
        
    }
}
