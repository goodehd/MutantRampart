using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HongTestScene : Scene
{
    protected override void Init()
    {
        base.Init();
        Main.Get<TileManager>().GenerateMap();
        Main.Get<UIManager>().OpenSceneUI<DayMain_SceneUI>("DayMain_SceneUI");
        //Main.Get<UIManager>().OpenSceneUI<DayMainUI>("DayMain_SceneUI");
    }

    public void CreateEnemy()
    {
        CreateCharacter("Slime");
    }
}
