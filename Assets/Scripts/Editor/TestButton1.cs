using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HongTestScene))]
public class TestButton1 : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        HongTestScene generator = (HongTestScene)target;
        
        if (GUILayout.Button("Spwn"))
        {
            generator.CreateEnemy();
        }

        if (GUILayout.Button("Row"))
        {
            Main.Get<TileManager>().ExpandMapRow();
        }

        if (GUILayout.Button("Col"))
        {
            Main.Get<TileManager>().ExpandMapCol();
        }
    }
}

[CustomEditor(typeof(DefaultTile))]
public class TestButton3 : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DefaultTile generator = (DefaultTile)target;

        if (GUILayout.Button("RightTop"))
        {
            Main.Get<TileManager>().SetRoomDir(generator, ERoomDir.RightTop, !generator.IsDoorOpen(ERoomDir.RightTop));
        }

        if (GUILayout.Button("RightBottom"))
        {
            Main.Get<TileManager>().SetRoomDir(generator, ERoomDir.RightBottom, !generator.IsDoorOpen(ERoomDir.RightBottom));
        }

        if (GUILayout.Button("LeftTop"))
        {
            Main.Get<TileManager>().SetRoomDir(generator, ERoomDir.LeftTop, !generator.IsDoorOpen(ERoomDir.LeftTop));
        }

        if (GUILayout.Button("LeftBottom"))
        {
            Main.Get<TileManager>().SetRoomDir(generator, ERoomDir.LeftBottom, !generator.IsDoorOpen(ERoomDir.LeftBottom));
        }
    }
}

