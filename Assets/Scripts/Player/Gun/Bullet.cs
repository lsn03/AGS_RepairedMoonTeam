using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public ItemInfo itemInfo;
    public float speed;
    public float destroyTime;

    public GameObject hitEffect;
    private PhotonView photonView;

    public Rigidbody2D _rigidbody2D;


    private void Start()
    {
        Invoke( "DestroyBullet", destroyTime );
        photonView = GetComponent<PhotonView>();
        _rigidbody2D.velocity = transform.right * speed;
        
    }

    void DestroyBullet()
    {
        if ( photonView.IsMine )
        {
            Instantiate( hitEffect, transform.position, Quaternion.identity );
            PhotonNetwork.Destroy( gameObject );
            
        }
    }
    //public void OnTriggerEnter2D( Collider2D collision )
    //{
    //    Debug.Log( "SelfDamage" );
    //    PlayerController  Player = collision.GetComponent<PlayerController>();
    //    BlockToDestroy blackToDestroy = collision.GetComponent<BlockToDestroy>();
    //    if ( Player != null )
    //    {
    //        Debug.Log( "enemy damaged" );
    //        Player.gameObject.GetComponent<IDamage>()?.TakeDamage( ( ( GunIno )itemInfo ).damage );
    //        DestroyBullet();
    //    }

    //}
    private void OnCollisionEnter2D( Collision2D collision )
    {
        {
            PlayerController Player = collision.collider.GetComponent<PlayerController>();
            if ( Player != null  )
            {
                Debug.Log( "enemy damaged" );
                if( photonView.IsMine )
                    Player.gameObject.GetComponent<IDamage>()?.TakeDamage( ( ( GunIno )itemInfo ).damage );
                DestroyBullet();
            }
            DestroyBullet();
        }
    }

}



