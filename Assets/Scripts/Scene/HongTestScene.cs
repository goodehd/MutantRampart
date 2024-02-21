using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HongTestScene : Scene
{
    
    protected override void Init()
    {
        base.Init();
        Main.Get<UIManager>().OpenSceneUI<DayMain_SceneUI>();
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
        Main.Get<GameManager>().SaveData();
    }

    public void CreateEnemy()
    {
        CharacterBehaviour cha = CreateCharacter("Slime");
        cha.StateMachine.ChangeState(EState.Move);
    }
    
}
