using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class ScoreBoard : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform container;
    [SerializeField] GameObject scoreboardItemPrefab;
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
    }

    public override void OnPlayerEnteredRoom( Player newPlayer )
    {
        AddScoreboardItem( newPlayer );
    }
}
