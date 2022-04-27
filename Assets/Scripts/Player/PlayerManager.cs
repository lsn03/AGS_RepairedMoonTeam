using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;


public class PlayerManager : MonoBehaviour
{
    PhotonView photonView;
    GameObject controller;
    [SerializeField] AudioSource sound;
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
        Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
        controller = PhotonNetwork.Instantiate( "Player", spawnpoint.position, spawnpoint.rotation,0,new object[] {photonView.ViewID } );
    }
    // Update is called once per frame
    public void Die()
    {
        PhotonNetwork.Destroy( controller );
        sound.Play();
        Invoke ( "CreateController",1f );
    }
}
