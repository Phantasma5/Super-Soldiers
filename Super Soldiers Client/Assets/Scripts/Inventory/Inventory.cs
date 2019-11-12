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
                References.localStatSystem.SetValue(StatSystem.StatType.Damage, 50);
                break;
            case 2:
                ammoMax = 1;
                References.localStatSystem.SetValue(StatSystem.StatType.Damage, 100);
                break;
            case 3:
                ammoMax = 15;
                References.localStatSystem.SetValue(StatSystem.StatType.Damage, 10);
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
        Vector3 target;
        target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target = new Vector3(target.x, target.y, 0);
        //tempTrans.LookAt(target);
        //tempTrans.position = new Vector3(tempTrans.position.x, tempTrans.position.y, 0);
        var temp = ExampleClient.GetInstance().clientNet.Instantiate("BulletAR", tempTrans.position, Quaternion.identity);
        //float smthn = Vector2.SignedAngle()
        //temp.transform.root.eulerAngles = new Vector3(0, 0, smthn);
        Vector2 delta = target - transform.position;
        temp.GetComponent<Rigidbody2D>().AddForce(delta.normalized * temp.GetComponent<BulletMovement>().speed);
        Destroy(tempObj);
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
