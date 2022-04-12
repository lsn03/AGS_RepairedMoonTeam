using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGun : MonoBehaviour
{
    public GameObject Gun;
    public float offset;
    private PhotonView photonView;
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( !photonView.IsMine ) return;
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position;
        float rotateZ = Mathf.Atan2(difference.y,difference.x)*Mathf.Rad2Deg;

        Gun.transform.rotation = Quaternion.Euler( 0f, 0f, rotateZ + offset );
    }
}
