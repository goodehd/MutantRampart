using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShamanFx2 : MonoBehaviour
{
    public void EndAnimation()
    {
        Main.Get<ResourceManager>().Destroy(gameObject);
    }
}
