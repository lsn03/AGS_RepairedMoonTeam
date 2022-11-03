using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGun : MonoBehaviour
{
    public GameObject Gun;
    public float offset;
    private PhotonView photonView;
    float localscale_y;
    float localscalePrev;
    float localscaleNext;


    void Start()
    {
        photonView = GetComponent<PhotonView>();
        localscale_y = Gun.transform.localScale.y;
        localscalePrev = localscale_y*-1f;
        localscaleNext = localscale_y;
    }
     
    // Update is called once per frame
    void Update()
    {

        //if ( !photonView.IsMine ) return;
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position;
        float rotateZ = Mathf.Atan2(difference.y,difference.x)*Mathf.Rad2Deg;

        Gun.transform.rotation = Quaternion.Euler( 0f, 0f, rotateZ + offset );

        Vector3 LocalScale = Gun.transform.localScale;
        
        if ( rotateZ >90 || rotateZ <-90 )
        {

            LocalScale.y = localscalePrev ;
            Gun.transform.position = new Vector3( Gun.transform.position.x, Gun.transform.position.y, -1f );
        }
        else
        {
            LocalScale.y = localscaleNext;
            Gun.transform.position = new Vector3( Gun.transform.position.x, Gun.transform.position.y, -5f );
        }
        Gun.transform.localScale = LocalScale;
    }
}
