using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class SingleShot : Gun
{

    [SerializeField] Camera cam;
    public Transform bulletSpawn;

    public LineRenderer lineRenderer;
    public PhotonView photonView;

    public TextMeshProUGUI text;

    [Range(0,0.5f),SerializeField] float waitForSeconds;
    public override void Use()
    {

    }
    private void Awake()
    {

    }
    void Start()
    {
       


        photonView = GetComponent<PhotonView>();

    }

    private void Update()
    {
        if ( !photonView.IsMine ) return;

        if ( timeBeforeShoots <= 0 )
        {
            //if ( Input.GetKeyDown( KeyCode.R ) && bulletsLeft < magazineSize && !reloading ) Reload();

            if ( Input.GetMouseButtonDown( 0 ) && itemGameObject.active && bulletsLeft > 0 )
            {
                photonView.RPC( "Shoot", RpcTarget.All );
                timeBeforeShoots = timeBetweenShoots;
            }
        }
        else
        {
            timeBeforeShoots -= Time.deltaTime;
        }
        if ( itemGameObject.active )
        {
            text.gameObject.SetActive( true );
            text.SetText( bulletsLeft + " / " + maxBullets );
        }

        else
        {
            text.gameObject.SetActive( false );
        }
    }

    //void Reload()
    //{

    //    reloading = true;
    //    Invoke( "ReloadFinished", reloadTime );
    //}

    //void ReloadFinished()
    //{

    //    if ( currentCountOfBullets < magazineSize )
    //    {
    //        bulletsLeft = currentCountOfBullets;
    //        currentCountOfBullets = 0;
    //    }
    //    else
    //    {
    //        currentCountOfBullets = currentCountOfBullets - magazineSize+bulletsLeft;
    //        bulletsLeft = magazineSize;
    //    }
    //    reloading = false;
    //}

    [PunRPC]
    IEnumerator Shoot()
    {

        //Physics2D.queriesStartInColliders = false;
        RaycastHit2D hitInfo = Physics2D.Raycast( bulletSpawn.position, bulletSpawn.right );
        bulletsLeft--;
        if ( hitInfo )
        {
            Instantiate( hitEffect, hitInfo.point, Quaternion.identity );
            Debug.Log( hitInfo.transform.name );
            if ( photonView.IsMine )
            {
                hitInfo.collider.gameObject.GetComponent<IDamage>()?.TakeDamage( ( ( GunIno )itemInfo ).damage );
            }
            lineRenderer.SetPosition( 0, bulletSpawn.position );
            lineRenderer.SetPosition( 1, hitInfo.point );
        }
        else
        {
            lineRenderer.SetPosition( 0, bulletSpawn.position );
            lineRenderer.SetPosition( 1, bulletSpawn.position + bulletSpawn.right * 50 );
        }

 
        timeBeforeShoots = timeBetweenShoots;
        if ( lineRenderer != null )
            lineRenderer.enabled = true;
        yield return new WaitForSeconds( waitForSeconds );
        if ( lineRenderer != null )
            lineRenderer.enabled = false;

    }


    public void AddBullet( int addBullet )
    {
        SetAddBullet( addBullet );
    }



}
