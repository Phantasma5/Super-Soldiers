using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void Update()
    {
        transform.position = References.localPlayer.transform.position;
    }
}
