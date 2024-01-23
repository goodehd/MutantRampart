using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HongTestScene : Scene
{
    protected override void Init()
    {
        Main.Get<TileManager>().GenerateMap();
        Main.Get<UIManager>().OpenPopup<ChangeSetButtons>("ChangeSetButtons");
    }

    public void CreateEnemy()
    {
        Main.Get<ResourceManager>().InstantiateWithPoolingOption("Prefabs/Enemy/SlimeBlue");
    }
}
