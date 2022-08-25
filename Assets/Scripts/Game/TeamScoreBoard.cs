using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using Hastable = ExitGames.Client.Photon.Hashtable;
using System;

public class TeamScoreBoard : ScoreBoard
{
    [SerializeField] Transform blueContainer;
    [SerializeField] Transform redContainer;
    List<Player> redPlayer = new List<Player>();
    List<Player> bluePlayer = new List<Player>();
    Player[]redPlayerList,bluePlayerList;
    private void Start()
    {
        InitScoreBoard();
        foreach(Player player in PhotonNetwork.PlayerList )
        {
            object team;
            player.CustomProperties.TryGetValue( "team", out team );
            if((string)team == "blue" )
            {
                bluePlayer.Add( player );
            }else if ((string)team == "red" )
            {
                redPlayer.Add( player );
            }

        }
        ChangeCountOfRedOrBlue();


    }
    private void ChangeCountOfRedOrBlue()
    {
        redPlayerList = new Player[redPlayer.Count];
        for ( int i = 0; i < redPlayer.Count; i++ )
        {
            redPlayerList[i] = redPlayer[i];
        }
        bluePlayerList = new Player[bluePlayer.Count];
        for ( int i = 0; i < bluePlayer.Count; i++ )
        {
            bluePlayerList[i] = bluePlayer[i];
        }
    }
    public void AddPlayerAfterChooseTeam( Player player )
    {
        Debug.Log( nameof( AddPlayerAfterChooseTeam ) );
        AddScoreboardItem( player );
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
            if ( !bluePlayer.Contains( player ) )
            {
                ChangeCountOfRedOrBlue();
            }
        }
        else if ( ( string )team == "red" )
        {
            PlayerLeaderboardListItem item = Instantiate(scoreboardItemPrefab,redContainer).GetComponent<PlayerLeaderboardListItem>();
            item.SetUp( player );
            scoreBoardItems[player] = item;
            if ( !redPlayer.Contains( player ) )
            {
                ChangeCountOfRedOrBlue();
            }
        }

    }
    public override void OnPlayerPropertiesUpdate( Player targetPlayer, Hastable changedProps )
    {
       
        if ( changedProps.ContainsKey( "team" ) )
        {
            Debug.Log( "InIfOnPlayer..." );
            changedProps.TryGetValue( "team", out object team );
            if( (string)team == "blue" && !bluePlayer.Contains( targetPlayer ) )
            {
                AddPlayerAfterChooseTeam( targetPlayer );
            }
            else if ( ( string )team == "red" && !redPlayer.Contains( targetPlayer ) )
            {
                AddPlayerAfterChooseTeam( targetPlayer );
            }
        }


    }
    public override void OnPlayerLeftRoom( Player otherPlayer )
    {

        RemoveScoreboardItem( otherPlayer );

        redPlayer.Remove( otherPlayer );
        bluePlayer.Remove( otherPlayer );
        ChangeCountOfRedOrBlue();

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

            foreach ( var player in bluePlayer )
            {
                //Debug.Log( $"ScoreVBoardUpdate\t {scoreBoardItems[player].isChanged}" );
                try
                {
                   // if ( scoreBoardItems[player].isChanged )
                    {
                        if ( scoreBoardItems[player].team == "blue" )
                        {
                            scoreBoardItems[player].isChanged = false;
                            
                            BubbleSort( bluePlayerList );
                            
                        }


                    }
                }
                catch(Exception ex )
                {
                    Debug.Log( ex.Message );
                }
            }
            foreach ( var player in redPlayer )
            {
                //Debug.Log( $"ScoreVBoardUpdate\t {scoreBoardItems[player].isChanged}" );
                try
                {
                    //if ( scoreBoardItems[player].isChanged )
                    {
                        if ( scoreBoardItems[player].team == "red" )
                        {
                            scoreBoardItems[player].isChanged = false;
                            
                            BubbleSort( redPlayerList );
                            NumerateList( redContainer );
                        }


                    }
                }
                catch ( Exception ex )
                {
                    Debug.Log( ex.Message );
                }
            }
            NumerateList( blueContainer );
            NumerateList( redContainer );
        }
        else
        {
            NumerateList( blueContainer );
            NumerateList( redContainer );
            BubbleSort( redPlayerList );
            BubbleSort( bluePlayerList );
        }
    }

    private void NumerateList(Transform container)
    {
        int i = 0;
       foreach ( Transform child in container )
        {
            i += 1;
            PlayerLeaderboardListItem item = child.GetComponent<PlayerLeaderboardListItem>();
            item.ChangeNumber(i);
        }
    }
   


    // red 9C3A3A
}
