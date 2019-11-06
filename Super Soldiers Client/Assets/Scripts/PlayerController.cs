using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region References
    [HideInInspector] private Rigidbody2D myRigidbody;
    [HideInInspector] private BoxCollider2D myColldier;
    [HideInInspector] private NetworkSync myNetSync;
    [HideInInspector] private StatSystem myStatSystem;
    [HideInInspector] private Inventory myInventory;
    #endregion

    #region Variables
    [HideInInspector] private float knockback = 0;
    [HideInInspector] private float jumpCD = 0;
    #endregion
    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myColldier = GetComponent<BoxCollider2D>();
        myNetSync = GetComponent<NetworkSync>();
        myStatSystem = GetComponent<StatSystem>();
        myInventory = GetComponent<Inventory>();
        if (myNetSync.owned)
        {
            References.LocalPlayerReferences(this.gameObject);
        }
    }

    void Update()
    {
        if (myNetSync.owned)
        {
            if (knockback < Time.time)
            {
                Movement();
            }
            KeyPress();
            Jetpack();
        }
    }
    private void Jetpack()
    {
        if (Input.GetAxis("Vertical") > 0.5f)
        {
            if (myStatSystem.GetValue(StatSystem.StatType.Fuel) > 0)
            {
                myRigidbody.AddForce(new Vector3(0, 700, 0) * Time.deltaTime);
                if(myRigidbody.velocity.y > 5)
                {
                    Vector3 vel;
                    vel = myRigidbody.velocity;
                    vel.y = 5;
                    myRigidbody.velocity = vel;
                }
                myStatSystem.AddValue(StatSystem.StatType.Fuel, -50 * Time.deltaTime);
            }
            else
            {
                myStatSystem.SetValue(StatSystem.StatType.Fuel, 0);
            }
        }
        else
        {
            RaycastHit2D hit;
            hit = Physics2D.Raycast(transform.position - (new Vector3(0, myColldier.size.y + 0.1f, 0) / 2), -Vector3.up, 0.1f);
            if (null != hit.collider)//upward jump
            {
                myStatSystem.AddValue(StatSystem.StatType.Fuel, 20 * Time.deltaTime);
                if(myStatSystem.GetValue(StatSystem.StatType.Fuel) > myStatSystem.GetMaxValue(StatSystem.StatType.Fuel))
                {
                    myStatSystem.SetValue(StatSystem.StatType.Fuel, myStatSystem.GetMaxValue(StatSystem.StatType.Fuel));
                }
            }
        }
    }
    private void Movement()
    {
        Vector3 vel;
        vel = myRigidbody.velocity;
        vel.x = Input.GetAxis("Horizontal") * myStatSystem.GetValue(StatSystem.StatType.Movespeed) * Time.deltaTime;
        myRigidbody.velocity = vel;
    }
    private void KeyPress()
    {
        if (Input.GetButton("Jump"))
        {
            if (jumpCD < Time.time)
            {
                Jump();
                jumpCD = Time.time + 0.1f;
            }
        }
        if(Input.GetButton("Fire"))
        {
            myInventory.Fire();
        }
    }
    private void Jump()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position - (new Vector3(0, myColldier.size.y + 0.1f, 0) / 2), -Vector3.up, 0.1f);
        if (null != hit.collider)//upward jump
        {
            myRigidbody.AddForce(transform.up * myStatSystem.GetValue(StatSystem.StatType.JumpStrength));
            return;
        }

        hit = Physics2D.Raycast(transform.position - (new Vector3(myColldier.size.x + 0.1f, 0, 0) / 2), -Vector3.right, 0.1f);
        if (null != hit.collider)//wall jump left->right
        {
            myRigidbody.AddForce(transform.up * myStatSystem.GetValue(StatSystem.StatType.JumpStrength));
            myRigidbody.AddForce(transform.right * myStatSystem.GetValue(StatSystem.StatType.JumpStrength));
            knockback = Time.time + 1;
            return;
        }

        hit = Physics2D.Raycast(transform.position + (new Vector3(myColldier.size.x + 0.1f, 0, 0) / 2), Vector3.right, 0.1f);
        if (null != hit.collider)//wall jump right->left
        {
            myRigidbody.AddForce(transform.up * myStatSystem.GetValue(StatSystem.StatType.JumpStrength));
            myRigidbody.AddForce(-transform.right * myStatSystem.GetValue(StatSystem.StatType.JumpStrength));
            knockback = Time.time + 1;
            return;
        }
    }
}
