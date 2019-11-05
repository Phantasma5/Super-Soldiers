using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private void Awake()
    {
        if(null != instance)
        {
            Destroy(this.gameObject);
        }
        instance = this;
    }
}
