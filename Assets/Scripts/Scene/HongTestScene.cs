public class HongTestScene : Scene
{

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

    }
    protected override void OnApplicationQuit()
    {
        if (!Main.Get<GameManager>().isTutorial)
        {
            Main.Get<GameManager>().SaveData();
        }
    }

    public void CreateEnemy()
    {
        CharacterBehaviour cha = CreateCharacter("Slime");
        cha.StateMachine.ChangeState(EState.Move);
    }

}
