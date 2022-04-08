using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject player;
    private void Start()
    {
        PhotonNetwork.Instantiate( player.name, new Vector2(Random.Range(-6,6),-2 ),Quaternion.identity);
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
