using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : Scene
{

    protected override void Init()
    {
        //Main.Get<UIManager>().OpenPopup<ShopUI>("Shop_PopupUI");
        Main.Get<UIManager>().OpenSceneUI<DayMainUI>("DayMain_SceneUI");
    }

    
}
