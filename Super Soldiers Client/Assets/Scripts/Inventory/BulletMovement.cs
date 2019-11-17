using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    #region References
    [HideInInspector] private Rigidbody2D myRigidbody;
    #endregion
    #region Variables
    public float speed;
    [SerializeField] float ttl = 2.0f;
    #endregion
    void Start()
    {
        if (!GetComponent<NetworkSync>().owned)
        {
            Destroy(GetComponent<Rigidbody2D>());
            return;
        }
        GetComponent<NetworkSync>().AddToArea(1);
        //gameObject.AddComponent<Rigidbody2D>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myRigidbody.AddForce(transform.forward * speed);
;    }

    void Update()
    {
        if (!GetComponent<NetworkSync>().owned)
        {
            return;
        }
        ttl -= Time.deltaTime;
        //myRigidbody.velocity = transform.forward * speed;
        if(ttl <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        if(!GetComponent<NetworkSync>().owned)
        {
            return;
        }
        Vector3 pos;
        pos = transform.position;
        pos.z = 0;
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)    
    {
        //Debug.Log(collision.transform.tag);
        if("Wall" == collision.transform.tag)
        {
            Destroy(this.gameObject);
        }
        else if(collision.transform.tag == "Player" && GetComponent<NetworkSync>().owned)
        {
            Debug.Log("Bullet -> player");
            collision.gameObject.GetComponent<StatSystem>().AddValue(StatSystem.StatType.Health,
                -References.localStatSystem.GetValue(StatSystem.StatType.Damage));
            Destroy(this.gameObject);
        }
    }
}
