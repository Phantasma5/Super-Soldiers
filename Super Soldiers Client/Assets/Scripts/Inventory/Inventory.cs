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
            RandomWeapon();
        }
    }
    private void RandomWeapon()
    {
        Debug.Log("RandomingWeapon");
        SetWeapon(Random.Range(1, 4));
    }
    public void SetWeapon(int aWeapon)
    {
        weapon = (WeaponType)aWeapon;
        switch(aWeapon)
        {
            case 1:
                ammoMax = 1;
                break;
            case 2:
                ammoMax = 1;
                break;
            case 3:
                ammoMax = 15;
                break;
            case 4:
                ammoMax = 999999;
                break;
        }
        ammo = ammoMax;
    }
    public void Fire()
    {
        Debug.Log("Fire");
        if(0 >= ammo)
        {
            Debug.Log("out of ammo");
            Reload();
            return;
        }
        ammo -= 1;
        Debug.Log("Shooting");
        ClientNetwork.Instantiate(Resources.Load("BulletAR"), transform);
        //TODO: CD based on weapon
    }
    public void Reload()
    {
        shootCD = Time.time + 2;
        ammo = ammoMax;
    }
}
