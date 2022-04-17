using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Gun
{
    public float distance = 2f;
    public Transform bulletSpawn;
    PhotonView photonView;
    public override void Use()
    {
        Shoot();
    }
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    [PunRPC]
    void Shoot()
    {
        
        RaycastHit2D hitInfo = Physics2D.Raycast( bulletSpawn.position, bulletSpawn.right*transform.localScale.x,distance );
        if ( hitInfo )
        {
            Debug.Log( hitInfo.transform.name );
            if( photonView.IsMine )
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
