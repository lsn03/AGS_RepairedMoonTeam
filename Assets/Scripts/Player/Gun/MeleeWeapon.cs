using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Gun
{
    public float distance = 2f;
    public Transform bulletSpawn;
    PhotonView photonView;

    [SerializeField] AudioSource shootingSound;
    [SerializeField] AudioSource idleSound;
    public override void Use()
    {
       // Shoot();
    }
    string[] name;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
         name = photonView.Owner.NickName.Split('\t');
    }

    private void Update()
    {
        if ( !photonView.IsMine ) return;
        if ( !itemGameObject.active ) { shootingSound.Stop(); idleSound.Stop(); }
        if ( timeBeforeShoots <= 0 )
        {
            if ( itemGameObject.active )
            {
                if ( !idleSound.isPlaying )
                {
                    idleSound.Play();
                }

                if ( Input.GetMouseButton( 0 ))
                {
                    if( !shootingSound.isPlaying )
                    {
                        shootingSound.Play();
                        idleSound.Stop();
                    }

                    timeBeforeShoots = timeBetweenShoots;
                    photonView.RPC( "Shoot", RpcTarget.All );
                }
            }
        }
        else
        {
            timeBeforeShoots -= Time.deltaTime;
        }
    }
    
    [PunRPC]
    void Shoot()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast( bulletSpawn.position, bulletSpawn.right*transform.localScale.x,distance );

        if ( hitInfo )
        {
                hitInfo.collider.gameObject.GetComponent<IDamage>()?.TakeDamage( ( ( GunIno )itemInfo ).damage, name[0] );   
        }
    }

    private void OnDrawGizmos()
    {
        if( itemGameObject.active )
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine( bulletSpawn.position, bulletSpawn.position + bulletSpawn.right * transform.localScale.x * distance );
        }
    }
}
