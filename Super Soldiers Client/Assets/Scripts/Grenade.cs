using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 500.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (GetComponent<NetworkSync>().owned)
        {
            rb.isKinematic = false;
            this.enabled = true;
            //rb.AddRelativeForce(Vector2.right * initialForce);
        }
        else
        {
            rb.isKinematic = true;
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
