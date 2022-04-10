using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;


public class PlayerManager : MonoBehaviour
{
    PhotonView photonView;

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
        PhotonNetwork.Instantiate( "Player", Vector3.zero, Quaternion.identity );
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
