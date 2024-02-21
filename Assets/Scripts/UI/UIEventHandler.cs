using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Action<PointerEventData>[] _Callback = new Action<PointerEventData>[(int)EUIEventState.Max];

    public void OnPointerClick(PointerEventData eventData)
    {
        // TODO : 마우스 클릭 효과음 넣는부분;
        if (_Callback[(int)EUIEventState.Click] != null)
            _Callback[(int)EUIEventState.Click](eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_Callback[(int)EUIEventState.Hovered] != null)
            _Callback[(int)EUIEventState.Hovered](eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_Callback[(int)EUIEventState.Exit] != null)
            _Callback[(int)EUIEventState.Exit](eventData);
    }

    public void SetCallback(EUIEventState state, Action<PointerEventData> callback)
    {
        _Callback[(int)state] = callback;
    }

    private void OnDestroy()
    {
        for(int i = 0; i < _Callback.Length; i++)
        {
            _Callback[i] = null;
        }
    }
}
