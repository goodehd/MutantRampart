using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Setting_PopupUI : BaseUI
{
    private Button _settingCloseBtn;
    private Button _bgmMuteBtn;
    private Button _bgmMaxBtn;
    private Button _effectMuteBtn;
    private Button _effectMaxBtn;
    private Button _uiMuteBtn;
    private Button _uiMaxBtn;
    private Button _languageBtn;
    private Button _guideBtn;
    private Button _usBtn;

    private Slider _bgmSlider; // slider 는 UnityEngine.UI
    private Slider _effectSlider;
    private Slider _uiSlider;

    protected override void Init()
    {
        SetButton();
        SetSlider();
    }

    private void SetButton()
    {
        SetUI<Button>();

        _settingCloseBtn = GetUI<Button>("SettingCloseBtn");
        _bgmMuteBtn = GetUI<Button>("BGMMuteBtn");
        _bgmMaxBtn = GetUI<Button>("BGMMaxBtn");
        _effectMuteBtn = GetUI<Button>("EffectMuteBtn");
        _effectMaxBtn = GetUI<Button>("EffectMaxBtn");
        _uiMuteBtn = GetUI<Button>("UIMuteBtn");
        _uiMaxBtn = GetUI<Button>("UIMaxBtn");
        _languageBtn = GetUI<Button>("LangBtn");
        _guideBtn = GetUI<Button>("GuideBtn");
        _usBtn = GetUI<Button>("UsBtn");

        SetUICallback(_settingCloseBtn.gameObject, EUIEventState.Click, ClickCloseBtn);
        SetUICallback(_bgmMuteBtn.gameObject, EUIEventState.Click, ClickBGMMuteBtn);
        SetUICallback(_bgmMaxBtn.gameObject, EUIEventState.Click, ClickBGMMaxBtn);
        SetUICallback(_effectMuteBtn.gameObject, EUIEventState.Click, ClickEffectMuteBtn);
        SetUICallback(_effectMaxBtn.gameObject, EUIEventState.Click, ClickEffectMaxBtn);
        SetUICallback(_uiMuteBtn.gameObject, EUIEventState.Click, ClickUIMuteBtn);
        SetUICallback(_uiMaxBtn.gameObject, EUIEventState.Click, ClickUIMaxBtn);
        SetUICallback(_languageBtn.gameObject, EUIEventState.Click, ClickLanguageBtn);
        SetUICallback(_guideBtn.gameObject, EUIEventState.Click, ClickGuideBtn);
        SetUICallback(_usBtn.gameObject, EUIEventState.Click, ClickUsBtn);
    }

    private void SetSlider()
    {
        SetUI<Slider>();

        _bgmSlider = GetUI<Slider>("BGM_Slider");
        _effectSlider = GetUI<Slider>("Effect_Slider");
        _uiSlider = GetUI<Slider>("UI_Slider");
    }

    // todo : slider들(사운드 관련) value 값 조절

    private void ClickCloseBtn(PointerEventData EventData)
    {
        Main.Get<UIManager>().ClosePopup();
    }

    private void ClickBGMMuteBtn(PointerEventData EventData)
    {
        _bgmSlider.value = 0f;
    }

    private void ClickBGMMaxBtn(PointerEventData EventData)
    {
        _bgmSlider.value = 1f;
    }
    private void ClickEffectMuteBtn(PointerEventData EventData)
    {
        _effectSlider.value = 0f;

    }

    private void ClickEffectMaxBtn(PointerEventData EventData)
    {
        _effectSlider.value = 1f;

    }
    private void ClickUIMuteBtn(PointerEventData EventData)
    {
        _uiSlider.value = 0f;

    }

    private void ClickUIMaxBtn(PointerEventData EventData)
    {
        _uiSlider.value = 1f;
    }

    private void ClickLanguageBtn(PointerEventData EventData)
    {

    }

    private void ClickGuideBtn(PointerEventData EventData)
    {

    }

    private void ClickUsBtn(PointerEventData EventData)
    {

    }

}
