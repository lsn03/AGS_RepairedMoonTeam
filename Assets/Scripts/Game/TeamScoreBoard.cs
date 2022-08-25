using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using Hastable = ExitGames.Client.Photon.Hashtable;

public class TeamScoreBoard : ScoreBoard
{
    [SerializeField] Transform blueContainer;
    [SerializeField] Transform redContainer;
    private void Start()
    {
        InitScoreBoard();
    }
    protected override void AddScoreboardItem( Player player )
    {
        Hastable playerProps = player.CustomProperties;

        object team;
        playerProps.TryGetValue( "team", out team );

        if ( ( string )team == "blue" )
        {
            PlayerLeaderboardListItem item = Instantiate(scoreboardItemPrefab,blueContainer).GetComponent<PlayerLeaderboardListItem>();
            item.SetUp( player );
            scoreBoardItems[player] = item;
        }
        else if ( ( string )team == "red" )
        {
            PlayerLeaderboardListItem item = Instantiate(scoreboardItemPrefab,redContainer).GetComponent<PlayerLeaderboardListItem>();
            item.SetUp( player );
            scoreBoardItems[player] = item;
        }

    }
    public override void OnPlayerLeftRoom( Player otherPlayer )
    {

        RemoveScoreboardItem( otherPlayer );
    }
    public override void OnPlayerEnteredRoom( Player newPlayer )
    {
        AddScoreboardItem( newPlayer );
    }
    private void Update()
    {
        if ( isEnd ) return;

        OpenLeaderBoard();
    }
    protected override void OpenLeaderBoard()
    {
        if ( !isEndGameCanvas )
        {
            if ( Input.GetKeyDown( KeyCode.Tab ) )
            {
                canvasGroup.alpha = 1;
            }
            else if ( Input.GetKeyUp( KeyCode.Tab ) )
            {
                canvasGroup.alpha = 0;
            }

            foreach ( var player in PhotonNetwork.PlayerList )
            {
                //Debug.Log( $"ScoreVBoardUpdate\t {scoreBoardItems[player].isChanged}" );
                if ( scoreBoardItems[player].isChanged )
                {
                    scoreBoardItems[player].isChanged = false;
                    BubbleSort( PhotonNetwork.PlayerList );

                }
            }
        }
    }
    // red 9C3A3A
}
