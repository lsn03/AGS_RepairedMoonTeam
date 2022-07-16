using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class CountLeaderBoard : MonoBehaviourPunCallbacks
{

    [SerializeField] Transform playerListContent;
    Player player;
    private string killer,dead;
    public void AddKill( string killer )
    {
        foreach( Transform child in playerListContent )
        {
            var player =  child.GetComponent<PlayerLeaderboardListItem>();

             this.killer =player.GetNickName();
            if(this.killer == killer )
            {
                player.ChangeCountOfKill( 1 );
                Debug.Log( "ADDDDDDDDDDDDDDDDDDDDDDDDDD" );
            }
        }
    }
    public void AddDeath( string dead )
    {
        foreach ( Transform child in playerListContent )
        {
            var player =  child.GetComponent<PlayerLeaderboardListItem>();
            this.dead = player.GetNickName();
            if ( this.dead == dead )
            {
                player.ChangeCountOfDeath( 1 );
                Debug.Log( "ADDDDDD" );
            }
        }
    }

}
