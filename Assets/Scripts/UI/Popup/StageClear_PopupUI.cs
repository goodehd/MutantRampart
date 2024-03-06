using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageClear_PopupUI : BaseUI
{
    private TextMeshProUGUI _stageText;
    private TextMeshProUGUI _RewardsText;
    private Button _nextBtn;

    public int _curStage {  get; set; }
    public int _rewardsGold { get; set; }

    protected override void Init()
    {
        SetUI<TextMeshProUGUI>();
        SetUI<Button>();

        _stageText = GetUI<TextMeshProUGUI>("StageClearLevelText");
        _RewardsText = GetUI<TextMeshProUGUI>("ClearRewardMoneyText");
        _nextBtn = GetUI<Button>("StageClearContinueBtn");

        SetUICallback(_nextBtn.gameObject, EUIEventState.Click, ClickNextBtn);

        _stageText.text = $"Day {_curStage -1}";
        _RewardsText.text = $"{_rewardsGold}gold";

        //만약 골드 업그레이드를 했다면
        if (Main.Get<UpgradeManager>().GoldUpgradeLevel > 1)
        {
            _RewardsText.text = $"{_rewardsGold} + ({(int)(_rewardsGold * Main.Get<UpgradeManager>().UpgradeGoldPercent)})gold";
        }
    }

    private void ClickNextBtn(PointerEventData EventData)
    {
        Main.Get<GameManager>().SaveData();
        Main.Get<UIManager>().ClosePopup();

        Random random = new Random();
        int randomNumber = random.Next(0, 100);
        if(randomNumber < Main.Get<UpgradeManager>().UpgradeRewardChance)
        {
            Main.Get<UIManager>().OpenPopup<RewardSelect_PopupUI>();
        }
        /*if ((_curStage-1) % 5 == 0 && _curStage != 0)
        {
            Main.Get<UIManager>().OpenPopup<RewardSelect_PopupUI>();
        }*/
    }
}
