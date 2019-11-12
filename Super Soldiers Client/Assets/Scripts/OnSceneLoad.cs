using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSceneLoad : MonoBehaviour
{
    GameObject myPlayer;
    private void Start()
    {
        myPlayer = References.clientNet.Instantiate("Player", new Vector3(Random.Range(-2, 2), 3, 0), Quaternion.identity);
        myPlayer.GetComponent<NetworkSync>().AddToArea(1);
        References.FindReferences();
        References.LocalPlayerReferences(myPlayer);
        References.client.GimmeAGun();
    }
}
