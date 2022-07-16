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
    GameObject leaderBoard;
    void Start()
    {
        if ( photonView.IsMine )
        {
            CreateController();
        }
       // leaderBoard = GetComponent<CountLeaderBoard>();
        DeathMenuManager.Instance.CloseDeathMenu();
    }
    void CreateController()
    {
        Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
        DeathMenuManager.Instance.CloseDeathMenu();
        controller = PhotonNetwork.Instantiate( "Player", spawnpoint.position, spawnpoint.rotation,0,new object[] {photonView.ViewID } );
    }
    // Update is called once per frame
    public void Die(string killer)
    {
        PhotonNetwork.Destroy( controller );
        sound.Play();
        DeathMenuManager.Instance.OpenDeathMenu();
        Invoke ( "CreateController",1f );
        //leaderBoard.AddKill( killer );
    }



}
