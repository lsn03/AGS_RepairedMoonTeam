using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LeaderBoardManager : MonoBehaviourPunCallbacks
{
    private bool isOpenLeaderboard = false;

    Player[] players = PhotonNetwork.PlayerList;
    [SerializeField] Transform PlayerListContent;

    void Start()
    {
        if ( gameObject.active )
        {
            CloseLeaderboard();
        }
    }
    

    void Update()
    {
        if ( Input.GetKeyDown( KeyCode.Tab ) )
        {
            isOpenLeaderboard = !isOpenLeaderboard;
        }
        if ( isOpenLeaderboard )
        {
            OpenLeaderboard();
        }
        else
        {
            CloseLeaderboard();
        }
    }
    private void OpenLeaderboard()
    {
        gameObject.SetActive( true );
    }
    private void CloseLeaderboard()
    {
        gameObject.SetActive( false );
    }
}
