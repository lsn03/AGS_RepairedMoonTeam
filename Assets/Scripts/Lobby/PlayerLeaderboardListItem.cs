using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Hastable = ExitGames.Client.Photon.Hashtable;
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
        scores.text = "0";
        countOfKills.text = "0";
        countOfDeaths.text = "0";
        nickname.text = nick[0];

        UpdateStats();
    }
    public float GetScore()
    {
        return float.Parse( scores.text );
    }

    public void ChangeNumber( int number )
    {
        this.number.text = number.ToString();
    }
    private void ChangeScore()
    {
        int death =  int.Parse( countOfDeaths.text );
        int kills = int.Parse( countOfKills.text);
        if ( death == 0 && kills == 0 )
            scores.text = 0.ToString();
        else if ( kills > 0 && death == 0 )
        {
            scores.text = kills.ToString();
        } else if ( kills == 0 && death > 0 )
        {
            scores.text = "0";
        }
        else if(kills>0 && death>0)
        {
            scores.text = ( System.Math.Round( (float)kills / death ,2)).ToString();
        }
    }
   
    public override void OnPlayerPropertiesUpdate( Player targetPlayer, Hastable changedProps )
    {
     if(targetPlayer == player )
        {
            if ( changedProps.ContainsKey( "kills" ) || changedProps.ContainsKey( "deaths" ) )
            {
                UpdateStats();
            }
        }
    }
    void UpdateStats()
    {
        if ( player.CustomProperties.TryGetValue( "kills", out object kills ) )
        {
            countOfKills.text = kills.ToString();
        }
        if ( player.CustomProperties.TryGetValue( "deaths", out object death ) )
        {
            countOfDeaths.text = death.ToString();
        }
        ChangeScore();
    }

}
