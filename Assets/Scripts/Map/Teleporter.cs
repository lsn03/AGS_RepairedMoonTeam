using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameObject pointTeleport;
    public float TeleportTime;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<PlayerController>().teleportTimer < 0)
            {
                collision.gameObject.transform.position = pointTeleport.gameObject.transform.position;
                collision.gameObject.GetComponent<PlayerController>().teleportTimer = TeleportTime;
            }
        }           
    }
}
