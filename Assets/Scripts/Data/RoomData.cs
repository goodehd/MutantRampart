using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : Data, INextKey
{
    public EStatusformat Type { get; set; }
    public string Instruction { get; set; }
    public int MaxUnitCount { get; set; }

    public int Price { get; set; }
    
    public float Duration { get; set; }
    public float UpgradeValue_1 { get; set; }
    public float UpgradeValue_2 { get; set; }
    public float UpgradeValue_3 { get; set; }

    public string Nextkey { get; set; }

}
