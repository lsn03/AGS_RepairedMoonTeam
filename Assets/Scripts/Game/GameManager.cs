using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;


public class GameManager : MonoBehaviourPunCallbacks
{
    //[SerializeField] private GameObject player;

    //[SerializeField] public Transform Spawn;
    [SerializeField]PauseMenuManager menu;
    public void Leave()
    {
        menu.ResumeButtonClick();
        Time.timeScale = 1;
        PhotonNetwork.LeaveRoom();

    }
    public override void OnLeftRoom()
    {
        RoomManager.Destroy( this.gameObject );
        SceneManager.LoadScene( "Lobby" );
        //MenuManager.Instance.OpenMenu( "title" );

    }
}
