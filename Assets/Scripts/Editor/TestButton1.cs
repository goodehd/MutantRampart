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
    }
}