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
    private TextMeshProUGUI _rewardUpgradeLevelTxt;
    private TextMeshProUGUI _groundColUpgradeLevelTxt;
    private TextMeshProUGUI _groundRowUpgradeLevelTxt;
    private TextMeshProUGUI _upgradeNameTxt;
    private TextMeshProUGUI _upgradeLevelTxt;
    private TextMeshProUGUI _upgradeDescriptTxt;
    private TextMeshProUGUI _nextUpgradeLevelTxt;
    private TextMeshProUGUI _nextUpgradeDescriptTxt;
    private TextMeshProUGUI _upgradeNeedPointTxt;

    private Button _goldUpgradeBtn;
    private Button _hpUpgradeBtn;
    private Button _wallUpgradeBtn;
    private Button _rewardUpgradeBtn;
    private Button _groundColUpgradeBtn;
    private Button _groundRowUpgradeBtn;
    private Button _upgradeYesBtn;
    private Button _upgradeNoBtn;
    private Button _playerUpgradeBackBtn;
    private Button _resetBtn;

    private int _upgradePoint;
    private int _goldUpgradeLevel;
    private int _hpUpgradeLevel;
    private int _wallUpgradeLevel;
    private int _rewardUpgradeLevel;
    private int _groundColUpgradeLevel;
    private int _groundRowUpgradeLevel;

    private bool _isGoldUpgradeClick;
    private bool _isHpUpgradeClick;
    private bool _isWallUpgradeClick;
    private bool _isRewardUpgradeClick;
    private bool _isGroundColUpgradeClick;
    private bool _isGroundRowUpgradeClick;

    private UpgradeManager _upgradeManager;

    protected override void Init()
    {
        base.Init();

        _upgradeManager = Main.Get<UpgradeManager>();

        SetUpgradeLevel();
        
        SetUI<TextMeshProUGUI>();
        SetUI<Button>();
        SetUI<Image>();

        _descriptionBox = GetUI<Image>("DescriptionBox");

        _upgradePointTxt = GetUI<TextMeshProUGUI>("UpgradePointTxt");
        _goldUpgradeLevelTxt = GetUI<TextMeshProUGUI>("GoldUpgradeLevelTxt");
        _hpUpgradeLevelTxt = GetUI<TextMeshProUGUI>("HpUpgradeLevelTxt");
        _wallUpgradeLevelTxt = GetUI<TextMeshProUGUI>("WallUpgradeLevelTxt");
        _rewardUpgradeLevelTxt = GetUI<TextMeshProUGUI>("RewardUpgradeLevelTxt");
        _groundColUpgradeLevelTxt = GetUI<TextMeshProUGUI>("GroundColUpgradeLevelTxt");
        _groundRowUpgradeLevelTxt = GetUI<TextMeshProUGUI>("GroundRowUpgradeLevelTxt");
        _upgradeNameTxt = GetUI<TextMeshProUGUI>("UpgradeNameTxt");
        _upgradeLevelTxt = GetUI<TextMeshProUGUI>("UpgradeLevelTxt");
        _upgradeDescriptTxt = GetUI<TextMeshProUGUI>("UpgradeDescriptTxt");
        _nextUpgradeLevelTxt = GetUI<TextMeshProUGUI>("NextUpgradeLevelTxt");
        _nextUpgradeDescriptTxt = GetUI<TextMeshProUGUI>("NextUpgradeDescriptTxt");
        _upgradeNeedPointTxt = GetUI<TextMeshProUGUI>("UpgradeNeedPointTxt");

        _goldUpgradeBtn = GetUI<Button>("GoldUpgradeBtn");
        _hpUpgradeBtn = GetUI<Button>("HpUpgradeBtn");
        _wallUpgradeBtn = GetUI<Button>("WallUpgradeBtn");
        _rewardUpgradeBtn = GetUI<Button>("RewardUpgradeBtn");
        _groundColUpgradeBtn = GetUI<Button>("GroundColUpgradeBtn");
        _groundRowUpgradeBtn = GetUI<Button>("GroundRowUpgradeBtn");
        _playerUpgradeBackBtn = GetUI<Button>("PlayerUpgradeBackBtn");
        _upgradeYesBtn = GetUI<Button>("UpgradeYesBtn");
        _upgradeNoBtn = GetUI<Button>("UpgradeNoBtn");
        _resetBtn = GetUI<Button>("ResetBtn");

        UpdateText();
        SetIsUpgradeClickFalse();

        SetUICallback(_goldUpgradeBtn.gameObject, EUIEventState.Click, ClickGoldUpgradeBtn);
        SetUICallback(_hpUpgradeBtn.gameObject, EUIEventState.Click, ClickHpUpgradeBtn);
        SetUICallback(_wallUpgradeBtn.gameObject, EUIEventState.Click, ClickWallUpgradeBtn);
        SetUICallback(_rewardUpgradeBtn.gameObject, EUIEventState.Click, ClickRewardUpgradeBtn);
        SetUICallback(_groundColUpgradeBtn.gameObject, EUIEventState.Click, ClickGroundColUpgradeBtn);
        SetUICallback(_groundRowUpgradeBtn.gameObject, EUIEventState.Click, ClickGroundRowUpgradeBtn);
        SetUICallback(_playerUpgradeBackBtn.gameObject, EUIEventState.Click, ClickPlayerUpgradeBackBtn);
        SetUICallback(_upgradeYesBtn.gameObject, EUIEventState.Click, ClickUpgradeYesBtn);
        SetUICallback(_upgradeNoBtn.gameObject, EUIEventState.Click, ClickUpgradeNoBtn);
        SetUICallback(_resetBtn.gameObject, EUIEventState.Click, ClickResetBtn);
    }

    private void UpdateText()
    {
        _upgradePointTxt.text = $"{_upgradePoint}";
        _goldUpgradeLevelTxt.text = $"Lv.{_goldUpgradeLevel}";
        _hpUpgradeLevelTxt.text = $"Lv.{_hpUpgradeLevel}";
        _wallUpgradeLevelTxt.text = $"Lv.{_wallUpgradeLevel}";
        _rewardUpgradeLevelTxt.text = $"Lv.{_rewardUpgradeLevel}";
        _groundColUpgradeLevelTxt.text = $"Lv.{_groundColUpgradeLevel}";
        _groundRowUpgradeLevelTxt.text = $"Lv.{_groundRowUpgradeLevel}";
    }

    private void ClickGoldUpgradeBtn(PointerEventData data)
    {
        _upgradeNameTxt.text = "Gold Upgrade";
        
        _upgradeLevelTxt.text = $"Lv.{_goldUpgradeLevel}";
        _upgradeDescriptTxt.text = $"골드 획득량 {_upgradeManager.UpgradeGoldPercent * 100}% 증가";
        _nextUpgradeLevelTxt.text = $"Next -> Lv.{_goldUpgradeLevel + 1}";
        _nextUpgradeDescriptTxt.text = $"골드 획득량 {(_upgradeManager.UpgradeGoldPercent + 0.05f) * 100}% 증가";
        _upgradeNeedPointTxt.text = $"필요한 포인트 : {_goldUpgradeLevel}";
        _descriptionBox.gameObject.SetActive(true);
        _isGoldUpgradeClick = true;
    }

    private void ClickHpUpgradeBtn(PointerEventData data)
    {
        _upgradeNameTxt.text = "Hp Upgrade";
        _upgradeLevelTxt.text = $"Lv.{_hpUpgradeLevel}";
        _upgradeDescriptTxt.text = $"플레이어 체력 {_hpUpgradeLevel*_hpUpgradeLevel} 증가\n" +
                                   $"(업그레이드 된 체력 : {5 + _hpUpgradeLevel * _hpUpgradeLevel})";
        if(_hpUpgradeLevel == 1) _upgradeDescriptTxt.text = $"플레이어 기본 체력 : 5";
        _nextUpgradeLevelTxt.text = $"Next -> Lv.{_hpUpgradeLevel + 1}";
        _nextUpgradeDescriptTxt.text = $"플레이어 체력 {(_hpUpgradeLevel+1) *(_hpUpgradeLevel+1)} 증가\n" +
                                       $"(업그레이드 된 체력 : {5 + (_hpUpgradeLevel + 1) * (_hpUpgradeLevel + 1)})";
        _upgradeNeedPointTxt.text = $"필요한 포인트 : {_hpUpgradeLevel}";
        _descriptionBox.gameObject.SetActive(true);
        _isHpUpgradeClick = true;
    }
    private void ClickWallUpgradeBtn(PointerEventData data)
    {
        _upgradeNameTxt.text = "Wall Upgrade";
        _upgradeLevelTxt.text = $"Lv.{_wallUpgradeLevel}";
        _upgradeDescriptTxt.text = $"타일 벽 생성 가능 {(_wallUpgradeLevel * 3)} 개 ";
        _nextUpgradeLevelTxt.text = $"Next -> Lv.{_wallUpgradeLevel + 1}";
        _nextUpgradeDescriptTxt.text = $"타일 벽 생성 가능 {((_wallUpgradeLevel + 1) * 3)} 개";
        _upgradeNeedPointTxt.text = $"필요한 포인트 : {_wallUpgradeLevel}";
        _descriptionBox.gameObject.SetActive(true);
        _isWallUpgradeClick = true;
    }

    private void ClickRewardUpgradeBtn(PointerEventData data)
    {
        _upgradeNameTxt.text = "Reward Upgrade";
        _upgradeLevelTxt.text = $"Lv.{_rewardUpgradeLevel}";
        _upgradeDescriptTxt.text = $"클리어 시 추가보상 확률 {_upgradeManager.UpgradeRewardChance}%";
        _nextUpgradeLevelTxt.text = $"Next -> Lv.{_rewardUpgradeLevel + 1}";
        _nextUpgradeDescriptTxt.text = $"클리어 시 추가보상 확률  {_upgradeManager.UpgradeRewardChance + 5}%";
        _upgradeNeedPointTxt.text = $"필요한 포인트 : {_rewardUpgradeLevel}";
        _descriptionBox.gameObject.SetActive(true);
        _isRewardUpgradeClick = true;
    }

    private void ClickGroundColUpgradeBtn(PointerEventData data)
    {
        if (_groundColUpgradeLevel > _groundRowUpgradeLevel)
        {
            Error_PopupUI ui = _ui.OpenPopup<Error_PopupUI>();
            ui.curErrorText = "Ground Row Upgrade가 필요합니다.";
            return;
        }
        _upgradeNameTxt.text = "GroundCol Upgrade";
        _upgradeLevelTxt.text = $"Lv.{_groundColUpgradeLevel}";
        _upgradeDescriptTxt.text = $"맵 크기 {_upgradeManager.UpgradeMapSizeCol}x{_upgradeManager.UpgradeMapSizeRow}";
        _nextUpgradeLevelTxt.text = $"Next -> Lv.{_groundColUpgradeLevel + 1}";
        _nextUpgradeDescriptTxt.text = $"맵 크기 {_upgradeManager.UpgradeMapSizeCol + 1}x{_upgradeManager.UpgradeMapSizeRow}";
        _upgradeNeedPointTxt.text = $"필요한 포인트 : {_groundColUpgradeLevel * 2}";
        _descriptionBox.gameObject.SetActive(true);
        _isGroundColUpgradeClick = true;
    }

    private void ClickGroundRowUpgradeBtn(PointerEventData data)
    {
        if (_groundRowUpgradeLevel > _groundColUpgradeLevel)
        {
            Error_PopupUI ui = _ui.OpenPopup<Error_PopupUI>();
            ui.curErrorText = "Ground Col Upgrade가 필요합니다.";
            return;
        }
        _upgradeNameTxt.text = "GroundRow Upgrade";
        _upgradeLevelTxt.text = $"Lv.{_groundRowUpgradeLevel}";
        _upgradeDescriptTxt.text = $"맵 크기 {_upgradeManager.UpgradeMapSizeCol}x{_upgradeManager.UpgradeMapSizeRow}";
        _nextUpgradeLevelTxt.text = $"Next -> Lv.{_groundRowUpgradeLevel + 1}";
        _nextUpgradeDescriptTxt.text = $"맵 크기 {_upgradeManager.UpgradeMapSizeCol}x{_upgradeManager.UpgradeMapSizeRow + 1}";
        _upgradeNeedPointTxt.text = $"필요한 포인트 : {_groundRowUpgradeLevel * 2}";
        _descriptionBox.gameObject.SetActive(true);
        _isGroundRowUpgradeClick = true;
    }

    private void ClickUpgradeYesBtn(PointerEventData data)
    {
        if (Main.Get<SaveDataManager>().isSaveFileExist)
        {
            Error_PopupUI ui = _ui.OpenPopup<Error_PopupUI>();
            ui.curErrorText = "저장된 데이터가 있을땐\n업그레이드가 불가능합니다.";
            return;
        }
        if (_isGoldUpgradeClick)
        {
            if (_upgradePoint >= _goldUpgradeLevel)
            {
                _upgradePoint -= _goldUpgradeLevel;
                _goldUpgradeLevel++;
                UpdateText();
                _upgradeManager.GoldUpgradeLevel = _goldUpgradeLevel;
                _upgradeManager.UpgradePoint = _upgradePoint;
                SetUpgradeLevel();
                _upgradeManager.UpdateUpgradeGoldPercent();
            }
            else
            {
                OpenNotEnoughPopUp();
            }
            Main.Get<DataManager>().stageMonsterInfoList = new List<StageMonsterInfo>();
            Main.Get<DataManager>().CreateStageInfo();
        }
        else if (_isHpUpgradeClick)
        {
            if (_upgradePoint >= _hpUpgradeLevel)
            {
                _upgradePoint -= _hpUpgradeLevel;
                _hpUpgradeLevel++;
                UpdateText();
                _upgradeManager.HpUpgradeLevel = _hpUpgradeLevel;
                _upgradeManager.UpgradePoint = _upgradePoint;
                SetUpgradeLevel();
                Main.Get<GameManager>().SetPlayerHp();
            }
            else
            {
                OpenNotEnoughPopUp();
            }
        }
        else if (_isWallUpgradeClick)
        {
            if (_upgradePoint >= _wallUpgradeLevel)
            {
                _upgradePoint -= _wallUpgradeLevel;
                _wallUpgradeLevel++;
                UpdateText();
                _upgradeManager.WallUpgradeLevel = _wallUpgradeLevel;
                _upgradeManager.UpgradePoint = _upgradePoint;
                SetUpgradeLevel();
                Main.Get<TileManager>().UpdateWallCount();
            }
            else
            {
                OpenNotEnoughPopUp();
            }
        }
        else if (_isRewardUpgradeClick)
        {
            if (_upgradePoint >= _rewardUpgradeLevel)
            {
                _upgradePoint -= _rewardUpgradeLevel;
                _rewardUpgradeLevel++;
                UpdateText();
                _upgradeManager.RewardUpgradeLevel = _rewardUpgradeLevel;
                _upgradeManager.UpgradePoint = _upgradePoint;
                SetUpgradeLevel();
                _upgradeManager.UpdateRewardChance();
            }
            else
            {
                OpenNotEnoughPopUp();
            }
        }
        else if (_isGroundColUpgradeClick)
        {
            int groundColUpgradePoint = _groundColUpgradeLevel * 2;
            if (_upgradePoint >= groundColUpgradePoint)
            {
                _upgradePoint -= groundColUpgradePoint;
                _groundColUpgradeLevel ++;
                UpdateText();
                _upgradeManager.GroundColUpgradeLevel = _groundColUpgradeLevel;
                _upgradeManager.UpgradePoint = _upgradePoint;
                SetUpgradeLevel();
                _upgradeManager.UpdateMapSizeCol();
            }
            else
            {
                OpenNotEnoughPopUp();
            }
        }
        else if (_isGroundRowUpgradeClick)
        {
            int groundRowUpgradePoint = _groundRowUpgradeLevel * 2;
            if (_upgradePoint >= groundRowUpgradePoint)
            {
                _upgradePoint -= groundRowUpgradePoint;
                _groundRowUpgradeLevel++;
                UpdateText();
                _upgradeManager.GroundRowUpgradeLevel = _groundRowUpgradeLevel;
                _upgradeManager.UpgradePoint = _upgradePoint;
                SetUpgradeLevel();
                _upgradeManager.UpdateMapSizeRow();
            }
            else
            {
                OpenNotEnoughPopUp();
            }
        }
        _descriptionBox.gameObject.SetActive(false);
        _upgradeManager.SaveUpgrade();
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
    private void ClickResetBtn(PointerEventData data)
    {
        if (Main.Get<SaveDataManager>().isSaveFileExist)
        {
            Error_PopupUI ui = _ui.OpenPopup<Error_PopupUI>();
            ui.curErrorText = "저장된 데이터가 있을땐\n초기화가 불가능합니다.";
            return;
        }
        _upgradePoint = (_goldUpgradeLevel - 1) + (_hpUpgradeLevel - 1) + (_wallUpgradeLevel - 1) + 
                        (_rewardUpgradeLevel - 1) + (_groundColUpgradeLevel - 1) + (_groundRowUpgradeLevel - 1);
        _upgradeManager.UpgradePoint += _upgradePoint;
        _upgradeManager.ResetUpgrade();
        SetUpgradeLevel();
        UpdateText();
    }

    private void SetIsUpgradeClickFalse()
    {
        _isGoldUpgradeClick = false;
        _isHpUpgradeClick = false;
        _isWallUpgradeClick = false;
        _isRewardUpgradeClick = false;
        _isGroundColUpgradeClick = false;
        _isGroundRowUpgradeClick = false;
    }
    private void OpenNotEnoughPopUp()
    {
        Error_PopupUI ui = _ui.OpenPopup<Error_PopupUI>();
        ui.curErrorText = "업그레이드 포인트가 부족합니다.";
        return;
    }
    
    private void SetUpgradeLevel()
    {
        _upgradePoint = Main.Get<UpgradeManager>().UpgradePoint;
        _goldUpgradeLevel = Main.Get<UpgradeManager>().GoldUpgradeLevel;
        _hpUpgradeLevel = Main.Get<UpgradeManager>().HpUpgradeLevel;
        _wallUpgradeLevel = Main.Get<UpgradeManager>().WallUpgradeLevel;
        _rewardUpgradeLevel = Main.Get<UpgradeManager>().RewardUpgradeLevel;
        _groundColUpgradeLevel = Main.Get<UpgradeManager>().GroundColUpgradeLevel;
        _groundRowUpgradeLevel = Main.Get<UpgradeManager>().GroundRowUpgradeLevel;
    }

   
}
