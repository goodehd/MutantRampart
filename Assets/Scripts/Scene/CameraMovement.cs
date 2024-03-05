using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    private UIManager uI;
    private Vector3 Origin;
    private Vector3 Diference;
    private bool isOver = false;
    private bool Drag = false;
    public bool Rock { get; set; } = false;
    private Unit _onMouesUnit;
    private Vector3 targetPosition = Vector3.zero;

    private void Start()
    {
        uI = Main.Get<UIManager>();
    }

    void LateUpdate()
    {
        /*
        if (Rock) //다른 UI 가 있을때 무브가 작동하지않게끔
        {
            return;
        }
        */

        if (!IsPointerOverUI())
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 5f, LayerMask.GetMask("Unit"));

            if (hit)
            {
                if (!isOver)
                {
                    _onMouesUnit = hit.collider.GetComponent<Unit>();
                    _onMouesUnit.DrawOutline();
                    isOver = true;
                }

                if (isOver && _onMouesUnit != hit.collider.GetComponent<Unit>())
                {
                    _onMouesUnit.UndrawOutline();
                    _onMouesUnit = hit.collider.GetComponent<Unit>();
                    _onMouesUnit.DrawOutline();
                }

                if (Input.GetMouseButtonDown(0))
                {
                    ((DayMain_SceneUI)uI.SceneUI).CreateClickUnitUI(_onMouesUnit.CharacterInfo);
                }
            }
            else
            {
                if (isOver)
                {
                    _onMouesUnit.UndrawOutline();
                    _onMouesUnit = null;
                    isOver = false;
                }
            }
        }

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

    private bool IsPointerOverUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.layer == LayerMask.NameToLayer("UI"))
                return true;
        }

        return false;
    }
}