using UnityEngine;

public static class Utility 
{
    public static T GetAddComponent<T>(GameObject obj) where T : Component
    {
        T component = obj.GetComponent<T>();
        if (component == null)
        {
            component = obj.AddComponent<T>();
        }
        return component;
    }

    public static float ManhattanDistance(Vector2 pos1,  Vector2 pos2)
    {
        float distance = Mathf.Abs(pos2.x - pos1.x) + Mathf.Abs(pos2.y - pos1.y);
        return distance;
    }
}
