using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShot : Gun
{

    [SerializeField] Camera cam;
    public Transform bulletSpawn;

    public override void Use()
    {
        Shoot();
    }
    void Shoot()
    {
       
        RaycastHit2D hitInfo = Physics2D.Raycast( bulletSpawn.position, bulletSpawn.right );
        
        if ( hitInfo )
        {
            Debug.Log( hitInfo.transform.name );
            hitInfo.collider.gameObject.GetComponent<IDamage>()?.TakeDamage(((GunIno)itemInfo).damage);
        }
        
    }
}
