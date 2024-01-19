using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : Scene
{

    protected override void Init()
    {
        Main.Get<UIManager>().OpenPopup<Shop>("Shop");
    }

    
}
