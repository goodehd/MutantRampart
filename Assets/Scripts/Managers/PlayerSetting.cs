using System.Collections;
using System.Collections.Generic;

public enum SortOption
{
    levelAscend,
    levelDescend,
    NameAscend,
    NameDescend,
}

public static class PlayerSetting 
{
    public static bool BattleStartOption = true;
    public static bool DamageTextActive = true;
    public static SortOption SortSetting = SortOption.NameAscend;
    public static float BgmVolume = 1.0f;
    public static float EffectVolume = 1.0f;
    public static float UIVolume = 1.0f;
}
