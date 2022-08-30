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
    string team = null;
    string gameMode = null;
    void Start()
    {
        if ( photonView.IsMine )
        {
            PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue( "team", out object obj );
            team = ( string )obj;
            PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue( "gamemode", out object gameMode );
            this.gameMode = ( string )gameMode;
            Debug.Log( this.gameMode );
            CreateController();
            
            //Debug.Log( (string)team );
        }
        
        
        DeathMenuManager.Instance.CloseDeathMenu();
    }
    void CreateController()
    {
        if (team == "blue" )
        {
            Transform spawnpoint = SpawnManager.Instance.GetTeamDeathMatchBlueSpawnpoint();
            controller = PhotonNetwork.Instantiate( "Player", spawnpoint.position, spawnpoint.rotation, 0, new object[] { photonView.ViewID } );

        }
        else if (team == "red" )
        {
            Transform spawnpoint = SpawnManager.Instance.GetTeamDeathMatchRedSpawnpoint();
            controller = PhotonNetwork.Instantiate( "Player", spawnpoint.position, spawnpoint.rotation, 0, new object[] { photonView.ViewID } );

        }
        else
        {
            if ( this.gameMode == "deathMatch" )
            {
                Transform spawnpoint = SpawnManager.Instance.GetDeathMatchSpawnpoint();
                controller = PhotonNetwork.Instantiate( "Player", spawnpoint.position, spawnpoint.rotation, 0, new object[] { photonView.ViewID } );

            }
            else
            {

                DeathMenuManager.Instance.OpenChooseTeamMenu();
            }

        }

        DeathMenuManager.Instance.CloseDeathMenu();
    }
    private void Update()
    {
        if ( photonView.IsMine )
        {
            if ( gameMode != "deathMatch" && team == null )
            {


                team = DeathMenuManager.Instance.team;

                if ( team == "blue" )
                {
                    Transform spawnpoint = SpawnManager.Instance.GetTeamDeathMatchBlueSpawnpoint();
                    controller = PhotonNetwork.Instantiate( "Player", spawnpoint.position, spawnpoint.rotation, 0, new object[] { photonView.ViewID } );
                    DeathMenuManager.Instance.CloseChooseTeamMenu();
                    Hastable ht = new Hastable();
                    ht.Add( "team", "blue" );
                    PhotonNetwork.LocalPlayer.SetCustomProperties( ht );
                    return;
                }
                else if ( team == "red" )
                {
                    Transform spawnpoint = SpawnManager.Instance.GetTeamDeathMatchRedSpawnpoint();
                    controller = PhotonNetwork.Instantiate( "Player", spawnpoint.position, spawnpoint.rotation, 0, new object[] { photonView.ViewID } );
                    DeathMenuManager.Instance.CloseChooseTeamMenu();
                    Hastable ht = new Hastable();
                    ht.Add( "team", "red" );
                    PhotonNetwork.LocalPlayer.SetCustomProperties( ht );
                    return;
                }


            }
        }
            
    }
    // Update is called once per frame
    public void Die(Player player,string weapon = null)
    {
        if ( !photonView.IsMine ) return;
        deaths++;
        Debug.Log( "Мертвый считается :\t " + PhotonNetwork.LocalPlayer.NickName );
        Hastable hash = new Hastable();
        hash.Add( "deaths", deaths );
        PhotonNetwork.LocalPlayer.SetCustomProperties( hash );

        PhotonNetwork.Destroy( controller );
        sound.Play();

       
        SetUpPlayerOnKillFeed( player.NickName.Split( '\t' )[0], weapon, PhotonNetwork.LocalPlayer.NickName.Split( '\t' )[0] );
        DeathMenuManager.Instance.OpenDeathMenu(player.NickName.Split('\t')[0]);
        Invoke( "CreateController", 1f );



    }
    public void SetUpPlayerOnKillFeed( string killer, string nameOfGun, string killed )
    {
        //KillFeedManager.Instance.SetUpPlayer( killer,nameOfGun,killed );

        Hastable ht = PhotonNetwork.CurrentRoom.CustomProperties;
        string temp = killer+"\t"+nameOfGun+"\t"+killed;
        object obj = temp;
        ht.Remove( "killAnouncer" );
        ht.Add( "killAnouncer", obj );
        PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
    }
    public void Die()
    {
        if ( !photonView.IsMine ) return;
        deaths++;
        Debug.Log( "Мертвый считается :\t " + PhotonNetwork.LocalPlayer.NickName );
        Hastable hash = new Hastable();
        hash.Add( "deaths", deaths );
        PhotonNetwork.LocalPlayer.SetCustomProperties( hash );

        PhotonNetwork.Destroy( controller );
        sound.Play();
        DeathMenuManager.Instance.OpenDeathMenu();
        Invoke ( "CreateController",1f );

        

    }

    public void GetKill()
    {
        photonView.RPC( nameof( RPC_GetKill ), photonView.Owner );
    }

    [PunRPC]
    void RPC_GetKill()
    {
        
        if ( !photonView.IsMine ) return;
        kills++;
        Debug.Log( "Килл засчитан: \t" + PhotonNetwork.LocalPlayer.NickName );
        Hastable hash = new Hastable();
        hash.Add( "kills", kills );
        PhotonNetwork.LocalPlayer.SetCustomProperties( hash );
    }

    public static PlayerManager Find(Player player )
    {
        return FindObjectsOfType<PlayerManager>().SingleOrDefault( x => x.photonView.Owner == player );
    }

}
