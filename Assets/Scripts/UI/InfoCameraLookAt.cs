using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoCameraLookAt : MonoBehaviour
{
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectRoominfoCameraLookAt(Transform tr)
    {
        this.gameObject.transform.position = new Vector3(tr.position.x, tr.position.y + 1.75f, -10);
    }
}
