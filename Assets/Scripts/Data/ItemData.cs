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
}
