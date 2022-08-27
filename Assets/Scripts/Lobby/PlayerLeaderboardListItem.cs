using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Hastable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerLeaderboardListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text number;
    [SerializeField] TMP_Text nickname;
    [SerializeField] TMP_Text scores;
    [SerializeField] TMP_Text countOfKills;
    [SerializeField] TMP_Text countOfDeaths;
    [SerializeField] Image colorImage;
    public string team { get; private set; }
    Player player;
    public void SetUp( Player _player )
    {
        player = _player;
        string[] nick = _player.NickName.Split('\t');
        scores.text = "0";
        countOfKills.text = "0";
        countOfDeaths.text = "0";
        nickname.text = nick[0];
        number.text = PhotonNetwork.PlayerList.Length.ToString();
        
        Hastable ht = player.CustomProperties;
        object obj;
        ht.TryGetValue( "team", out obj );
        team = ( string )obj;
        if(team == "blue" )
        {
            colorImage.color = Color.blue;
        }else if ( team == "red" )
        {
            colorImage.color = Color.red;
        }
        else
        {
            var temp = colorImage.color;
            temp.a = 0f;
            colorImage.color = temp;
        }

        UpdateStats();
        isChanged = false;
    }
   
    public float GetScore()
    {
        return float.Parse( scores.text );
    }

    public void ChangeNumber( int number )
    {
        this.number.text = number.ToString();
        Debug.Log( "NumberChanged\t"+ number);
    }
    public bool isChanged { get; set; }
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
        isChanged = true;

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
    private void Update()
    {
       // Debug.Log( $"isChanged in leaderboardListItem Update {isChanged}" );
    }
}
