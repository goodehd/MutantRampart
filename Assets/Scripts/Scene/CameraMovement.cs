using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 Origin;
    private Vector3 Diference;
    private bool Drag = false;
    public bool Rock { get; set; } = false;
    
    void LateUpdate()
    {
        if (Rock)
        {
            return;
        }
        
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
}