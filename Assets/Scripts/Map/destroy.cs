using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy : MonoBehaviour
{

    public float StartTime;
    public float EndTime;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        StartTime += 0.1f * Time.deltaTime;
        if (StartTime >= EndTime)
        {
            Destroy(gameObject);
        }

    }
}
