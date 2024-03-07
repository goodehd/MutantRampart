using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UpgradeManager : IManagers
{
    public int UpgradePoint { get; set; }
    public int GoldUpgradeLevel { get; set; }
    public int HpUpgradeLevel { get; set; }
    public int WallUpgradeLevel { get; set; }
    public int RewardUpgradeLevel { get; set; }
    public int GroundColUpgradeLevel { get; set; }
    public int GroundRowUpgradeLevel { get; set; }
    public int UpgradeRewardChance { get; set; }
    public int UpgradeMapSizeRow { get; set; }
    public int UpgradeMapSizeCol { get; set; }
    public float UpgradeGoldPercent { get; set; }

    private SaveDataManager _saveDataManager;

    public bool Init()
    {
        _saveDataManager = Main.Get<SaveDataManager>();

        if (File.Exists(_saveDataManager.UpgradeDataPath))
        {
            _saveDataManager.LoadUpgradeData();
            UpgradePoint = _saveDataManager.Upgrade.UpgradePoint;
            GoldUpgradeLevel = _saveDataManager.Upgrade.GoldUpgradeLevel;
            HpUpgradeLevel = _saveDataManager.Upgrade.HpUpgradeLevel;
            WallUpgradeLevel = _saveDataManager.Upgrade.WallUpgradeLevel;
            RewardUpgradeLevel = _saveDataManager.Upgrade.RewardUpgradeLevel;
            GroundColUpgradeLevel = _saveDataManager.Upgrade.GroundColUpgradeLevel;
            GroundRowUpgradeLevel = _saveDataManager.Upgrade.GroundRowUpgradeLevel;
        }
        else
        {
            UpgradePoint = 0;
            GoldUpgradeLevel = 1;
            HpUpgradeLevel = 1;
            WallUpgradeLevel = 1;
            RewardUpgradeLevel = 1;
            GroundColUpgradeLevel = 1;
            GroundRowUpgradeLevel = 1;
        }
        UpdateUpgradeGoldPercent();
        UpdateRewardChance();
        UpdateMapSizeCol();
        UpdateMapSizeRow();
        return true;
    }

    public void SaveUpgrade()
    {
        _saveDataManager.Upgrade.UpgradePoint = UpgradePoint;
        _saveDataManager.Upgrade.GoldUpgradeLevel = GoldUpgradeLevel;
        _saveDataManager.Upgrade.HpUpgradeLevel = HpUpgradeLevel;
        _saveDataManager.Upgrade.WallUpgradeLevel = WallUpgradeLevel;
        _saveDataManager.Upgrade.RewardUpgradeLevel = RewardUpgradeLevel;
        _saveDataManager.Upgrade.GroundColUpgradeLevel = GroundColUpgradeLevel;
        _saveDataManager.Upgrade.GroundRowUpgradeLevel = GroundRowUpgradeLevel;
    }

    public void UpdateUpgradeGoldPercent()
    {
        if(GoldUpgradeLevel == 1)
        {
            UpgradeGoldPercent = 0f;
        }
        else if(GoldUpgradeLevel > 1)
        {
            UpgradeGoldPercent = 0.05f;
        }
        else if(GoldUpgradeLevel > 2)
        {
            UpgradeGoldPercent = 0.1f;
        }
        else if(GoldUpgradeLevel > 3)
        {
            UpgradeGoldPercent = 0.15f;
        }
        else if(GoldUpgradeLevel > 4)
        {
            UpgradeGoldPercent = 0.2f;
        }
        else if(GoldUpgradeLevel > 5)
        {
            UpgradeGoldPercent = 0.25f;
        }
    }

    public void UpdateRewardChance()
    {
        if (RewardUpgradeLevel == 1)
        {
            UpgradeRewardChance = 20;
        }
        else if (RewardUpgradeLevel > 1)
        {
            UpgradeRewardChance = 25;
        }
        else if (RewardUpgradeLevel > 2)
        {
            UpgradeRewardChance = 30;
        }
        else if (GoldUpgradeLevel > 3)
        {
            UpgradeRewardChance = 35;
        }
        else if (RewardUpgradeLevel > 4)
        {
            UpgradeRewardChance = 40;
        }
        else if (RewardUpgradeLevel > 5)
        {
            UpgradeRewardChance = 45;
        }
    }

    public void UpdateMapSizeRow()
    {
        UpgradeMapSizeRow = 2 + GroundRowUpgradeLevel;
    }
    public void UpdateMapSizeCol()
    {
        UpgradeMapSizeCol = 2 + GroundColUpgradeLevel;
    }

}
