using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerUpgrade_PopupUI : BaseUI  
{
    private Image _descriptionBox;
    private TextMeshProUGUI _goldUpgradeTxt;
    private TextMeshProUGUI _upgradePointTxt;
    private TextMeshProUGUI _goldUpgradeLevelTxt;
    private TextMeshProUGUI _upgradeNameTxt;
    private TextMeshProUGUI _upgradeLevelTxt;
    private TextMeshProUGUI _upgradeDescriptTxt;
    private TextMeshProUGUI _nextUpgradeLevelTxt;
    private TextMeshProUGUI _nextUpgradeDescriptTxt;
    private TextMeshProUGUI _upgradeNeedPointTxt;
    private Button _goldUpgradeBtn;
    private Button _upgradeYesBtn;
    private Button _upgradeNoBtn;
    private Button _playerUpgradeBackBtn;

    private int _upgradePoint;
    private int _goldUpgradeLevel;

    protected override void Init()
    {
        base.Init();
        _upgradePoint = Main.Get<UpgradeManager>().UpgradePoint;
        _goldUpgradeLevel = Main.Get<UpgradeManager>().GoldUpgradeLevel;
        
        SetUI<TextMeshProUGUI>();
        SetUI<Button>();
        SetUI<Image>();

        _descriptionBox = GetUI<Image>("DescriptionBox");

        _goldUpgradeTxt = GetUI<TextMeshProUGUI>("GoldUpgradeTxt");
        _upgradePointTxt = GetUI<TextMeshProUGUI>("UpgradePointTxt");
        _goldUpgradeLevelTxt = GetUI<TextMeshProUGUI>("GoldUpgradeLevelTxt");
        _upgradeNameTxt = GetUI<TextMeshProUGUI>("UpgradeNameTxt");
        _upgradeLevelTxt = GetUI<TextMeshProUGUI>("UpgradeLevelTxt");
        _upgradeDescriptTxt = GetUI<TextMeshProUGUI>("UpgradeDescriptTxt");
        _nextUpgradeLevelTxt = GetUI<TextMeshProUGUI>("NextUpgradeLevelTxt");
        _nextUpgradeDescriptTxt = GetUI<TextMeshProUGUI>("NextUpgradeDescriptTxt");
        _upgradeNeedPointTxt = GetUI<TextMeshProUGUI>("UpgradeNeedPointTxt");

        _goldUpgradeBtn = GetUI<Button>("GoldUpgradeBtn");
        _playerUpgradeBackBtn = GetUI<Button>("PlayerUpgradeBackBtn");
        _upgradeYesBtn = GetUI<Button>("UpgradeYesBtn");
        _upgradeNoBtn = GetUI<Button>("UpgradeNoBtn");

        UpdateText();

        SetUICallback(_goldUpgradeBtn.gameObject, EUIEventState.Click, ClickGoldUpgradeBtn);
        SetUICallback(_playerUpgradeBackBtn.gameObject, EUIEventState.Click, ClickPlayerUpgradeBackBtn);
        SetUICallback(_upgradeYesBtn.gameObject, EUIEventState.Click, ClickUpgradeYesBtn);
        SetUICallback(_upgradeNoBtn.gameObject, EUIEventState.Click, ClickUpgradeNoBtn);


    }

    private void UpdateText()
    {
        _upgradePointTxt.text = $"{_upgradePoint}";
        _goldUpgradeLevelTxt.text = $"Lv.{_goldUpgradeLevel}";
    }

    private void ClickGoldUpgradeBtn(PointerEventData data)
    {
        SetGoldDescriptionInfo("Gold Upgrade");
        _descriptionBox.gameObject.SetActive(true);
    }

    private void SetGoldDescriptionInfo(string upgradeName)
    {
        _upgradeNameTxt.text = upgradeName;
        _upgradeLevelTxt.text = $"Lv.{_goldUpgradeLevel}";
        _upgradeDescriptTxt.text = $"골드 획득량 {(Main.Get<UpgradeManager>().UpgradeGoldPercent)*100}% 증가";
        _nextUpgradeLevelTxt.text = $"Next -> Lv.{_goldUpgradeLevel + 1}";
        _nextUpgradeDescriptTxt.text = $"골드 획득량 {((Main.Get<UpgradeManager>().UpgradeGoldPercent)+0.05f) * 100}% 증가";
        _upgradeNeedPointTxt.text = $"필요한 포인트 : {_goldUpgradeLevel}";
    }

    private void ClickUpgradeYesBtn(PointerEventData data)
    {
        if (_upgradePoint >= _goldUpgradeLevel)
        {
            _upgradePoint -= _goldUpgradeLevel;
            _goldUpgradeLevel++;
            UpdateText();
            PlayerPrefs.SetInt("GoldUpgradeLevel", _goldUpgradeLevel);
            PlayerPrefs.SetInt("UpgradePoint", _upgradePoint);
        }
        else
        {
            Error_PopupUI ui = _ui.OpenPopup<Error_PopupUI>();
            ui.curErrorText = "업그레이드 포인트가 부족합니다.";
            return;
        }
        Main.Get<UpgradeManager>().Init();
        Main.Get<DataManager>().stageMonsterInfoList = new List<StageMonsterInfo>();
        Main.Get<DataManager>().CreateStageInfo();
        Debug.Log($"{Main.Get<UpgradeManager>().UpgradeGoldPercent}");
        _descriptionBox.gameObject.SetActive(false);
    }
    private void ClickUpgradeNoBtn(PointerEventData data)
    {
        _descriptionBox.gameObject.SetActive(false);
    }
    private void ClickPlayerUpgradeBackBtn(PointerEventData data)
    {
        _ui.ClosePopup();
    }
}
