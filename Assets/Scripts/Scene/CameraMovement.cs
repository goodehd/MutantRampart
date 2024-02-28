using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class CameraMovement : MonoBehaviour
{
    private Vector3 Origin;
    private Vector3 Diference;
    private bool Drag = false;
    public bool Rock { get; set; } = false;

    void LateUpdate()
    {
        /*
        if (Rock) //다른 UI 가 있을때 무브가 작동하지않게끔
        {
            return;
        }
        */
        if (Input.GetMouseButton(1))
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
    public void OnMove(InputAction.CallbackContext value)
    {
        Vector2 input = value.ReadValue<Vector2>();
        Camera.main.transform.position += new Vector3(input.x, input.y, 0f);
    }
    public void OnScroll(InputAction.CallbackContext value)
    {
        float input = value.ReadValue<float>();
        if (input > 0f)
        {
            Camera.main.DOOrthoSize(2.5f, 0.3f); //줌인기능
        }
        else if(input < 0f)
        {
            Camera.main.DOOrthoSize(5f, 0.3f); //줌인기능
        }
    }
}