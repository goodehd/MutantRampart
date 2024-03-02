using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

        _stageText.text = $"Day {_curStage}";
        _RewardsText.text = $"{_rewardsGold}gold";
    }

    private void ClickNextBtn(PointerEventData EventData)
    {
        Main.Get<GameManager>().SaveData();
        Main.Get<UIManager>().ClosePopup();
        Main.Get<UIManager>().OpenPopup<RewardSelect_PopupUI>();
        /*if ((_curStage-1) % 5 == 0 && _curStage != 0)
        {
            Main.Get<UIManager>().OpenPopup<RewardSelect_PopupUI>();
        }*/
    }
}
