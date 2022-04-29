using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text text;
    Player player;
    public void SetUp( Player _player)
    {
        player = _player;
        string[] nick = _player.NickName.Split('\t');
        text.text = nick[0];
    }
    public override void OnPlayerLeftRoom( Player otherPlayer )
    {
        if(player == otherPlayer )
        {
            Destroy( gameObject );
        }
    }
    public override void OnLeftRoom()
    {
        Destroy( gameObject );
    }
}
