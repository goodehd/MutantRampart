using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Enemy))]
public class TestButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Enemy generator = (Enemy)target;
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