using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScene : Scene
{
    protected override void Init()
    {
        base.Init();
        Main.Get<TileManager>().GenerateMap(3, 3);
        Main.Get<UIManager>().OpenSceneUI<DayMain_SceneUI>();

    }

    public void CreateEnemy()
    {
        CharacterBehaviour cha = CreateCharacter("Slime");
        cha.StateMachine.ChangeState(EState.Move);
    }
}
