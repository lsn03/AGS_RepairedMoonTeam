using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SingleShot : Gun
{

    [SerializeField] Camera cam;
    public Transform bulletSpawn;

    public LineRenderer lineRenderer;
    public PhotonView photonView;
    public override void Use()
    {
         photonView.RPC( "Shoot", RpcTarget.All );
    }

    void Start()
    {

        photonView = GetComponent<PhotonView>();

    }
    

    [PunRPC]
    IEnumerator Shoot()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast( bulletSpawn.position, bulletSpawn.right );
        
        if ( hitInfo )
        {
            Debug.Log( hitInfo.transform.name );
            hitInfo.collider.gameObject.GetComponent<IDamage>()?.TakeDamage(((GunIno)itemInfo).damage);
            lineRenderer.SetPosition( 0, bulletSpawn.position );
            lineRenderer.SetPosition( 1, hitInfo.point );
        }
        else
        {
            lineRenderer.SetPosition( 0, bulletSpawn.position );
            lineRenderer.SetPosition( 1, bulletSpawn.position + bulletSpawn.right * 50 ) ;
        }
        if ( lineRenderer != null )
            lineRenderer.enabled = true;
        yield return new WaitForSeconds( 0.02f );
        if ( lineRenderer != null )
            lineRenderer.enabled = false;
    }
}
