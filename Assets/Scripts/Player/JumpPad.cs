using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public Vector2 jumpPadForce;
    public bool isPreservePlayerSpeed;
    private void OnCollisionEnter2D( Collision2D collision )
    {
        if(collision.gameObject.CompareTag("Player") )
        {
            Vector2 _finalForce = jumpPadForce;
            if (isPreservePlayerSpeed)
                _finalForce.x = collision.gameObject.GetComponent<Rigidbody2D>().velocity.x;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = _finalForce;
        }
    }
}
