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
        if (GUILayout.Button("hp -10"))
        {
            generator.Status.GetStat<Vital>(EstatType.Hp).CurValue -= 10;
        }
    }
}

[CustomEditor(typeof(Unit))]
public class TestButton2 : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Unit generator = (Unit)target;
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

