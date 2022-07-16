using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class PlayerLeaderboardListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text number;
    [SerializeField] TMP_Text nickname;
    [SerializeField] TMP_Text scores;
    [SerializeField] TMP_Text countOfKills;
    [SerializeField] TMP_Text countOfDeaths;
    Player player;
    public void SetUp( Player _player )
    {
        player = _player;
        string[] nick = _player.NickName.Split('\t');
        this.number.text = ""; //number.ToString();
        nickname.text = nick[0];
        scores.text = "0";
        countOfKills.text = "0";
        countOfDeaths.text = "0";
    }
    public void ChangeCountOfKill( int countOfKills )
    {
        int cnt = int.Parse( this.countOfKills.text);
         cnt+= countOfKills;
        this.countOfKills.text = cnt.ToString();
        ChangeScore();
    }
    public void ChangeCountOfDeath( int countOfDeaths )
    {
        int cnt = int.Parse( this.countOfDeaths.text);
        cnt += countOfDeaths;
        this.countOfDeaths.text = cnt.ToString();
        ChangeScore();
    }

    private void ChangeScore()
    {
        scores.text = (int.Parse( countOfKills.text ) * 10 - int.Parse( countOfDeaths.text ) * 2).ToString();
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
    public string GetNickName()
    {
        return nickname.text;
    }

}
