using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Setting_PopupUI : BaseUI
{
    private SoundManager soundManager;

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
    private Button _exitBtn;
    private Button _option1CheckBtn;
    private Button _option1Btn;
    private Button _option2CheckBtn;
    private Button _option2Btn;
    private Button _confirmBtn;

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
        base.Init();
        
        soundManager = Main.Get<SoundManager>();

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
        _exitBtn = GetUI<Button>("ExitBtn");
        _option1CheckBtn = GetUI<Button>("Option1CheckBox");
        _option1Btn = GetUI<Button>("Option1Box");
        _option2CheckBtn = GetUI<Button>("Option2CheckBox");
        _option2Btn = GetUI<Button>("Option2Box");
        _confirmBtn = GetUI<Button>("ConfirmBtn"); 

        SetUICallback(_settingCloseBtn.gameObject, EUIEventState.Click, ClickCloseBtn);
        SetUICallback(_bgmMuteBtn.gameObject, EUIEventState.Click, ClickBGMMuteBtn);
        SetUICallback(_bgmMaxBtn.gameObject, EUIEventState.Click, ClickBGMMaxBtn);
        SetUICallback(_effectMuteBtn.gameObject, EUIEventState.Click, ClickEffectMuteBtn);
        SetUICallback(_effectMaxBtn.gameObject, EUIEventState.Click, ClickEffectMaxBtn);
        SetUICallback(_uiMuteBtn.gameObject, EUIEventState.Click, ClickUIMuteBtn);
        SetUICallback(_uiMaxBtn.gameObject, EUIEventState.Click, ClickUIMaxBtn);
        SetUICallback(_exitBtn.gameObject, EUIEventState.Click, ClickExitBtn);
        SetUICallback(_option1CheckBtn.gameObject, EUIEventState.Click, ClickOption1CheckBtn);
        SetUICallback(_option1Btn.gameObject, EUIEventState.Click, ClickOption1Btn);
        SetUICallback(_option2CheckBtn.gameObject, EUIEventState.Click, ClickOption2CheckBtn);
        SetUICallback(_option2Btn.gameObject, EUIEventState.Click, ClickoOption2Btn);
        SetUICallback(_confirmBtn.gameObject, EUIEventState.Click, ClickoConfirmBtn);

        _option1CheckBtn.gameObject.SetActive(PlayerSetting.BattleStartOption);
        _option1Btn.gameObject.SetActive(!PlayerSetting.BattleStartOption);

        _option2CheckBtn.gameObject.SetActive(PlayerSetting.DamageTextActive);
        _option2Btn.gameObject.SetActive(!PlayerSetting.DamageTextActive);
    }

    private void ClickoConfirmBtn(PointerEventData data)
    {
        PlayerSetting.BattleStartOption = _option1CheckBtn.gameObject.activeSelf;
        PlayerSetting.DamageTextActive = _option2CheckBtn.gameObject.activeSelf;

        int battleStartOption = PlayerSetting.BattleStartOption ? 1 : 0;
        int damageTextActive = PlayerSetting.DamageTextActive ? 1 : 0;

        PlayerPrefs.SetInt("BattleStartOption", battleStartOption);
        PlayerPrefs.SetInt("DamageTextActive", damageTextActive);
        PlayerPrefs.SetFloat("BgmVolume", soundManager.GetVolume(ESoundType.BGM));
        PlayerPrefs.SetFloat("EffectVolume", soundManager.GetVolume(ESoundType.Effect));
        PlayerPrefs.SetFloat("UIVolume", soundManager.GetVolume(ESoundType.UI));
        PlayerPrefs.Save();

        _ui.ClosePopup();
    }

    private void ClickoOption2Btn(PointerEventData data)
    {
        _option2CheckBtn.gameObject.SetActive(true);
        _option2Btn.gameObject.SetActive(false);
    }

    private void ClickOption2CheckBtn(PointerEventData data)
    {
        _option2CheckBtn.gameObject.SetActive(false);
        _option2Btn.gameObject.SetActive(true);
    }

    private void ClickOption1Btn(PointerEventData data)
    {
        _option1CheckBtn.gameObject.SetActive(true);
        _option1Btn.gameObject.SetActive(false);
    }

    private void ClickOption1CheckBtn(PointerEventData data)
    {
        _option1CheckBtn.gameObject.SetActive(false);
        _option1Btn.gameObject.SetActive(true);
    }

    private void SetSlider()
    {
        SetUI<Slider>();

        _bgmSlider = GetUI<Slider>("BGM_Slider");
        _effectSlider = GetUI<Slider>("Effect_Slider");
        _uiSlider = GetUI<Slider>("UI_Slider");

        _bgmSlider.onValueChanged.AddListener(BGMbar);
        _effectSlider.onValueChanged.AddListener(Effectbar);
        _uiSlider.onValueChanged.AddListener(UIbar);

        UpdateBGMBar(soundManager.GetVolume(ESoundType.BGM));
        UpdateEffectBar(soundManager.GetVolume(ESoundType.Effect));
        UpdateUIBar(soundManager.GetVolume(ESoundType.UI));
    }

    // todo : slider들(사운드 관련) value 값 조절

    private void BGMbar(float value)
    {
        soundManager.SetVolume(ESoundType.BGM, value);
    }

    private void Effectbar(float value)
    {
        soundManager.SetVolume(ESoundType.Effect, value);
    }

    private void UIbar(float value)
    {
        soundManager.SetVolume(ESoundType.UI, value);
    }

    private void ClickCloseBtn(PointerEventData EventData)
    {
        _ui.ClosePopup();
        Camera.main.GetComponent<CameraMovement>().Rock = false;
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

    private void UpdateBGMBar(float value)
    {
        _bgmSlider.value = value;
    }

    private void UpdateEffectBar(float value)
    {
        _effectSlider.value = value;
    }

    private void UpdateUIBar(float value)
    {
        _uiSlider.value = value;
    }

    private void ClickExitBtn(PointerEventData EventData)
    {
        Main.Get<GameManager>().ExitGame();
    }


}
