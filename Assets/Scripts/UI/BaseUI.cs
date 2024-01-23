using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseUI : MonoBehaviour
{
    protected Dictionary<Type, Dictionary<string, UnityEngine.Object>> _uiObjects = new Dictionary<Type, Dictionary<string, UnityEngine.Object>>();

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        
    }

    protected void SetUI<T>() where T : UnityEngine.Object
    {
        T[] components = gameObject.GetComponentsInChildren<T>(true);
        _uiObjects.Add(typeof(T), components.ToDictionary(comp => comp.name, comp => comp as UnityEngine.Object));
    }

    protected T GetUI<T>(string objName) where T : UnityEngine.Object
    {
        if(!_uiObjects.TryGetValue(typeof(T), out Dictionary<string, UnityEngine.Object> dict))
        {
            throw new NullReferenceException($"BaseUI NotFoundObject : {objName}");
        }

        if (!dict.TryGetValue(objName, out UnityEngine.Object component))
        {
            throw new NullReferenceException($"BaseUI NotFoundObject : {objName}");
        }

        return component as T;
    }

    protected void SetUICallback(GameObject obj, EUIEventState state, Action<PointerEventData> callback)
    {
        UIEventHandler callbackHandler = Utility.GetAddComponent<UIEventHandler>(obj);
        callbackHandler.SetCallback(state, callback);
    }
}
