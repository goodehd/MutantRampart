using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Vector3 touchPoint; //최초클릭의 위치
    [SerializeField] private Vector3 MousePoint; //현재 마우스의 위치

    // Update is called once per frame
    void FixedUpdate()
    {
        ViewMoving();
    }

    void ViewMoving()
    {
        MousePoint = Input.mousePosition;
        // 마우스 최초 클릭 시의 위치 기억
        if (Input.GetMouseButtonDown(0))
        {
            touchPoint = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        Vector3 move = touchPoint - MousePoint;
        transform.position += move * (Time.deltaTime * .5f);
        touchPoint = MousePoint;


        /*
        // (현재 마우스 위치 - 최초 위치)의 음의 방향으로 카메라 이동
        Vector2 position =
            Camera.main.ScreenToViewportPoint(-new Vector3(MousePoint.x - touchPoint.x, MousePoint.y - touchPoint.y,
                0));
        Vector2 move = position * (Time.deltaTime * 30f);
        Camera.main.transform.Translate(move);
        */
    }
}