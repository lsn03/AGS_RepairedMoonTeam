using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float destroyTime;
    public float damage = 25f;

    private PhotonView photonView;

    private void Start()
    {
        Invoke( "DestroyBullet", destroyTime );
        photonView = GetComponent<PhotonView>();
    }
    void Update()
    {
        
        transform.Translate( new Vector2(1,0) * speed * Time.deltaTime );
    }

    void DestroyBullet()
    {
        PhotonNetwork.Destroy( gameObject );
    }
    public void OnTriggerEnter2D( Collider2D collision )
    {
  
        PlayerController  Player = collision.GetComponent<PlayerController>();
        //BlockToDestroy blackToDestroy = collision.GetComponent<BlockToDestroy>();
        if ( Player != null && !photonView.IsMine )
        {
            Player.TakeDamage( damage );
            
        }
        DestroyBullet();
    }

}
