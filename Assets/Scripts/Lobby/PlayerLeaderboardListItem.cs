using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class PlayerLeaderboardListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text number { get; set; }
    [SerializeField] TMP_Text nickname;
    [SerializeField] TMP_Text scores;
    [SerializeField] TMP_Text countOfKills;
    [SerializeField] TMP_Text countOfDeaths;
    Player player;
    public void SetUp( Player _player )
    {
        player = _player;
        string[] nick = _player.NickName.Split('\t');
        number.text = "0";
        nickname.text = nick[0];
        scores.text = "0";
        countOfKills.text = "0";
        countOfDeaths.text = "0";
    }
    public override void OnPlayerLeftRoom( Player otherPlayer )
    {
        if ( player == otherPlayer )
        {
            Destroy( gameObject );
        }
    }
    public override void OnLeftRoom()
    {
        Destroy( gameObject );
    }
}
