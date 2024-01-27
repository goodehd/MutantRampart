using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Setting_PopupUI : BaseUI
{
    private Button _settingCloseBtn;
    // slider 는 UnityEngine.UI
    private Slider _bgmSlider;
    private Slider _effectSlider;
    private Slider _uiSlider;
    private Button _bgmMuteBtn;
    private Button _bgmMaxBtn;
    private Button _effectMuteBtn;
    private Button _effectMaxBtn;
    private Button _uiMuteBtn;
    private Button _uiMaxBtn;
    private Button _guideBtn;
    private Button _usBtn;

    protected override void Init()
    {
        SetUI<Button>();
        SetUI<Slider>();

        _settingCloseBtn = GetUI<Button>("SettingCloseBtn");
        SetUICallback(_settingCloseBtn.gameObject, EUIEventState.Click, ClickCloseBtn);
        _bgmSlider = GetUI<Slider>("BGM_Slider");
        _effectSlider = GetUI<Slider>("Effect_Slider");
        _uiSlider = GetUI<Slider>("UI_Slider");
        _bgmMuteBtn = GetUI<Button>("BGMMuteBtn");
        SetUICallback(_bgmMuteBtn.gameObject, EUIEventState.Click, ClickBGMMuteBtn);
        _bgmMaxBtn = GetUI<Button>("BGMMaxBtn");
        SetUICallback(_bgmMaxBtn.gameObject, EUIEventState.Click, ClickBGMMaxBtn);
        _effectMuteBtn = GetUI<Button>("EffectMuteBtn");
        SetUICallback(_effectMuteBtn.gameObject, EUIEventState.Click, ClickEffectMuteBtn);
        _effectMaxBtn = GetUI<Button>("EffectMaxBtn");
        SetUICallback(_effectMaxBtn.gameObject, EUIEventState.Click, ClickEffectMaxBtn);
        _uiMuteBtn = GetUI<Button>("UIMuteBtn");
        SetUICallback(_uiMuteBtn.gameObject, EUIEventState.Click, ClickUIMuteBtn);
        _uiMaxBtn = GetUI<Button>("UIMaxBtn");
        SetUICallback(_uiMaxBtn.gameObject, EUIEventState.Click, ClickUIMaxBtn);
        _guideBtn = GetUI<Button>("GuideBtn");
        SetUICallback(_guideBtn.gameObject, EUIEventState.Click, ClickGuideBtn);
        _usBtn = GetUI<Button>("UsBtn");
        SetUICallback(_usBtn.gameObject, EUIEventState.Click, ClickUsBtn);

    }

    // todo : slider들(사운드 관련) value 값 조절

    private void ClickBGMMuteBtn(PointerEventData EventData)
    {

    }

    private void ClickBGMMaxBtn(PointerEventData EventData)
    {

    }
    private void ClickEffectMuteBtn(PointerEventData EventData)
    {

    }

    private void ClickEffectMaxBtn(PointerEventData EventData)
    {

    }
    private void ClickUIMuteBtn(PointerEventData EventData)
    {

    }

    private void ClickUIMaxBtn(PointerEventData EventData)
    {

    }

    private void ClickGuideBtn(PointerEventData EventData)
    {

    }
    private void ClickUsBtn(PointerEventData EventData)
    {

    }
    private void ClickCloseBtn(PointerEventData EventData)
    {
        Main.Get<UIManager>().ClosePopup();
    }
}
