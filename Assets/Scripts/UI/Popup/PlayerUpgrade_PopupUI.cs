using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpgrade_PopupUI : BaseUI  
{
    private TextMeshProUGUI _upgradePointTxt;
    private TextMeshProUGUI _goldUpgradeLevelTxt;
    private Button _goldUpgradeBtn;

    public int UpgradePoint { get; set; }
    public int GoldUpgradeLevel { get; set; }

    protected override void Init()
    {
        SetUI<TextMeshProUGUI>();
        SetUI<Button>();

        _upgradePointTxt = GetUI<TextMeshProUGUI>("UpgradePointTxt");
        _goldUpgradeLevelTxt = GetUI<TextMeshProUGUI>("GoldUpgradeLevelTxt");

        _upgradePointTxt.text = $"{UpgradePoint}";
        _goldUpgradeLevelTxt.text = $"{GoldUpgradeLevel}";
    }
}
