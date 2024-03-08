using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class StartScene_SceneUI : BaseUI
{
    private TextMeshProUGUI _pressAnyKeyTxt;
    private float blinkDuration = 0.5f;
    protected override void Init()
    {
        SetUI<TextMeshProUGUI>();

        _pressAnyKeyTxt = GetUI<TextMeshProUGUI>("PressAnyKeyTxt");

        _pressAnyKeyTxt.DOBlendableColor(new Color(1, 1, 1, 0), blinkDuration).SetLoops(-1, LoopType.Yoyo);
    }

    private void Update()
    {
        if(Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            Main.Get<SceneManager>().ChangeScene<SelectScene>();
        }
    }
}
