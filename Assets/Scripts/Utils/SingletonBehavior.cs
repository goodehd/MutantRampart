using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonBehavior<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_isDisabled)
            {
                return null;
            }

            if(_instance != null)
            {
                return _instance;
            }

            _instance = FindFirstObjectByType(typeof(T)) as T;
            if( _instance == null )
            {
                GameObject singletonObject = new GameObject() { name = "[Singleton] " + typeof(T) };
                _instance = singletonObject.AddComponent<T>();
                InitSingleton();
                DontDestroyOnLoad(singletonObject);
            }
            else
            {
                DontDestroyOnLoad(_instance);
            }

            return _instance;
        }
    }

    protected static void InitSingleton()
    {
        if (_instance != null)
        {
            (_instance as SingletonBehavior<T>).Init();
        }
    }

    protected abstract void Init();

    protected static bool _isDisabled;

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _isDisabled = true;
        }
    }
}
