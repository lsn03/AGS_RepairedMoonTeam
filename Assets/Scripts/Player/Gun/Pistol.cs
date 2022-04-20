using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    public float offset;
    public GameObject bullet;
    public Transform bulletSpawn;
    public GameObject Gun;
    public float startTime;
    private float timeShoot;

    private PhotonView photonView;

    PlayerController player;
  
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        player = GetComponent<PlayerController>();
        
        
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if ( !photonView.IsMine ) return;
    //    Gun.SetActive( true );
    //    Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position;
    //    float rotateZ = Mathf.Atan2(difference.y,difference.x)*Mathf.Rad2Deg;

    //    Gun.transform.rotation = Quaternion.Euler( 0f, 0f, rotateZ + offset );


    //}

    private void Update()
    {
        if ( timeShoot <= 0 )
        {
            if ( Input.GetMouseButtonDown( 0 ) && itemGameObject.active )
            {
                Shoot();
                timeShoot = startTime;

            }
        }
        else
        {
            timeShoot -= Time.deltaTime;
        }
    }

    public override void Use()
    {
        
    }
    public void Shoot()
    {
        
        {
            //if ( Input.GetMouseButton( 0 ) )
            {
                PhotonNetwork.Instantiate( bullet.name, bulletSpawn.position, bulletSpawn.transform.rotation );
               // timeShot = startTime;
            }
        }
        
    }
}
