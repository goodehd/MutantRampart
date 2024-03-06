using DG.Tweening;
using UnityEngine;

public class MainScene : Scene
{
    private GameObject _unitArrow;
    private Tweener _tweener;
    private int _groundColSize;
    private int _groundRowSize;
    protected override void Init()
    {
        base.Init();
        if (Main.Get<TutorialManager>().isTutorial)
        {
            Main.Get<TutorialManager>().CreateArrow();
        }

        _groundColSize = Main.Get<UpgradeManager>().UpgradeMapSizeCol;
        _groundRowSize = Main.Get<UpgradeManager>().UpgradeMapSizeRow;

        Main.Get<UIManager>().OpenSceneUI<DayMain_SceneUI>();
        Main.Get<SoundManager>().SoundPlay($"NightBGM", ESoundType.BGM);
        Main.Get<SoundManager>().SoundPlay($"DayBGM", ESoundType.BGM);
        if (Main.Get<SaveDataManager>().isSaveFileExist)
        {
            Main.Get<SaveDataManager>().GenerateSaveMap();
        }
        else
        {
            Main.Get<TileManager>().GenerateMap(_groundColSize, _groundRowSize);
        }

        _unitArrow = _resource.Instantiate("Prefabs/UnitArrow");
        _unitArrow.SetActive(false);
        SettingPool();
    }

    public void CreateEnemy()
    {
        CharacterBehaviour cha = CreateCharacter("Slime");
        cha.StateMachine.ChangeState(EState.Move);
    }

    public void ActiveUnitArrow(Vector2 pos)
    {
        _unitArrow.transform.position = pos;
        _unitArrow.SetActive(true);
        _tweener = _unitArrow.transform.DOMoveY(pos.y + 0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    public void InActiveUnitArrow()
    {
        _unitArrow.SetActive(false);
        _tweener.Kill();
    }

    private void SettingPool()
    {
        _pool.CreatePool($"{Literals.UNIT_PREFABS_PATH}Slime");
        _pool.CreatePool($"{Literals.UNIT_PREFABS_PATH}BigBull");
        _pool.CreatePool($"{Literals.UNIT_PREFABS_PATH}PlantBuger");
        _pool.CreatePool($"{Literals.UNIT_PREFABS_PATH}Snail");
        _pool.CreatePool($"{Literals.FX_PATH}ShamanFx1");
        _pool.CreatePool($"{Literals.FX_PATH}ShamanFx2");
    }
}
