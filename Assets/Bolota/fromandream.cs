using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class fromandream : MonoBehaviour
{
    DistanceJoint2D joint;

    Vector3 target;

    RaycastHit2D rayCast;

    public float distance = 15f;
    public LayerMask mask;

     public float offset;
    private PhotonView photonView;
    float localscale_y;
    float localscalePrev;
    float localscaleNext;
    

    void Start()
    {
    joint = GetComponent<DistanceJoint2D>();
    joint.enabled = false;
    
    
        photonView = GetComponent<PhotonView>();
           localscalePrev = localscale_y*-1f;
        localscaleNext = localscale_y;
    }

    void Update()

    {
                   
       
       
        if (Input.GetKeyDown(KeyCode.F))
        {
           Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position;
        float rotateZ = Mathf.Atan2(difference.y,difference.x)*Mathf.Rad2Deg;
            
            rayCast = Physics2D.Raycast(transform.position, target - transform.position, distance, mask);
            Vector3 LocalScale = transform.localScale;

             if ( rotateZ >90 || rotateZ <-90 )
        {

            LocalScale.y = localscalePrev ;
            transform.position = new Vector3( transform.position.x, transform.position.y, -1f );
        }
        else
        {
            LocalScale.y = localscaleNext;
            transform.position = new Vector3( transform.position.x, transform.position.y, -5f );
        }

            if (rayCast.collider != null)
            {
                joint.enabled = true;
                joint.connectedBody = rayCast.collider.gameObject.GetComponent<Rigidbody2D>();

                joint.distance = Vector2.Distance(transform.position, rayCast.point);
            }
        }

         if (Input.GetKeyUp(KeyCode.F))
         {
            joint.enabled = false;
         }
  
    }

}
