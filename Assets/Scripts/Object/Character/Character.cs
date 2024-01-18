using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterStatus _status;

    public void SetStatus(CharacterData data)
    {
        _status = new CharacterStatus(data.Hp, data.HpMax, data.Damage, data.Defense);
    }
}
