using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeUnitUI : BaseUI
{
    private Transform _content;
    public BatRoom SelectRoom;

    public CharacterData SelectUnitData;
    
    protected override void Init()
    {
        SetUI<Transform>();
        _content = GetUI<Transform>("Content");
        
        
        SetUnitInventory();
    }
    
    private void SetUnitInventory()
    {
        List<CharacterData> playerUnits = Main.Get<GameManager>().playerUnits;
        foreach (Transform item in _content.transform)
        {
            Destroy(item.gameObject);
        }

        for (int i = 0; i < playerUnits.Count; i++)
        {
            //if (playerUnits[i].isEquiped) //이부분은 batroom을 체크해야할 듯
            {
                //continue;
            }
            UnitSelectImage unitSelectImage = Main.Get<UIManager>().CreateSubitem<UnitSelectImage>("UnitSelectImage", _content);
            unitSelectImage.CharacterData = playerUnits[i];
            unitSelectImage.Owner = this;
           
        }
    }
}
