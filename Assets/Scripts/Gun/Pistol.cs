using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    public float offset;
    public GameObject Ammo;
    public Transform shotDir;

    public float startTime;
    private float timeShot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position;
        float rotateZ = Mathf.Atan2(difference.y,difference.x)*Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler( 0f, 0f, rotateZ + offset );

        if ( timeShot <= 0 )
        {
            if ( Input.GetMouseButton( 0 ) )
            {
                Instantiate( Ammo, shotDir.position, transform.rotation );
                timeShot = startTime;
            }
        }
        else
        {
            timeShot -= Time.deltaTime;
        }

        
    }
}
