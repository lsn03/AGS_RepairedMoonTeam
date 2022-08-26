using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telep : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pointTeleport;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position = pointTeleport.gameObject.transform.position;
        }
           
    }
}
