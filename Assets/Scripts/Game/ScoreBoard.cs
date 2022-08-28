using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using Hastable = ExitGames.Client.Photon.Hashtable;
public class ScoreBoard : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform container;
    [SerializeField] protected GameObject scoreboardItemPrefab;
    [SerializeField] protected CanvasGroup canvasGroup;
    [SerializeField] protected bool isEnd = false;
    [SerializeField] protected bool isEndGameCanvas = false;
    protected Dictionary<Player,PlayerLeaderboardListItem> scoreBoardItems = new Dictionary<Player, PlayerLeaderboardListItem>();
    
    private void Start()
    {
        
        InitScoreBoard();
    }
    protected virtual void InitScoreBoard()
    {
        foreach ( Player player in PhotonNetwork.PlayerList )
        {
            AddScoreboardItem( player );
        }
    }
    protected virtual void AddScoreboardItem(Player player )
    {
        PlayerLeaderboardListItem item = Instantiate(scoreboardItemPrefab,container).GetComponent<PlayerLeaderboardListItem>();
        item.SetUp( player );
        scoreBoardItems[player] = item;
    }
    public override void OnPlayerLeftRoom( Player otherPlayer )
    {
        
        RemoveScoreboardItem( otherPlayer );
    }
    protected virtual void RemoveScoreboardItem( Player player )
    {
        // scroreBoardItems[player].SetToDefault(player);
        try {

            Destroy( scoreBoardItems[player].gameObject );
            scoreBoardItems.Remove( player );
        }catch(System.Exception ex )
        {
            Debug.Log( ex.Message );
        }
    }
    protected virtual void BubbleSort( Player[] player)
    {
        int i = 0;
        bool t = true;
        while ( t )
        {
            t = false;
            for (int j = 0; j< player.Length - i - 1;j++ )
            {
                //scoreBoardItems[player[j]].ChangeNumber( j + 1 );

                float score_1 = scoreBoardItems[player[j]].GetScore();
                float score_2 = scoreBoardItems[player[j+1]].GetScore();

                var temp = scoreBoardItems[player[j]];
                if (score_1 <= score_2){
                   
                    scoreBoardItems[player[j]] = scoreBoardItems[player[j + 1]];
                    scoreBoardItems[player[j + 1]] = temp;

                    scoreBoardItems[player[j]].ChangeNumber( j+1 );
                    scoreBoardItems[player[j+1]].ChangeNumber( j + 2 );

                    int index1 = scoreBoardItems[player[j ]].transform.GetSiblingIndex();
                    int index2 = scoreBoardItems[player[j+1 ]].transform.GetSiblingIndex();

                    scoreBoardItems[player[j]].transform.SetSiblingIndex(index2);
                    scoreBoardItems[player[j+1]].transform.SetSiblingIndex( index1 );
                   
                    t = true;
                }    
            }
            i = i + 1;
        }
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
    public void IsEndGame()
    {
        isEnd = true;
        if(!isEndGameCanvas)
            canvasGroup.alpha = 0;
        //Debug.Log( "IsENDGAME" );
    }
    protected virtual void OpenLeaderBoard()
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
    

}
