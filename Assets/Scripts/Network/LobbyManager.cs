using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text TextName;
    [SerializeField] private Text TextLog;
    [SerializeField] private InputField inputField;

    private void Start()
    {
        inputField.text = PlayerPrefs.GetString( "name" );
        PhotonNetwork.NickName = TextName.text;
        Log( "welcome" );
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Log( "Connect" );
    }

    public void CreateRoom()
    {
        SaveName();
        Log( "Create" );
        PhotonNetwork.CreateRoom( null, new Photon.Realtime.RoomOptions { MaxPlayers = 10 } );
    }

    public void JoinRoom()
    {
        SaveName();
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinedRoom()
    {
        Log( "Join" );
        PhotonNetwork.LoadLevel( "Game" );
    }
    private void SaveName()
    {
        PlayerPrefs.SetString( "name", TextName.text );
        PhotonNetwork.NickName = PlayerPrefs.GetString( "name" );
    }
    private void Log(string message)
    {
        TextLog.text = message;
    }
}
