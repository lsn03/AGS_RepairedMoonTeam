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
    PlayerController player;
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        player = GetComponent<PlayerController>();
       
    }
    
    private void Update()
    {
        if ( timeShot <= 0 )
        {
            if ( Input.GetMouseButton( 0 ) && itemGameObject.active )
            {
                timeShot = startTime;
                photonView.RPC( "ShootAuto", RpcTarget.All );
            }
        }
        else
        {
            timeShot -= Time.deltaTime;
        }
    }

    [PunRPC]
    IEnumerator ShootAuto()
    {

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
            if ( lineRenderer != null )
                lineRenderer.enabled = true;
            yield return new WaitForSeconds( 0.01f );
            if ( lineRenderer != null )
                lineRenderer.enabled = false;

        }
    }
}
