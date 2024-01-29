using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : IManagers
{
    private Stack<BaseUI> _popupStack = new Stack<BaseUI>();
    private Transform _rootUI;
    public BaseUI SceneUI { get; private set; }

    public Transform RootUI
    {
        get
        {
            if(_rootUI != null)
            {
                return _rootUI;
            }

            GameObject rootUI = GameObject.Find("RootUI");
            if (rootUI == null)
            {
                rootUI = new GameObject { name = "RootUI" };
            }
            _rootUI = rootUI.transform;
            return _rootUI;
        }
    }

    public bool Init()
    {
        _rootUI = null;
        return true;
    }

    public T OpenSceneUI<T>(string prefabName = null, string path = Literals.UI_SCENE_PATH) where T : BaseUI
    {
        if(prefabName == null)
        {
            prefabName = typeof(T).Name;
        }
        GameObject uiObj = InstantiateUI(prefabName, path);
        SetCanvasInfo(uiObj, false);
        SceneUI = Utility.GetAddComponent<T>(uiObj);
        return SceneUI as T;
    }

    public T OpenPopup<T>(string prefabName = null, string path = Literals.UI_POPUP_PATH) where T : BaseUI
    {
        if (prefabName == null)
        {
            prefabName = typeof(T).Name;
        }
        GameObject uiObj = InstantiateUI(prefabName, path);
        T popup = Utility.GetAddComponent<T>(uiObj);
        _popupStack.Push(popup);
        SetCanvasInfo(uiObj, true);
        return popup;
    }

    public T CreateSubitem<T>(string prefabName = null, Transform parent = null, string path = Literals.UI_SUBITEM_PATH) where T : BaseUI
    {
        if (prefabName == null)
        {
            prefabName = typeof(T).Name;
        }
        GameObject uiObj = InstantiateUI(prefabName, path);
        if(parent != null)
        {
            uiObj.transform.SetParent(parent);
        }
        return Utility.GetAddComponent<T>(uiObj);
    }

    public void ClosePopup()
    {
        if (_popupStack.Count == 0)
            return;

        Object.Destroy(_popupStack.Pop().gameObject);
    }

    public void CloseAllPopup()
    {
        while (_popupStack.Count > 0)
            ClosePopup();
    }

    public void DestroySubItem(GameObject obj)
    {
        Object.Destroy(obj);
    }

    private GameObject InstantiateUI(string prefabName, string path)
    {
        string fullPath = $"{path}{prefabName}";
        GameObject obj = Resources.Load<GameObject>(fullPath);

        if(obj == null)
        {
            Debug.LogError($"UI Prefab not Found, Path : {fullPath}");
            return null;
        }

        GameObject uiObj = Object.Instantiate(obj);
        uiObj.transform.SetParent(RootUI);
        uiObj.name = obj.name;
        return uiObj;
    }

    private void SetCanvasInfo(GameObject uiObject, bool isPopup)
    {
        Canvas canvas = uiObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (isPopup)
        {
            canvas.sortingOrder = _popupStack.Count + 1;
        }
        else
        {
            canvas.sortingOrder = 0;
        }

        CanvasScaler scales = uiObject.GetComponent<CanvasScaler>();
        scales.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scales.referenceResolution = new Vector2(1920, 1080);
    }
}
