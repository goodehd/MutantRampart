using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : Data
{
    public EStatusformat Type { get; set; }
    public string Instruction { get; set; }
    public string SpritePath { get; set; }
    public bool isEquiped { get; set; } = false;
}
