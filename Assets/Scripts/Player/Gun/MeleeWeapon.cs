using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Gun
{
    public float distance = 2f;
    public Transform bulletSpawn;
    PhotonView photonView;

    public float startTime;
    private float lostTime;

    [SerializeField] AudioSource shootingSound;
    public override void Use()
    {
        Shoot();
    }
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }
    bool ready;

    private void Update()
    {
        if ( !photonView.IsMine ) return;
        if ( !itemGameObject.active ) { shootingSound.Stop(); }
        if ( timeBeforeShoots <= 0 )
        {
            if ( itemGameObject.active )
            {
                if ( Input.GetMouseButton( 0 ))
                {
                    
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
                hitInfo.collider.gameObject.GetComponent<IDamage>()?.TakeDamage( ( ( GunIno )itemInfo ).damage );   
        }
    }
    void PlaySound()
    {
        shootingSound.Play();
    }
    void StopPlaySound()
    {
        shootingSound.Stop();
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
