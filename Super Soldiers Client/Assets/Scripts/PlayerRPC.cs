using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SSRPCs;

[RPCClass]
public class PlayerRPC : MonoBehaviour
{
    [HideInInspector] private PlayerController myPlayerController;
    [HideInInspector] private Inventory myInventory;
    private void Start()
    {
        myInventory = GetComponent<Inventory>();
        myPlayerController = GetComponent<PlayerController>();
        //References.client.gameObject.GetComponent<ClientNetwork>().CallRPC("GetIt", UCNetwork.MessageReceiver.ServerOnly, -1);
    }
    [RPCMethod]
    public void SelectWeapon(int aWeapon)
    {
        myInventory.weapon = (Inventory.WeaponType)aWeapon;
    }
}
