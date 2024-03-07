using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RewardSelect_PopupUI : BaseUI
{
    public enum RewardType
    {
        HealthGain50,
        HealthGain40,
        HealthGain30,
        HealthGain20,
        GoldGain500,
        GoldGain700,
        GoldGain1000,
        GoldGain1500,
        GoldLoss500,
        GoldLoss700,
        GoldLoss1000,

    }

    private TextMeshProUGUI _rewardTxt;
    private Button _yesBtn;
    private Button _noBtn;
    private Button _exitBtn;
    private float _maxHp;

    protected override void Init()
    {
        base.Init();

        _maxHp = Main.Get<GameManager>().PlayerMaxHp;

        SetUI<TextMeshProUGUI>();
        SetUI<Button>();

        _rewardTxt = GetUI<TextMeshProUGUI>("RewardTxt");

        _yesBtn = GetUI<Button>("YesBtn");
        _noBtn = GetUI<Button>("NoBtn");
        _exitBtn = GetUI<Button>("ExitBtn");

        _rewardTxt.color = Color.green;
        _rewardTxt.text = "";

        // 텍스트의 알파값을 0으로 초기화
        Color textColor = _rewardTxt.color;
        textColor.a = 0f;
        _rewardTxt.color = textColor;

        _exitBtn.gameObject.SetActive(false);

        SetUICallback(_yesBtn.gameObject, EUIEventState.Click, ClickYesBtn);
        SetUICallback(_noBtn.gameObject, EUIEventState.Click, ClickNoBtn);
        SetUICallback(_exitBtn.gameObject, EUIEventState.Click, ClickExitBtn);
    }

    public void SelectReward()
    {
        RewardType reward = GetRandomReward(); // 랜덤 보상 선택
        ApplyReward(reward); // 선택한 보상 적용
        Main.Get<GameManager>().SaveData();
    }

    private RewardType GetRandomReward()
    {
        return (RewardType)Random.Range(0, Enum.GetValues(typeof(RewardType)).Length);
    }

    private void ApplyReward(RewardType reward)
    {
        switch (reward)
        {
            case RewardType.HealthGain50:
                GainHealth(0.5f);
                _rewardTxt.text = "보상: 체력 50% 회복";
                FadeInRewardText(1f);
                break;
            case RewardType.HealthGain40:
                GainHealth(0.4f);
                _rewardTxt.text = "보상: 체력 40% 회복";
                FadeInRewardText(1f);
                break;
            case RewardType.HealthGain30:
                GainHealth(0.3f);
                _rewardTxt.text = "보상: 체력 30% 회복";
                FadeInRewardText(1f);
                break;
            case RewardType.HealthGain20:
                GainHealth(0.2f);
                _rewardTxt.text = "보상: 체력 20% 회복";
                FadeInRewardText(1f);
                break;
            case RewardType.GoldGain500:
                Main.Get<GameManager>().ChangeMoney(+500);
                _rewardTxt.text = "보상: 500 gold 획득";
                FadeInRewardText(1f);
                break;
            case RewardType.GoldGain700:
                Main.Get<GameManager>().ChangeMoney(+700);
                _rewardTxt.text = "보상: 700 gold 획득";
                FadeInRewardText(1f);
                break;
            case RewardType.GoldGain1000:
                Main.Get<GameManager>().ChangeMoney(+1000);
                _rewardTxt.text = "보상: 1000 gold 획득";
                FadeInRewardText(1f);
                break;
            case RewardType.GoldGain1500:
                Main.Get<GameManager>().ChangeMoney(+1500);
                _rewardTxt.text = "보상: 1500 gold 획득";
                FadeInRewardText(1f);
                break;
            case RewardType.GoldLoss500:
                Main.Get<GameManager>().ChangeMoney(-500);
                _rewardTxt.color = Color.red;
                _rewardTxt.text = "보상: 500 gold 감소";
                FadeInRewardText(1f);
                break;
            case RewardType.GoldLoss700:
                Main.Get<GameManager>().ChangeMoney(-700);
                _rewardTxt.color = Color.red;
                _rewardTxt.text = "보상: 700 gold 감소";
                FadeInRewardText(1f);
                break;
            case RewardType.GoldLoss1000:
                Main.Get<GameManager>().ChangeMoney(-1000);
                _rewardTxt.color = Color.red;
                _rewardTxt.text = "보상: 1000 gold 감소";
                FadeInRewardText(1f);
                break;
            default:
                break;
        }
    }

    private void ClickNoBtn(PointerEventData data)
    {
        _ui.ClosePopup();
    }

    private void ClickYesBtn(PointerEventData data)
    {
        _yesBtn.gameObject.SetActive(false);
        _noBtn.gameObject.SetActive(false);
        SelectReward();
        _exitBtn.gameObject.SetActive(true);
    }
    private void ClickExitBtn(PointerEventData data)
    {
        _ui.ClosePopup();
    }

    private void GainHealth(float percent)
    {
        float healAmount = _maxHp * percent;
        Main.Get<GameManager>().PlayerHP.CurValue += healAmount;
        if (Main.Get<GameManager>().PlayerHP.CurValue > _maxHp)
        {
            Main.Get<GameManager>().PlayerHP.CurValue = _maxHp;
        }
    }

    private void FadeInRewardText(float fadeInDuration)
    {
        _rewardTxt.DOFade(1f, fadeInDuration).From(0f);
    }
}
