using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Vector3 Origin;
    [SerializeField] private Vector3 Diference;
    [SerializeField] private bool Drag = false;

    void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Diference = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;
            if (Drag == false)
            {
                Drag = true;
                Origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            Drag = false;
        }

        if (Drag == true)
        {
            Camera.main.transform.position = Origin - Diference;
        }
        
    }
    /*
    [SerializeField] private Vector3 touchPoint; //최초클릭의 위치
    [SerializeField] private Vector3 MousePoint; //현재 마우스의 위치
    [SerializeField] private bool isDragging = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        MousePoint = Input.mousePosition;
        ViewMoving();
    }

    void ViewMoving()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchPoint = Input.mousePosition;
            isDragging = true;
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            return;
        }

        if (isDragging)
        {
            Vector3 difference = touchPoint - MousePoint;
            transform.position +=  touchPoint * (Time.deltaTime * 0.1f);
            touchPoint = MousePoint;
        }


        /*
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

    }
    */
}