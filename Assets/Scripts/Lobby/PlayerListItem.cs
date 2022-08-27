using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using Hastable = ExitGames.Client.Photon.Hashtable;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text text;
    Player player;
    string team = "";
    public void SetUp( Player _player)
    {
        player = _player;
        string[] nick = _player.NickName.Split('\t');
        text.text = nick[0];
    }
    public Player GetPlayer() { return player; }
    public void SetTeam( string team )
    {
        this.team = team;
        Hastable ht = new Hastable();
        ht.Add( "team", team );
        PhotonNetwork.LocalPlayer.SetCustomProperties(ht);
    }
    public override void OnPlayerLeftRoom( Player otherPlayer )
    {
        if(player == otherPlayer )
        {
            Destroy( gameObject );
            // PhotonNetwork.LocalPlayer.CustomProperties.Clear();
        }
    }
    public override void OnLeftRoom()
    {
        Destroy( gameObject );
        // PhotonNetwork.LocalPlayer.CustomProperties.Clear();
    }
}
