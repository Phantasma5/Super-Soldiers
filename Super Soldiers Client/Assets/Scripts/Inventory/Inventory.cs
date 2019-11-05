using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public enum WeaponType
    {
        None,
        Sniper,
        Gernade,
        AR,
        Laser
    }

    #region References
    #endregion
    #region Variables
    [HideInInspector] public WeaponType weapon;
    [HideInInspector] public int ammo;
    [HideInInspector] private int ammoMax;
    [HideInInspector] private float shootCD;
    #endregion
    private void Start()
    {
        if(WeaponType.None == weapon)
        {
            Debug.Log("RandomingWeapon");
            weapon = (WeaponType)Random.Range(1, 4);
        }
    }
    public void Fire()
    {
        if(0 >= ammo)
        {
            Reload();
            return;
        }
        ammo -= 1;
        //TODO: Spawn networked bullet and give it velocity.
        //TODO: CD based on weapon
    }
    public void Reload()
    {
        shootCD = Time.time + 2;
        ammo = ammoMax;
    }
}
