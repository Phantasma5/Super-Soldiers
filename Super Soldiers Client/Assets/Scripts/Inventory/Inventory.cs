﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public enum WeaponType
    {
        None,
        Sniper,
        Gernade,
        AR
    }

    #region References
    public GameObject turret;
    #endregion
    #region Variables
    [HideInInspector] public WeaponType weapon;
    [HideInInspector] public int ammo;
    [HideInInspector] private int ammoMax;
    [HideInInspector] public float reloadCD = 0;
    #endregion

    public void SetWeapon(int aWeapon)
    {
        weapon = (WeaponType)aWeapon;
        Camera.main.GetComponent<CameraController>().UpdateCameraZoom();
        switch (aWeapon)
        {
            case 1:
                ammoMax = 1;//sniper
                References.localStatSystem.SetValue(StatSystem.StatType.Damage, 50);
                break;
            case 2:
                ammoMax = 1;//gernade
                References.localStatSystem.SetValue(StatSystem.StatType.Damage, 100);
                break;
            case 3:
                ammoMax = 15;//AR
                References.localStatSystem.SetValue(StatSystem.StatType.Damage, 10);
                break;
        }
        ammo = ammoMax;
    }
    public void Fire()
    {
        if (0 >= ammo)
        {
            Reload();
            return;
        }
        ammo -= 1;
        UpdateAmmo();
        //GameObject tempObj = new GameObject();
        //Transform tempTrans = tempObj.transform;
        //tempTrans.position = transform.position;
        Vector3 target;
        target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target = new Vector3(target.x, target.y, 0);
        var temp = ExampleClient.GetInstance().clientNet.Instantiate(BulletType(), turret.transform.position, Quaternion.identity);
        Vector2 delta = target - transform.position;
        try
        {
            temp.GetComponent<Rigidbody2D>().AddForce(delta.normalized * temp.GetComponent<BulletMovement>().speed);
        }
        catch
        {
            temp.GetComponent<Rigidbody2D>().AddForce(delta.normalized * temp.GetComponent<Grenade>().speed);
        }
        //ExampleClient.GetInstance().clientNet.ad
        //Destroy(tempObj);
    }
    private void UpdateAmmo()
    {
        if (this.gameObject == References.localPlayer)
        {
            References.userInterface.UpdateAmmo(ammo, ammoMax);
        }
    }
    public void Reload()
    {

        if (reloadCD < Time.time)
        {
            StartCoroutine(ReloadCourotine());
            reloadCD = Time.time + 2;
        }
    }
    IEnumerator ReloadCourotine()
    {
        yield return new WaitForSeconds(2);
        ammo = ammoMax;
        UpdateAmmo();
    }
    private string BulletType()
    {
        switch (weapon)
        {
            case WeaponType.Sniper:
                return "BulletAR";
            case WeaponType.Gernade:
                return "Grenade";
            case WeaponType.AR:
                return "BulletAR";
            default:
                return "BulletAR";
        }
    }
}
