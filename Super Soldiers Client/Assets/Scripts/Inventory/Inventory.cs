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
        AR
    }

    #region References
    #endregion
    #region Variables
    [HideInInspector] public WeaponType weapon;
    [HideInInspector] public int ammo;
    [HideInInspector] private int ammoMax;
    [HideInInspector] private float shootCD;
    #endregion

    public void SetWeapon(int aWeapon)
    {
        Debug.Log("Weapon" + aWeapon);
        weapon = (WeaponType)aWeapon;
        switch (aWeapon)
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
        }
        ammo = ammoMax;
    }
    public void Fire()
    {
        if (shootCD > Time.time)
        {
            return;
        }
        if (0 >= ammo)
        {
            Reload();
            return;
        }
        ammo -= 1;
        UpdateAmmo();
        GameObject tempObj = new GameObject();
        Transform tempTrans = tempObj.transform;
        tempTrans.position = transform.position;
        tempTrans.LookAt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        var temp = ExampleClient.GetInstance().clientNet.Instantiate("BulletAR", tempTrans.position, tempTrans.rotation);
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
        shootCD = Time.time + 2;
        ammo = ammoMax;
        UpdateAmmo();
    }
}
