using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    #region References
    [HideInInspector] private Rigidbody2D myRigidbody;
    #endregion
    #region Variables
    [SerializeField] private float speed;
    [SerializeField] float ttl = 2.0f;
    #endregion
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
;    }

    void Update()
    {
        ttl -= Time.deltaTime;
        myRigidbody.velocity = transform.forward * speed;
        if(ttl <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        Vector3 pos;
        pos = transform.position;
        pos.z = 0;
        transform.position = pos;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.transform.tag);
        if("Wall" == collision.transform.tag)
        {
            Destroy(this.gameObject);
        }
    }
}
