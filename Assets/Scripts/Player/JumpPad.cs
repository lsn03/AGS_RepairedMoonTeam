using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public Vector2 jumpPadForce;
    private void OnCollisionEnter2D( Collision2D collision )
    {
        if(collision.gameObject.CompareTag("Player") )
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = jumpPadForce;
        }
    }
}
