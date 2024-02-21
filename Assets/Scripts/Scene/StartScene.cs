using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : Scene
{
    protected override void Init()
    {
        Main.Get<UIManager>().OpenSceneUI<StartScene_SceneUI>();
    }
}
