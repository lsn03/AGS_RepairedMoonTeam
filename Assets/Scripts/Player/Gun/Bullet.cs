using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public ItemInfo itemInfo;
    public float speed;
    public float destroyTime;
    public float damage = 25f;

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
        PhotonNetwork.Destroy( gameObject );
    }
    public void OnTriggerEnter2D( Collider2D collision )
    {
        Debug.Log( "SelfDamage" );
        PlayerController  Player = collision.GetComponent<PlayerController>();
        //BlockToDestroy blackToDestroy = collision.GetComponent<BlockToDestroy>();
        if ( Player != null )
        {
            Debug.Log( "enemy damaged" );
            Player.gameObject.GetComponent<IDamage>()?.TakeDamage( ( ( GunIno )itemInfo ).damage );
            DestroyBullet();
        }
        
    }

}
