using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeToLive : MonoBehaviour
{
    [SerializeField] float maxTime = 2.0f;
    float elapsedTime = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > maxTime)
        {
            NetworkSync ns;
            if (TryGetComponent<NetworkSync>(out ns))
            {
                ns.Destroy();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
