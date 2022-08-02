using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using Hastable = ExitGames.Client.Photon.Hashtable;
public class ScoreBoard : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform container;
    [SerializeField] GameObject scoreboardItemPrefab;
    [SerializeField] CanvasGroup canvasGroup;

    Dictionary<Player,PlayerLeaderboardListItem> scroreBoardItems = new Dictionary<Player, PlayerLeaderboardListItem>();

    private void Start()
    {
        foreach ( Player player in PhotonNetwork.PlayerList )
        {
            AddScoreboardItem( player );
        }
    }

    void AddScoreboardItem(Player player )
    {
        PlayerLeaderboardListItem item = Instantiate(scoreboardItemPrefab,container).GetComponent<PlayerLeaderboardListItem>();
        item.SetUp( player );
        scroreBoardItems[player] = item;
    }
    public override void OnPlayerLeftRoom( Player otherPlayer )
    {
        
        RemoveScoreboardItem( otherPlayer );
    }
    void RemoveScoreboardItem(Player player )
    {
       // scroreBoardItems[player].SetToDefault(player);
        Destroy( scroreBoardItems[player].gameObject );
        scroreBoardItems.Remove( player );
    }
    public override void OnPlayerEnteredRoom( Player newPlayer )
    {
        AddScoreboardItem( newPlayer );
    }
    private void Update()
    {
        if ( Input.GetKeyDown( KeyCode.Tab ) )
        {
            canvasGroup.alpha = 1;
        }
        else if(Input.GetKeyUp(KeyCode.Tab))
        {
            canvasGroup.alpha = 0;
        }
    }
}