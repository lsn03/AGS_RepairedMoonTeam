using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGrenade : MonoBehaviour
{
    public ItemInfo itemInfo;
    public float speed;
    public float fallSpeed;
    public float destroyTime;

    public GameObject hitEffect;
   

    public Rigidbody2D _rigidbody2D;

    [Range(0.1f, 5f), SerializeField] float splashRange;
    public void SetSender( string name )
    {
        this.name = name;
    }
    private string name;

    private void Start()
    {
        Invoke("DestroyBullet", destroyTime);
        
        _rigidbody2D.velocity = transform.right * speed;
    }

    private void Update()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y - fallSpeed * Time.deltaTime);
    }

    void DestroyBullet()
    {
        Destroy(gameObject);        
    }
    private void OnCollisionEnter2D( Collision2D collision )
    {
        var obj = collision.collider.gameObject.GetComponent<IDamage>();

        if ( obj != null )
        {
            Debug.Log( "enemy damaged" );

            hitEffectController hit =  Instantiate(hitEffect, transform.position, Quaternion.identity).GetComponent<hitEffectController>();
            hit.ShowDamage( ( ( GunInfo )itemInfo ).damage );
            obj.TakeDamage( ( ( GunInfo )itemInfo ).damage, name, nameof( GrenadeLauncher ) );
        }

        DestroyBullet();
    }
    
    // ����������
    // �������
    // ���������

}



