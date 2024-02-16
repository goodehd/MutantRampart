using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectScene : Scene
{
    protected override void Init()
    {
        Main.Get<UIManager>().OpenSceneUI<SelectScene_SceneUI>();
    }
}
