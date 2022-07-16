using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LeaderBoardManager : MonoBehaviourPunCallbacks
{
    private bool isOpenLeaderboard = false;

    Player[] players = PhotonNetwork.PlayerList;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerLeaderBoardListItemPrefab;
    [SerializeField] GameObject panel;

    void Awake()
    {
        
            panel.SetActive( false );
        SetUpPlayer();
    }


    void Update()
    {
        if ( Input.GetKeyDown( KeyCode.Tab ) )
        {
            isOpenLeaderboard = !isOpenLeaderboard;
        }

        OpenLeaderboard();

        CloseLeaderboard();
        
    }

    public override void OnPlayerEnteredRoom( Player newPlayer )
    {
        Instantiate( playerLeaderBoardListItemPrefab, playerListContent ).GetComponent<PlayerLeaderboardListItem>().SetUp( newPlayer );
    }

    public void SetUpPlayer()
    {
        for (int i = 0;i< players.Length;i++ )
        {
             Instantiate( playerLeaderBoardListItemPrefab, playerListContent ).GetComponent<PlayerLeaderboardListItem>().SetUp(players[i]);
          
        }
    }

   

    private void OpenLeaderboard()
    {
        if ( isOpenLeaderboard )
            panel.SetActive( true );
    }
    private void CloseLeaderboard()
    {
        if ( !isOpenLeaderboard )
            panel.SetActive( false );
    }
}
