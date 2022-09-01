using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlasma : MonoBehaviour
{
    public ItemInfo itemInfo;
    public float speed;
    public float destroyTime;

    public GameObject hitEffect;
   // private PhotonView photonView;

    public Rigidbody2D _rigidbody2D;

   


    private void Start()
    {
        Invoke("DestroyBullet", destroyTime);
       // photonView = GetComponent<PhotonView>();
        _rigidbody2D.velocity = transform.right * speed;
    }

    void DestroyBullet()
    {
        //if ( photonView.IsMine )
        {
            
            Destroy( gameObject );

        }
    }
    public void SetSender( string name)
    {
        this.name = name;
    }
    private string name;
    private void OnCollisionEnter2D(Collision2D collision)
    {

        var obj = collision.collider.gameObject.GetComponent<IDamage>();
        if ( obj != null )
        {
            hitEffectController hit =  Instantiate(hitEffect, transform.position, Quaternion.identity).GetComponent<hitEffectController>();
            hit.ShowDamage( ( ( GunInfo )itemInfo ).damage );
            obj.TakeDamage( ( ( GunInfo )itemInfo ).damage, name, nameof( PlasmaRifle ) );
        }
        DestroyBullet();
        
    }
    // лидерборды
    // патроны
    // ракетница

}



