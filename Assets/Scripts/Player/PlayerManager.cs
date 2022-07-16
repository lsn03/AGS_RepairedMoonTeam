using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;
using System.Linq;
using Hastable = ExitGames.Client.Photon.Hashtable;


public class PlayerManager : MonoBehaviour
{
    PhotonView photonView;
    GameObject controller;
    [SerializeField] AudioSource sound;

    int kills,deaths;
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
        
        
        DeathMenuManager.Instance.CloseDeathMenu();
    }
    void CreateController()
    {
        Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
        DeathMenuManager.Instance.CloseDeathMenu();
        controller = PhotonNetwork.Instantiate( "Player", spawnpoint.position, spawnpoint.rotation,0,new object[] {photonView.ViewID } );
    }
    // Update is called once per frame
    public void Die()
    {
        PhotonNetwork.Destroy( controller );
        sound.Play();
        DeathMenuManager.Instance.OpenDeathMenu();
        Invoke ( "CreateController",1f );

        if ( !photonView.IsMine ) return;
        deaths++;

        Hastable hash = new Hastable();
        hash.Add( "deaths", deaths );
        PhotonNetwork.LocalPlayer.SetCustomProperties( hash );

    }
    public void GetKill()
    {
        photonView.RPC( nameof( RPC_GetKill ), RpcTarget.All );
    }

    [PunRPC]
    void RPC_GetKill()
    {
        if ( !photonView.IsMine ) return;
        kills++;

        Hastable hash = new Hastable();
        hash.Add( "kills", kills );
        PhotonNetwork.LocalPlayer.SetCustomProperties( hash );
    }

    public static PlayerManager Find(Player player )
    {
        return FindObjectsOfType<PlayerManager>().SingleOrDefault( x => x.photonView.Owner == player );
    }

}
