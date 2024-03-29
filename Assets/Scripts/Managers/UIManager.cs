using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : IManagers
{
    private ResourceManager _resource;

    private Stack<BaseUI> _popupStack;
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
        _popupStack = new Stack<BaseUI>();
        _resource = Main.Get<ResourceManager>();
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

        BaseUI ui = _popupStack.Pop();
        ui.Destroy();
        _resource.Destroy(ui.gameObject);
    }

    public void CloseAllPopup()
    {
        while (_popupStack.Count > 0)
            ClosePopup();
    }

    public void DestroySubItem(GameObject obj)
    {
        _resource.Destroy(obj);
    }

    private GameObject InstantiateUI(string prefabName, string path)
    {
        string fullPath = $"{path}{prefabName}";
        GameObject uiObj = _resource.Instantiate(fullPath);
        uiObj.transform.SetParent(RootUI);
        return uiObj;
    }

    public void SetCanvasInfo(GameObject uiObject, bool isPopup)
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
        
        CanvasScaler scales = Utility.GetAddComponent<CanvasScaler>(uiObject);
        scales.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scales.referenceResolution = new Vector2(1920, 1080);
    }
}
