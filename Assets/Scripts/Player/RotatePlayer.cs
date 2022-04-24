using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlayer : MonoBehaviour
{

    private PhotonView photonView;
    Camera camera;
    Vector3 pos;
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( !photonView.IsMine ) return;
        
        pos = Camera.main.WorldToScreenPoint( transform.position );
        flip();
    }
    void flip()
    {
        if ( Input.mousePosition.x < pos.x )
        {
            transform.localRotation = Quaternion.Euler( 0f, 180f, 0f );
        }
        if ( Input.mousePosition.x > pos.x )
        {
            transform.localRotation = Quaternion.Euler( 0f, 0f, 0f );
        }
    }
}
