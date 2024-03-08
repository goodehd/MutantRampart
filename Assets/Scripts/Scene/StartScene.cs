using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : Scene
{
    protected override void Init()
    {
        Main.Get<UIManager>().OpenSceneUI<StartScene_SceneUI>();
        LoadPlayerSetting();
    }

    private void LoadPlayerSetting()
    {
        SoundManager soundManager = Main.Get<SoundManager>();

        if (PlayerPrefs.HasKey("BattleStartOption"))
        {
            PlayerSetting.BattleStartOption = PlayerPrefs.GetInt("BattleStartOption") == 1;
        }

        if (PlayerPrefs.HasKey("DamageTextActive"))
        {
            PlayerSetting.DamageTextActive = PlayerPrefs.GetInt("DamageTextActive") == 1;
        }

        if (PlayerPrefs.HasKey("BgmVolume"))
        {
            soundManager.SetVolume(ESoundType.BGM, PlayerPrefs.GetFloat("BgmVolume"));
        }

        if (PlayerPrefs.HasKey("EffectVolume"))
        {
            soundManager.SetVolume(ESoundType.Effect, PlayerPrefs.GetFloat("EffectVolume"));
        }

        if (PlayerPrefs.HasKey("UIVolume"))
        {
            soundManager.SetVolume(ESoundType.UI, PlayerPrefs.GetFloat("UIVolume"));
        }

        if (PlayerPrefs.HasKey("SortOption"))
        {
            PlayerSetting.SortSetting = (SortOption)PlayerPrefs.GetInt("SortOption");
        }
    }
}
