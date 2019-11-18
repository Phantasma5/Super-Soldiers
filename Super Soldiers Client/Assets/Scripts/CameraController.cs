using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float CameraZoom = 5;
    private void Update()
    {
        if (null == References.localPlayer)
        {
            return;
        }
        transform.position = new Vector3(References.localPlayer.transform.position.x, References.localPlayer.transform.position.y, -10);
    }// end update
    public void UpdateCameraZoom()
    {
        if (References.localInventory.weapon == Inventory.WeaponType.Sniper)
        {
            Camera.main.orthographicSize = CameraZoom * 2;
        }
        else
        {
            Camera.main.orthographicSize = CameraZoom;
        }
    }
}// end class
