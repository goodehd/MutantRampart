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
        }
        else
        {
            UpgradePoint = 0;
            GoldUpgradeLevel = 1;
            HpUpgradeLevel = 1;
            WallUpgradeLevel = 1;
        }
        UpdateUpgradeGoldPercent();
        return true;
    }

    public void SaveUpgrade()
    {
        _saveDataManager.Upgrade.UpgradePoint = UpgradePoint;
        _saveDataManager.Upgrade.GoldUpgradeLevel = GoldUpgradeLevel;
        _saveDataManager.Upgrade.HpUpgradeLevel = HpUpgradeLevel;
        _saveDataManager.Upgrade.WallUpgradeLevel = WallUpgradeLevel;
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
}
