using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NextEnemy_ContentsUI : BaseUI
{
    public Monster Monsterinfo;
    private Image _monsterinfoImg;
    private TextMeshProUGUI _monsterCountText;

    // Start is called before the first frame update
    protected override void Init()
    {
        base.Init();
        SetUI<Image>();
        SetUI<TextMeshProUGUI>();
        _monsterinfoImg = GetUI<Image>("NextEnemy_ContentsUI");
        _monsterCountText = GetUI<TextMeshProUGUI>("Enemy_CountText");
        SetInfo();
    }
    private void SetInfo()
    {
        string monstername = Main.Get<DataManager>().Character[Monsterinfo.Name].PrefabName;
        _monsterinfoImg.sprite = Main.Get<ResourceManager>().Load<Sprite>($"{Literals.ENEMY_SPRITE_PATH}{monstername}");
        _monsterCountText.text = $"X{Monsterinfo.Count}";
    }
}

