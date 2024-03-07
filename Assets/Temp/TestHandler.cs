using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;

public class TestHandler : MonoBehaviour
{
    public bool StartSelect;
    public bool EndSelect;

    public bool IsStartClick;
    public bool IsEndClick;

    public Vector2 startPos;
    public Vector2 endPos;

    public Stack<Vector2> stack = new Stack<Vector2>();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            if (StartSelect)
            {
                startPos = worldPosition;
                IsStartClick = true;
                StartSelect = false;
            }

            if (EndSelect)
            {
                endPos = worldPosition;
                IsEndClick = true;
                EndSelect = false;
            }
             
            if(IsStartClick && IsEndClick)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                Main.Get<TileManager>().FindPath(startPos, endPos, out stack);

                stopwatch.Stop();
                UnityEngine.Debug.Log("함수 실행 소요 시간: " + stopwatch.ElapsedMilliseconds + "ms");

                IsStartClick = false;
                IsEndClick = false;
            }

            //(int resultX, int resultY) = Main.Get<TileManager>()._navigation.GetIndex(worldPosition);
            //UnityEngine.Debug.Log("Position (" + worldPosition.x + ", " + worldPosition.y + ") is at index (" + resultX + ", " + resultY + ")");
        }
    }

    private void OnDrawGizmos()
    {
        if(stack != null)
        {
            if(stack.Count > 0)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(startPos, stack.Peek());
                Vector2[] pathArray = stack.ToArray();
                for (int i = 1; i < pathArray.Length; i++)
                {
                    Gizmos.DrawLine(pathArray[i - 1], pathArray[i]);
                }
            }
        }
    }
}
