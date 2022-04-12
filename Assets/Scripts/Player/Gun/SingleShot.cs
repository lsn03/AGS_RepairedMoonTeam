using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShot : Gun
{

    [SerializeField] Camera cam;
    public Transform bulletSpawn;
    public GameObject Gun;
    public float offset;

    public override void Use()
    {
        Shoot();
    }
    void Shoot()
    {
        

        Debug.Log( "Using gun " + itemInfo.itemName );
        RaycastHit2D hitInfo = Physics2D.Raycast( bulletSpawn.position, bulletSpawn.right );
        
        if ( hitInfo )
        {
            Debug.Log( hitInfo.transform.name );
            //hitInfo.transform.GetComponent<>
        }
        //Ray2D ray =  Camera.main.ScreenToWorldPoint( Input.mousePosition ) - transform.position;
    }
}
