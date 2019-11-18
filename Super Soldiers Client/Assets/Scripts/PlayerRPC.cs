using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SSRPCs;

[RPCClass]
public class PlayerRPC : MonoBehaviour
{
    [HideInInspector] private PlayerController myPlayerController;
    [HideInInspector] private Inventory myInventory;
    [HideInInspector] private StatSystem myStatSystem;
    private void Start()
    {
        myInventory = GetComponent<Inventory>();
        myPlayerController = GetComponent<PlayerController>();
        myStatSystem = GetComponent<StatSystem>();
        myStatSystem.AddCallback(StatSystem.StatType.Health, UpdateHealth);
        //References.client.gameObject.GetComponent<ClientNetwork>().CallRPC("GetIt", UCNetwork.MessageReceiver.ServerOnly, -1);
    }
    [RPCMethod]
    public void SelectWeapon(int aWeapon)
    {
        myInventory.SetWeapon(aWeapon);
    }
    private void UpdateHealth(StatSystem.StatType aStat, float oldVal, float newVal)
    {
        Debug.Log("PlayerRPC: UpdateHealth" + newVal);
        References.clientNet.CallRPC("UpdatePlayerHealth",
            UCNetwork.MessageReceiver.OtherClients, GetComponent<NetworkSync>().GetId(), newVal);
    }
    [RPCMethod]
    public void UpdatePlayerHealth(float newVal)
    {
        Debug.Log(newVal);
        myStatSystem.SetValue(StatSystem.StatType.Health, newVal);
    }
}
