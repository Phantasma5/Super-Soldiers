using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponentInParent<NetworkSync>().owned)
        {

        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.mousePosition.x - Screen.width / 2;
        float y = Input.mousePosition.y - Screen.height / 2;
        transform.localPosition = new Vector3(
            x, y, 0).normalized * 0.6f;
    }
}
