using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonGameObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {        
        if(GameObject.Find(gameObject.name) == gameObject)
        {
            //do nothing
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
