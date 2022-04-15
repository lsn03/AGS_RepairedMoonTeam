using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticShot : Gun
{
    public Transform bulletSpawn;

    public LineRenderer lineRenderer;
    public PhotonView photonView;
    public float startTime;
    private float timeShot;
    public override void Use()
    {
       
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
            hitInfo.collider.gameObject.GetComponent<IDamage>()?.TakeDamage( ( ( GunIno )itemInfo ).damage );
            lineRenderer.SetPosition( 0, bulletSpawn.position );
            lineRenderer.SetPosition( 1, hitInfo.point );
        }
        else
        {
            lineRenderer.SetPosition( 0, bulletSpawn.position );
            lineRenderer.SetPosition( 1, bulletSpawn.position + bulletSpawn.right * 50 );
        }
        lineRenderer.enabled = true;
        yield return new WaitForSeconds( 0.02f );
        lineRenderer.enabled = false;
        
    }
}
