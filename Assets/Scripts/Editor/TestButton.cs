using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Character))]
public class TestButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Character generator = (Character)target;
        if (GUILayout.Button("Idle"))
        {
            generator.StateMachine.ChangeState(EState.Idle);
        }
        if (GUILayout.Button("Move"))
        {
            generator.StateMachine.ChangeState(EState.Move);
        }
        if (GUILayout.Button("Attack"))
        {
            generator.StateMachine.ChangeState(EState.Attack);
        }
        if (GUILayout.Button("Dead"))
        {
            generator.StateMachine.ChangeState(EState.Dead);
        }
    }
}