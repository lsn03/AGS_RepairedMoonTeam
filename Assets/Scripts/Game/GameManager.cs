using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;


public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject player;

    [SerializeField] public Transform Spawn;

    



    public void Start()
    {
        var SpawnPoint = new Vector3[Spawn.childCount];
        for ( int j = 0; j < Spawn.childCount; j++ )
            SpawnPoint[j] = Spawn.GetChild( j ).transform.position;
        Vector2 _ = SpawnPoint[Random.Range(0,Spawn.childCount)];
        PhotonNetwork.Instantiate( player.name, new Vector2(_.x,_.y  ),Quaternion.identity);
    }
    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene( "Lobby" );
    }
}
