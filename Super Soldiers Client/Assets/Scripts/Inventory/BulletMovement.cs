using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    #region References
    [HideInInspector] private Rigidbody myRigidbody;
    #endregion
    #region Variables
    [SerializeField] private float speed;
    [SerializeField] float ttl = 2.0f;
    #endregion
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ttl -= Time.deltaTime;
        myRigidbody.velocity = transform.forward * speed;
        if(ttl <= 0)
        {
            Destroy(gameObject);
        }
    }
}
