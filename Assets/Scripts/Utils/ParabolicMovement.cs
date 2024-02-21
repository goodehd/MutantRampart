using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;

public class ParabolicMovement : MonoBehaviour
{
    private Vector3 _startPos;
    private Vector3 _endPos;
    protected float timer;
    protected float timeToFloor;

    public event Action OnMovementEnd;

    protected static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }

    protected IEnumerator BulletMove()
    {
        timer = 0;
        while (transform.position.y >= _startPos.y)
        {
            timer += Time.deltaTime * 3f;
            Vector3 tempPos = Parabola(_startPos, _endPos, 0.5f, timer);
            transform.position = tempPos;
            yield return new WaitForEndOfFrame();
        }
        OnMovementEnd?.Invoke();
    }

    public void MovementStart()
    {
        StartCoroutine("BulletMove");
    }

    public void SetPos(Vector3 startPos, Vector3 endPos)
    {
        _startPos = startPos;
        _endPos = endPos;
    }
}