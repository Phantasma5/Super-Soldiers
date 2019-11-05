﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class References : MonoBehaviour
{
    public static References instance;
    public static GameObject player;
    public static ExampleClient client;

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
        client = GetComponent<ExampleClient>();
    }
    
}
