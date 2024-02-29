using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : IManagers
{
    public int UpgradePoint { get; set; }
    public int GoldUpgradeLevel { get; set; }
    public float UpgradeGoldPercent { get; set; }
    public bool Init()
    {
        bool isGetUpgradePoint = PlayerPrefs.HasKey("UpgradePoint");
        bool isGetGoldUpgradeLevel = PlayerPrefs.HasKey("GoldUpgradeLevel");

        if (!isGetUpgradePoint)
        {
            UpgradePoint = 0;
        }
        else
        {
            UpgradePoint = PlayerPrefs.GetInt("UpgradePoint");
        }
        if (!isGetGoldUpgradeLevel)
        {
            GoldUpgradeLevel = 1;
        }
        else
        {
            GoldUpgradeLevel = PlayerPrefs.GetInt("GoldUpgradeLevel");
        }
        UpdateUpgradeGoldPercent();
        return true;
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
