using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerUpgrade_PopupUI : BaseUI  
{
    private Image _descriptionBox;

    private TextMeshProUGUI _upgradePointTxt;
    private TextMeshProUGUI _goldUpgradeLevelTxt;
    private TextMeshProUGUI _hpUpgradeLevelTxt;
    private TextMeshProUGUI _wallUpgradeLevelTxt;
    private TextMeshProUGUI _upgradeNameTxt;
    private TextMeshProUGUI _upgradeLevelTxt;
    private TextMeshProUGUI _upgradeDescriptTxt;
    private TextMeshProUGUI _nextUpgradeLevelTxt;
    private TextMeshProUGUI _nextUpgradeDescriptTxt;
    private TextMeshProUGUI _upgradeNeedPointTxt;

    private Button _goldUpgradeBtn;
    private Button _hpUpgradeBtn;
    private Button _wallUpgradeBtn;
    private Button _upgradeYesBtn;
    private Button _upgradeNoBtn;
    private Button _playerUpgradeBackBtn;

    private int _upgradePoint;
    private int _goldUpgradeLevel;
    private int _hpUpgradeLevel;
    private int _wallUpgradeLevel;

    private bool _isGoldUpgradeClick;
    private bool _isHpUpgradeClick;
    private bool _iswallUpgradeClick;

    private UpgradeManager _upgradeManager;

    protected override void Init()
    {
        base.Init();

        _upgradeManager = Main.Get<UpgradeManager>();

        _upgradePoint = Main.Get<UpgradeManager>().UpgradePoint;
        _goldUpgradeLevel = Main.Get<UpgradeManager>().GoldUpgradeLevel;
        _hpUpgradeLevel = Main.Get<UpgradeManager>().HpUpgradeLevel;
        _wallUpgradeLevel = Main.Get<UpgradeManager>().WallUpgradeLevel;
        
        SetUI<TextMeshProUGUI>();
        SetUI<Button>();
        SetUI<Image>();

        _descriptionBox = GetUI<Image>("DescriptionBox");

        _upgradePointTxt = GetUI<TextMeshProUGUI>("UpgradePointTxt");
        _goldUpgradeLevelTxt = GetUI<TextMeshProUGUI>("GoldUpgradeLevelTxt");
        _hpUpgradeLevelTxt = GetUI<TextMeshProUGUI>("HpUpgradeLevelTxt");
        _wallUpgradeLevelTxt = GetUI<TextMeshProUGUI>("WallUpgradeLevelTxt");
        _upgradeNameTxt = GetUI<TextMeshProUGUI>("UpgradeNameTxt");
        _upgradeLevelTxt = GetUI<TextMeshProUGUI>("UpgradeLevelTxt");
        _upgradeDescriptTxt = GetUI<TextMeshProUGUI>("UpgradeDescriptTxt");
        _nextUpgradeLevelTxt = GetUI<TextMeshProUGUI>("NextUpgradeLevelTxt");
        _nextUpgradeDescriptTxt = GetUI<TextMeshProUGUI>("NextUpgradeDescriptTxt");
        _upgradeNeedPointTxt = GetUI<TextMeshProUGUI>("UpgradeNeedPointTxt");

        _goldUpgradeBtn = GetUI<Button>("GoldUpgradeBtn");
        _hpUpgradeBtn = GetUI<Button>("HpUpgradeBtn");
        _wallUpgradeBtn = GetUI<Button>("WallUpgradeBtn");
        _playerUpgradeBackBtn = GetUI<Button>("PlayerUpgradeBackBtn");
        _upgradeYesBtn = GetUI<Button>("UpgradeYesBtn");
        _upgradeNoBtn = GetUI<Button>("UpgradeNoBtn");

        UpdateText();
        SetIsUpgradeClickFalse();

        SetUICallback(_goldUpgradeBtn.gameObject, EUIEventState.Click, ClickGoldUpgradeBtn);
        SetUICallback(_playerUpgradeBackBtn.gameObject, EUIEventState.Click, ClickPlayerUpgradeBackBtn);
        SetUICallback(_upgradeYesBtn.gameObject, EUIEventState.Click, ClickUpgradeYesBtn);
        SetUICallback(_upgradeNoBtn.gameObject, EUIEventState.Click, ClickUpgradeNoBtn);

        

    }

    private void UpdateText()
    {
        _upgradePointTxt.text = $"{_upgradePoint}";
        _goldUpgradeLevelTxt.text = $"Lv.{_goldUpgradeLevel}";
        _hpUpgradeLevelTxt.text = $"Lv.{_hpUpgradeLevel}";
        _wallUpgradeLevelTxt.text = $"Lv.{_wallUpgradeLevel}";
    }

    private void ClickGoldUpgradeBtn(PointerEventData data)
    {
        _upgradeNameTxt.text = "Gold Upgrade";
        _upgradeLevelTxt.text = $"Lv.{_goldUpgradeLevel}";
        _upgradeDescriptTxt.text = $"골드 획득량 {(Main.Get<UpgradeManager>().UpgradeGoldPercent) * 100}% 증가";
        _nextUpgradeLevelTxt.text = $"Next -> Lv.{_goldUpgradeLevel + 1}";
        _nextUpgradeDescriptTxt.text = $"골드 획득량 {((Main.Get<UpgradeManager>().UpgradeGoldPercent) + 0.05f) * 100}% 증가";
        _upgradeNeedPointTxt.text = $"필요한 포인트 : {_goldUpgradeLevel}";
        _descriptionBox.gameObject.SetActive(true);
        _isGoldUpgradeClick = true;
    }

    private void ClickHpUpgradeBtn(PointerEventData data)
    {

    }
    private void ClickWallUpgradeBtn(PointerEventData data)
    {

    }

    private void ClickUpgradeYesBtn(PointerEventData data)
    {
        if (_isGoldUpgradeClick)
        {
            if (_upgradePoint >= _goldUpgradeLevel)
            {
                _upgradePoint -= _goldUpgradeLevel;
                _goldUpgradeLevel++;
                UpdateText();
                _upgradeManager.GoldUpgradeLevel = _goldUpgradeLevel;
                _upgradeManager.UpgradePoint = _upgradePoint;
            }
            else
            {
                Error_PopupUI ui = _ui.OpenPopup<Error_PopupUI>();
                ui.curErrorText = "업그레이드 포인트가 부족합니다.";
                return;
            }
            Main.Get<DataManager>().stageMonsterInfoList = new List<StageMonsterInfo>();
            Main.Get<DataManager>().CreateStageInfo();
            _descriptionBox.gameObject.SetActive(false);
        }
        Main.Get<UpgradeManager>().UpdateUpgradeGoldPercent();
        Main.Get<UpgradeManager>().SaveUpgrade();
        Main.Get<SaveDataManager>().SaveUpgradeData();
        SetIsUpgradeClickFalse();
    }
    private void ClickUpgradeNoBtn(PointerEventData data)
    {
        _descriptionBox.gameObject.SetActive(false);
        SetIsUpgradeClickFalse();
    }
    private void ClickPlayerUpgradeBackBtn(PointerEventData data)
    {
        _ui.ClosePopup();
    }

    private void SetIsUpgradeClickFalse()
    {
        _isGoldUpgradeClick = false;
        _isHpUpgradeClick = false;
        _iswallUpgradeClick = false;
    }
}
