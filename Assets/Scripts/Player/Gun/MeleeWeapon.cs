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
    private float timeShot;
    public override void Use()
    {
        Shoot();
    }
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if ( !photonView.IsMine ) return;
        if ( timeShot <= 0 )
        {
            if ( Input.GetMouseButton( 0 ) && itemGameObject.active )
            {
                timeShot = startTime;
               
                photonView.RPC( "Shoot", RpcTarget.All );
            }
        }
        else
        {
            timeShot -= Time.deltaTime;
        }
    }

    [PunRPC]
    void Shoot()
    {
        
        RaycastHit2D hitInfo = Physics2D.Raycast( bulletSpawn.position, bulletSpawn.right*transform.localScale.x,distance );
        SoundManager.PlaySound( "Chainsaw_attack" );
        if ( hitInfo )
        {
                hitInfo.collider.gameObject.GetComponent<IDamage>()?.TakeDamage( ( ( GunIno )itemInfo ).damage );   
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
