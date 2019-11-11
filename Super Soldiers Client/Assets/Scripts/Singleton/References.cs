using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class References : MonoBehaviour
{
    public static References instance;
    public static GameObject localPlayer;
    public static ExampleClient client;
    public static StatSystem localStatSystem;
    public static UserInterface userInterface;

    private void Awake()
    {
        if(null != instance)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        FindReferences();
    }
    private void FindReferences()
    {
        try
        {
            client = GetComponent<ExampleClient>();
            userInterface = GameObject.FindWithTag("UserInterface").GetComponent<UserInterface>();
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    public static void LocalPlayerReferences(GameObject aPlayer)
    {
        localPlayer = aPlayer;
        localStatSystem = References.localPlayer.GetComponent<StatSystem>();
    }

    private void Update()
    {
        if (client == null || userInterface == null)
        {
            try
            {
                FindReferences();
            }
            catch(Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        if(localPlayer != null && localStatSystem == null)
        {
            LocalPlayerReferences(localPlayer);
        }
    }
}
