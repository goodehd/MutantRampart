using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeSetButtons : BaseUI
{
    
    public Room SelectRoom;
    
    private Button _changeRoomSetButton;
    private Button _changeUnitSetButton;

    public bool isRoomSet = false;
    public bool isUnitSet = false;
    
    
    protected override void Init()
    {
        SetUI<Button>();

        _changeRoomSetButton = GetUI<Button>("ChangeRoomSetButton");
        _changeUnitSetButton = GetUI<Button>("ChangeUnitSetButton");
        
        SetUICallback(_changeRoomSetButton.gameObject, EUIEventState.Click, ChangeRoomSet);
        SetUICallback(_changeUnitSetButton.gameObject, EUIEventState.Click, ChangeUnitSet);
        
    }

    private void ChangeRoomSet(PointerEventData eventData)
    {
        isRoomSet = true;
        isUnitSet = false;
        Debug.Log(isRoomSet);
        Debug.Log(isUnitSet);
    }
    private void ChangeUnitSet(PointerEventData eventData)
    {
        isRoomSet = false;
        isUnitSet = true;
        ChangeUnitUI changeUnitUI = Main.Get<UIManager>().OpenPopup<ChangeUnitUI>("ChangeUnit_PopUpUI");
        Debug.Log(isRoomSet);
        Debug.Log(isUnitSet);
    }
    
    
}
