using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float destroyTime;
    public float damage = 25f;

    
    private void Start()
    {
        Invoke( "DestroyBullet", destroyTime );
    }
    void Update()
    {
       
        transform.Translate( new Vector2(1,0) * speed * Time.deltaTime );
    }

    void DestroyBullet()
    {
        Destroy( gameObject );
    }
    public void OnTriggerEnter2D( Collider2D collision )
    {
  
        Enemy enemy = collision.GetComponent<Enemy>();
        if ( enemy != null )
        {
            enemy.TakeDamage( damage );
            
        }
        DestroyBullet();

    }
}
