using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AutomaticShot : Gun
{
    public Transform bulletSpawn;

    public LineRenderer lineRenderer;
    public PhotonView photonView;

    public TextMeshProUGUI text;

     AudioSource shootingSound;

    public override void Use()
    {
       
    }
    
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        shootingSound = GetComponent<AudioSource>();


    }
    bool readyToSound = false;
   [SerializeField,Range(0f,0.5f)]public  float spread;
    float x;
    float y;
    private void Update()
    {
        if ( !photonView.IsMine ) return;
        if ( timeBeforeShoots <= 0 )
        {
            if ( Input.GetMouseButton( 0 ) && itemGameObject.active && bulletsLeft>0 )
            {
                timeBeforeShoots = timeBetweenShoots;
                 x = Random.Range(-spread,spread);
                 y = Random.Range(-spread,spread);
                
                if(!shootingSound.isPlaying)
                    shootingSound.Play();
                
                photonView.RPC( "ShootAuto", RpcTarget.All );
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


    [PunRPC]
    IEnumerator ShootAuto()
    {



        RaycastHit2D hitInfo = Physics2D.Raycast( bulletSpawn.position, bulletSpawn.right + new Vector3(x,y,0));
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
        if ( lineRenderer != null )
            lineRenderer.enabled = true;
        yield return new WaitForSeconds( 0.02f );
        if ( lineRenderer != null )
            lineRenderer.enabled = false;
    }
    public void AddBullet( int addBullet )
    {
        SetAddBullet( addBullet );
    }
}
