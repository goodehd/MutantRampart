using DG.Tweening;
using UnityEngine;

public class HongTestScene : Scene
{
    private GameObject _unitArrow;
    private Tweener _tweener;

    protected override void Init()
    {
        base.Init();
        Main.Get<UIManager>().OpenSceneUI<DayMain_SceneUI>();
        Main.Get<SoundManager>().SoundPlay($"DayBGM", ESoundType.BGM);
        if (Main.Get<SaveDataManager>().isSaveFileExist)
        {
            Main.Get<SaveDataManager>().GenerateSaveMap();
        }
        else
        {
            Main.Get<TileManager>().GenerateMap(3, 3);
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
