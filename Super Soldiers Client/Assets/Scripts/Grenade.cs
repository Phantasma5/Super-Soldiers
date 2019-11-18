using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 500.0f;
    public float detonationForce = 1000.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (GetComponent<NetworkSync>().owned)
        {
            GetComponent<NetworkSync>().AddToArea(1);
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

    private void OnDestroy()
    {
        foreach(var target in Physics2D.OverlapCircleAll(transform.position, 2.0f))
        {
            NetworkSync ns;
            if(target.TryGetComponent<NetworkSync>(out ns))
            {
                if(ns.owned)
                {
                    Vector2 temp = target.transform.position - transform.position;                    
                    target.attachedRigidbody.AddForceAtPosition(temp * detonationForce, transform.position);
                    Debug.Log("Grenade -> player");
                    target.gameObject.GetComponent<StatSystem>().AddValue(StatSystem.StatType.Health,
                        -References.localStatSystem.GetValue(StatSystem.StatType.Damage));
                }
            }            
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log("Grenade -> player");
    //    collision.gameObject.GetComponent<StatSystem>().AddValue(StatSystem.StatType.Health,
    //        -References.localStatSystem.GetValue(StatSystem.StatType.Damage));
    //}
}
