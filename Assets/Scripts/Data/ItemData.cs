using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : Data
{
    public EItemType Type { get; set; }
    
    public float HpAdd { get; set; }

    public float DefenseAdd{ get; set; }

    public float AttackAdd{ get; set; }

    public float SpeedAdd{ get; set; }
    
    public string Instruction { get; set; }

    public int Price { get; set; }
    public float UpgradeValue_1 { get; set; }
    public float UpgradeValue_2 { get; set; }
    public float UpgradeValue_3 { get; set; }
}
