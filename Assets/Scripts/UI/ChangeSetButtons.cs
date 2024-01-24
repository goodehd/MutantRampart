using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeSetButtons : BaseUI
{
    private Button _changeRoomSetButton;
    private Button _changeUnitSetButton;

    public bool isRoomSet = false;
    public bool isUnitSet = false;
    
    public ChangeUnitUI changeUnitUI;
    
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
        if(isUnitSet)return;
        
        isRoomSet = true;
    }
    private void ChangeUnitSet(PointerEventData eventData)
    {
        if(isRoomSet) return;
        
        
        if (isUnitSet)
        {
            isUnitSet = false;
            Main.Get<UIManager>().ClosePopup();
        }
        else
        {
            changeUnitUI = Main.Get<UIManager>().OpenPopup<ChangeUnitUI>("ChangeUnit_PopUpUI");
            isUnitSet = true;
        }
        
    }
    
}
