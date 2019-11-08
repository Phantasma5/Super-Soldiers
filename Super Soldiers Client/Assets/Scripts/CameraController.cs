using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void Update()
    {
        if(null == References.localPlayer)
        {
            return;
        }
        transform.position = new Vector3(References.localPlayer.transform.position.x, References.localPlayer.transform.position.y, -10);
    }
}
