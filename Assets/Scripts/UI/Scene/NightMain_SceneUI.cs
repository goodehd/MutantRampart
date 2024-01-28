using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NightMain_SceneUI : BaseUI
{
    private Image _backPanel;

    private Button _settingBtn;
    private Button _playBtn;
    private Button _x2SpeedBtn;
    private Button _pausedBtn;

    private TMP_Text _playerMoneyText;
    private TMP_Text _stageText;

    private Slider _hpSlider;

    protected override void Init()
    {
        SetUI<Image>();
        SetUI<Button>();
        SetUI<TMP_Text>();
        SetUI<Slider>();

        _backPanel = GetUI<Image>("NightMainBackPanel");
        _settingBtn = GetUI<Button>("NightMainSettingButton");
        _playBtn = GetUI<Button>("PlayButton");
        _x2SpeedBtn = GetUI<Button>("X2SpeedButton");
        _pausedBtn = GetUI<Button>("PausedButton");
        _playerMoneyText = GetUI<TMP_Text>("NightPlayerMoneyTxt");
        _stageText = GetUI<TMP_Text>("NightCurStageTxt");
        _hpSlider = GetUI<Slider>("HP_Slider");

        SetUICallback(_settingBtn.gameObject, EUIEventState.Click, ClickSettingBtn);
        SetUICallback(_playBtn.gameObject, EUIEventState.Click, ClickPlayBtn);
        SetUICallback(_x2SpeedBtn.gameObject, EUIEventState.Click, ClickMultiplySpeedBtn);
        SetUICallback(_pausedBtn.gameObject, EUIEventState.Click, ClickPausedBtn);

    }

    private void ClickSettingBtn(PointerEventData eventData)
    {
        Time.timeScale = 0.0f;
        Main.Get<UIManager>().OpenPopup< Setting_PopupUI>("Setting_PopupUI");
    }

    private void ClickPlayBtn(PointerEventData eventData)
    {
        _playBtn.gameObject.SetActive(false);
        _x2SpeedBtn.gameObject.SetActive(true);
        _pausedBtn.gameObject.SetActive(false);
        // todo ; 플레이 속도 1배속 -> 2배속으로 설정

    }
    private void ClickMultiplySpeedBtn(PointerEventData eventData)
    {
        _playBtn.gameObject.SetActive(false);
        _x2SpeedBtn.gameObject.SetActive(false);
        _pausedBtn.gameObject.SetActive(true);
        // todo ; 플레이 속도 2배속 -> 일시정지로 설정
        Time.timeScale = 0.0f;

    }
    private void ClickPausedBtn(PointerEventData eventData)
    {
        _playBtn.gameObject.SetActive(true);
        _x2SpeedBtn.gameObject.SetActive(false);
        _pausedBtn.gameObject.SetActive(false);
        // todo ; 플레이 속도 일시정지 -> 1배속으로 설정

    }



}
