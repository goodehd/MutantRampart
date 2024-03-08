using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PastUpgrade_PopupUI : BaseUI
{
    private Button _upgradeConfirmButton;
    private Button _upgradeCancelBtn;

    private TMP_Text _curUpgradeTxt;
    private TMP_Text _nextUpgradeTxt;
    private TMP_Text _priceTxt;

    public string _upgradeCurTxt { get; set; }
    public string _upgradeNextTxt { get; set; }
    public string _upgradePriceTxt { get; set; }

    protected override void Init()
    {
        SetUI<Button>();
        SetUI<TMP_Text>();

        _upgradeConfirmButton = GetUI<Button>("UpgradeConfirmBtn");
        _upgradeCancelBtn = GetUI<Button>("UpgradeCancelBtn");

        SetUICallback(_upgradeConfirmButton.gameObject, EUIEventState.Click, ClickUpgradeConfirmButton);
        SetUICallback(_upgradeCancelBtn.gameObject, EUIEventState.Click, ClickUpgradeCancelButton);

        _curUpgradeTxt = GetUI<TMP_Text>("CurTxt");
        _nextUpgradeTxt = GetUI<TMP_Text>("NextTxt");
        _priceTxt = GetUI<TMP_Text>("PriceTxt");

        _curUpgradeTxt.text = _upgradeCurTxt;
        _nextUpgradeTxt.text = _upgradeNextTxt;
        _priceTxt.text = _upgradePriceTxt;

    }


    private void ClickUpgradeConfirmButton(PointerEventData EventData)
    {
        // 돈 차감 ,  업그레이드 내용 적용
    }

    private void ClickUpgradeCancelButton(PointerEventData EventData)
    {
        Main.Get<UIManager>().ClosePopup();
    }

}
