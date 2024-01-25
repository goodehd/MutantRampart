using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : Data
{
    public EStatusformat Type { get; set; }
    public string Instruction { get; set; }
    public int MaxUnitCount { get; set; }

    public int Price { get; set; }
    public bool isEquiped { get; set; } = false;
    public int indexX { get; set; }
    public int indexY { get; set; }
}
