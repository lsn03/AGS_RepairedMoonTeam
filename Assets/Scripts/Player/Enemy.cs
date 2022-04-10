using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    
    public void TakeDamage(float damage)
    {
        health -= damage;
       // Debug.Log( "1" );
        if ( health <= 0 )
        {
            Destroy( gameObject );
        }
    }
}
