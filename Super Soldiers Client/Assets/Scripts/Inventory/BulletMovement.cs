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
    #endregion
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        myRigidbody.velocity = transform.forward * speed;
    }
}
