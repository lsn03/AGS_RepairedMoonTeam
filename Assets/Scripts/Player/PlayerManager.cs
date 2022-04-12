using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;


public class PlayerManager : MonoBehaviour
{
    PhotonView photonView;
    GameObject controller;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Start()
    {
        if ( photonView.IsMine )
        {
            CreateController();
        }
    }
    void CreateController()
    {
        controller = PhotonNetwork.Instantiate( "Player", Vector3.zero, Quaternion.identity,0,new object[] {photonView.ViewID } );
    }
    // Update is called once per frame
    public void Die()
    {
        PhotonNetwork.Destroy( controller );
        CreateController();
    }
}
